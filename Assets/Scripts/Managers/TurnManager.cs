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
        StartPlayerTurn();
    }

    public void EndGame()
    {
        UIManager.Instance.CloseAllCanvas();
        UIManager.Instance.OpenEndGameCanvas();
    }

    public void HandleEnemyDeath()
    {
        _spellCount++;
        GameManager.Instance.Player.Mana.RestoreMana(2);

        if (_spellCount < UIManager.Instance.PlayerUI.SpellButtons.Count)
        {
            StartCoroutine(HandleNewSpellUnlock());
            WaveManager.Instance.SpawnEnemy(0);
            GameManager.Instance.Player.Mana.Reset();
            GameManager.Instance.Player.Health.Reset();
            StartBattle();
        }
        else
        {
            Debug.LogWarning("No more spell buttons to unlock.");
            // WaveManager.Instance.SpawnEnemy(1); // Commented out boss spawning
            // StartCoroutine(StartFinalDialogue()); // Commented out final dialogue
            StartCoroutine(StartEndlessMode());
        }

        if (CurrentEnemy != null)
        {
            Destroy(CurrentEnemy.gameObject);
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

    private IEnumerator StartFinalDialogue()
    {
        yield return GameManager.Instance.StartDialogueAndSketch(
            5,
            SketchType.Enemy,
            CurrentEnemy.EnemyData.EnemyName,
            CurrentEnemy.EnemyData.Description
        );
    }

    private IEnumerator StartEndlessMode()
    {
        yield return new WaitUntil(() => CurrentEnemy == null);
        while (true)
        {
            int randomEnemy = Random.Range(0, 2);
            WaveManager.Instance.SpawnEnemy(randomEnemy);
            yield return new WaitUntil(() => CurrentEnemy == null);
        }
    }

    public void ResetTurnManager()
    {
        CurrentEnemy = null;
        _spellCount = 0;
    }
}
