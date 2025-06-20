using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TurnManager : Singleton<TurnManager>
{
    public Enemy CurrentEnemy;
    private int _manaRegen = 2;
    private int _spellCount = 0;
    private bool _isPlayerTurnActive = false;

    public UnityEvent OnPlayerTurnStart = new UnityEvent();
    public UnityEvent OnPlayerTurnEnd = new UnityEvent();
    public UnityEvent OnEnemyTurnStart = new UnityEvent();
    public UnityEvent OnEnemyTurnEnd = new UnityEvent();

    public void SetCurrentEnemy(Enemy enemy)
    {
        CurrentEnemy = enemy;
    }

    public void StartPlayerTurn()
    {
        _isPlayerTurnActive = true;
        Debug.Log("Player Turn Started");
        foreach (Button button in UIManager.Instance.PlayerUI.SpellButtons)
        {
            if (
                button.GetComponent<SpellButton>().Spell.SpellData.ManaCost
                    <= GameManager.Instance.Player.Mana.CurrentMana
                && button.GetComponent<SpellButton>().IsUnlocked
            )
            {
                button.interactable = true;
            }
        }
        UIManager.Instance.PlayerUI.EndTurnButton.interactable = true;
    }

    public void EndPlayerTurn()
    {
        _isPlayerTurnActive = false;
        Debug.Log("Player Turn Ended");
        UIManager.Instance.PlayerUI.EndTurnButton.interactable = false;
        StartEnemyTurn();
    }

    public void StartEnemyTurn()
    {
        Debug.Log("Enemy Turn Started");
        OnEnemyTurnStart.Invoke();
        // Add logic for enemy actions here
        EndEnemyTurn();
    }

    public void EndEnemyTurn()
    {
        Debug.Log("Enemy Turn Ended");
        OnPlayerTurnStart.Invoke();
        GameManager.Instance.Player.Mana.RestoreMana(_manaRegen);
        _isPlayerTurnActive = false;
    }

    private void HandleEnemyTurnEnd()
    {
        StartPlayerTurn();
    }

    public void StartBattle()
    {
        OnPlayerTurnStart.AddListener(StartPlayerTurn);
        OnPlayerTurnEnd.AddListener(EndPlayerTurn);
        OnEnemyTurnEnd.AddListener(HandleEnemyTurnEnd);

        // Ensure the current enemy is set as the target for player spells
        if (CurrentEnemy != null && UIManager.Instance.PlayerUI != null)
        {
            UIManager.Instance.PlayerUI.SetTarget(CurrentEnemy.gameObject);
            Debug.Log($"Set target to {CurrentEnemy.name} for battle");
        }

        StartPlayerTurn();
    }

    public IEnumerator EndBattle()
    {
        yield return new WaitForSeconds(2);
        UIManager.Instance.CloseGameCanvas();
        UIManager.Instance.OpenEndGameCanvas();

        //     GameManager.Instance.Player.GetComponent<Player>().Health.Reset();
        //     GameManager.Instance.Player.GetComponent<Player>().Mana.Reset();
        //     if (_spellCount == UIManager.Instance.PlayerUI.SpellButtons.Count)
        //     {
        //         WaveManager.Instance.SpawnEnemy(1);
        //         Enemy enemy = GameObject.FindWithTag("Enemy").GetComponent<Enemy>();
        //         // All spells unlocked, play dialogue number 6 and sketch the Dark Lord
        //         yield return GameManager.Instance.StartDialogueAndSketch(
        //             6,
        //             SketchType.Enemy,
        //             enemy.EnemyData.EnemyName,
        //             enemy.EnemyData.Description
        //         );
        //         //wait until sketch is saved
        //         WaitUntil waitUntil = new WaitUntil(() => enemy.Sketch != null);

        //         StartBattle();

        //         CurrentEnemy.OnDeath.AddListener(EndGame);
        //     }
        //     else
        //     {
        //         _spellCount++;
        //         GameManager.Instance.Player.Mana.RestoreMana(2);

        //         // Start dialogue and sketching sequence for new spell
        //         yield return GameManager.Instance.StartDialogueAndSketch(
        //             4,
        //             SketchType.Spell,
        //             UIManager
        //                 .Instance.PlayerUI.SpellButtons[_spellCount]
        //                 .GetComponent<SpellButton>()
        //                 .Spell.SpellData.SpellName,
        //             UIManager
        //                 .Instance.PlayerUI.SpellButtons[_spellCount]
        //                 .GetComponent<SpellButton>()
        //                 .Spell.SpellData.Description
        //         );

        //         UIManager
        //             .Instance.PlayerUI.SpellButtons[_spellCount]
        //             .GetComponent<SpellButton>()
        //             .UnlockSpell();
        //         UIManager
        //             .Instance.PlayerUI.SpellButtons[_spellCount]
        //             .GetComponent<SpellButton>()
        //             .Spell.LoadSprite();

        //         yield return new WaitUntil(
        //             () =>
        //                 UIManager
        //                     .Instance.PlayerUI.SpellButtons[_spellCount]
        //                     .GetComponent<SpellButton>()
        //                     .Spell.Sketch != null
        //         );

        //         WaveManager.Instance.SpawnEnemy(0);
        //         //load enemy sprite
        //         StartBattle();
        //     }
        //     yield return null;
    }

    public void EndGame()
    {
        UIManager.Instance.CloseAllCanvas();
        UIManager.Instance.OpenEndGameCanvas();
    }

    public void HandleEnemyDeath()
    {
        if (WaveManager.Instance.CurrentWaveIndex == 5)
        {
            WaveManager.Instance.SpawnEnemy(1);
            if (CurrentEnemy != null)
            {
                CurrentEnemy.OnDeath.AddListener(EndGame);
            }
        }
        else
        {
            _spellCount++;
            GameManager.Instance.Player.Mana.RestoreMana(2);

            if (_spellCount < UIManager.Instance.PlayerUI.SpellButtons.Count)
            {
                StartCoroutine(HandleNewSpellUnlock());
            }
            else
            {
                Debug.LogWarning("No more spell buttons to unlock.");
            }

            if (CurrentEnemy != null)
            {
                Destroy(CurrentEnemy.gameObject);
            }
            WaveManager.Instance.SpawnEnemy(0);
            StartBattle();
        }
    }

    private IEnumerator HandleNewSpellUnlock()
    {
        yield return GameManager.Instance.StartDialogueAndSketch(
            4,
            SketchType.Spell,
            UIManager
                .Instance.PlayerUI.SpellButtons[_spellCount]
                .GetComponent<SpellButton>()
                .Spell.SpellData.SpellName,
            UIManager
                .Instance.PlayerUI.SpellButtons[_spellCount]
                .GetComponent<SpellButton>()
                .Spell.SpellData.Description
        );

        UIManager
            .Instance.PlayerUI.SpellButtons[_spellCount]
            .GetComponent<SpellButton>()
            .UnlockSpell();
        UIManager
            .Instance.PlayerUI.SpellButtons[_spellCount]
            .GetComponent<SpellButton>()
            .Spell.LoadSprite();

        yield return new WaitUntil(
            () =>
                UIManager
                    .Instance.PlayerUI.SpellButtons[_spellCount]
                    .GetComponent<SpellButton>()
                    .Spell.Sketch != null
        );
    }

    public void ResetTurnManager()
    {
        CurrentEnemy = null;
        _spellCount = 0;
    }
}
