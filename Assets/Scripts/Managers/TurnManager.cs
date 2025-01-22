using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TurnManager : Singleton<TurnManager>
{
    private Enemy currentEnemy;
    private int m_ManaRegen = 2;
    private int m_spellCount = 0;

    public UnityEvent OnPlayerTurnStart = new UnityEvent();
    public UnityEvent OnPlayerTurnEnd = new UnityEvent();
    public UnityEvent OnEnemyTurnStart = new UnityEvent();
    public UnityEvent OnEnemyTurnEnd = new UnityEvent();

    private Player player;

    private void OnEnable() 
    {
        OnPlayerTurnStart.AddListener(StartPlayerTurn);
        OnPlayerTurnEnd.AddListener(EndPlayerTurn);
        OnEnemyTurnStart.AddListener(StartEnemyTurn);
        OnEnemyTurnEnd.AddListener(EndEnemyTurn);
    }

    private void Start()
    {
        player = GameManager.Instance.b_Player.GetComponent<Player>();
        WaveManager.Instance.StartNextWave();
    }

    public void SetCurrentEnemy(Enemy enemy)
    {
        currentEnemy = enemy;
    }

    public void StartPlayerTurn()
    {
        Debug.Log("Player Turn Started");
        foreach(Button button in UIManager.Instance.b_playerUI.spellButtons)
        {
            button.interactable = true;
        }
        UIManager.Instance.b_playerUI.b_endTurnButton.interactable = true;
    }

    public void EndPlayerTurn()
    {
        Debug.Log("Player Turn Ended");
        StartEnemyTurn();
        foreach(Button button in UIManager.Instance.b_playerUI.spellButtons)
        {
            button.interactable = false;
        }
        UIManager.Instance.b_playerUI.b_endTurnButton.interactable = false;
    }

    public void StartEnemyTurn()
    {
        OnPlayerTurnStart.Invoke();
    }

    public void EndEnemyTurn()
    {
        OnEnemyTurnEnd.Invoke();
        StartPlayerTurn();
        player.b_mana.RestoreMana(2);
    }

    public void EndBattle()
    {
        //make the player draw a spell using the spellSO
        m_spellCount++;
        player.b_mana.RestoreMana(15);
        GameManager.Instance.StartDialogue(m_spellCount, SketchType.Player, UIManager.Instance.b_playerUI.spells[2].b_spellName, UIManager.Instance.b_playerUI.spells[2].b_description);
    }
}
