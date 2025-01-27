using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TurnManager : Singleton<TurnManager>
{
    public Enemy CurrentEnemy;
    private int _manaRegen = 2;
    private int _spellCount = 0;

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
        Debug.Log("Player Turn Ended");
        foreach (Button button in UIManager.Instance.PlayerUI.SpellButtons)
        {
            button.interactable = false;
        }
        UIManager.Instance.PlayerUI.EndTurnButton.interactable = false;
        OnEnemyTurnStart.Invoke();
    }

    public void EndEnemyTurn()
    {
        Debug.Log("Enemy Turn Ended");
        OnPlayerTurnStart.Invoke();
        GameManager.Instance.Player.Mana.RestoreMana(_manaRegen);
    }

    private void HandleEnemyTurnEnd()
    {
        StartPlayerTurn();
    }

    public void StartBattle()
    {
        StartPlayerTurn();
            OnPlayerTurnStart.AddListener(StartPlayerTurn);
        OnPlayerTurnEnd.AddListener(EndPlayerTurn);
        OnEnemyTurnEnd.AddListener(HandleEnemyTurnEnd); // Change listener to a new method
    }

    public void EndBattle()
    {
        if (WaveManager.Instance.CurrentWaveIndex == 5)
        {
            WaveManager.Instance.SpawnEnemy(1);
            CurrentEnemy.OnDeath.AddListener(EndGame);
        }
        else
        {
            _spellCount++;
            GameManager.Instance.Player.Mana.RestoreMana(2);
            GameManager.Instance.StartDialogueAndSketch(
                4,
                sketchType: SketchType.Spell,
                name: UIManager
                    .Instance.PlayerUI.SpellButtons[_spellCount]
                    .GetComponent<SpellButton>()
                    .Spell.SpellData.SpellName,
                description: UIManager
                    .Instance.PlayerUI.SpellButtons[_spellCount]
                    .GetComponent<SpellButton>()
                    .Spell.SpellData.Description
            );
            UIManager
                .Instance.PlayerUI.SpellButtons[_spellCount]
                .GetComponent<SpellButton>()
                .UnlockSpell();
            GameManager.Instance.Player.GetComponent<Player>().Health.Reset();
        }
    }

    public void EndGame()
    {
        UIManager.Instance.CloseAllCanvas();
        UIManager.Instance.CloseEndGameCanvas();
    }

    public void HandleEnemyDeath()
    {
        if (WaveManager.Instance.CurrentWaveIndex == 5)
        {
            WaveManager.Instance.SpawnEnemy(1);
            CurrentEnemy.OnDeath.AddListener(EndBattle);
        }
        else
        {
            _spellCount++;
            GameManager.Instance.Player.Mana.RestoreMana(2);
            GameManager.Instance.StartDialogueAndSketch(
                4,
                sketchType: SketchType.Spell,
                name: UIManager
                    .Instance.PlayerUI.SpellButtons[_spellCount]
                    .GetComponent<SpellButton>()
                    .Spell.SpellData.SpellName,
                description: UIManager
                    .Instance.PlayerUI.SpellButtons[_spellCount]
                    .GetComponent<SpellButton>()
                    .Spell.SpellData.Description
            );
            UIManager
                .Instance.PlayerUI.SpellButtons[_spellCount]
                .GetComponent<SpellButton>()
                .UnlockSpell();
            WaveManager.Instance.SpawnEnemy(0);
        }
        StartBattle();
        OnPlayerTurnStart.AddListener(StartPlayerTurn);
        OnPlayerTurnEnd.AddListener(EndPlayerTurn);
        OnEnemyTurnEnd.AddListener(HandleEnemyTurnEnd);
    }

    public void ResetTurnManager()
    {
        CurrentEnemy = null;
        _spellCount = 0;
    }
}
