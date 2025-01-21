using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : Singleton<TurnManager>
{
    enum Turn
    {
        Player,
        Enemy
    }

    private Turn currentTurn;

    public UnityEvent OnPlayerTurnStart = new UnityEvent();
    public UnityEvent OnPlayerTurnEnd = new UnityEvent();
    public UnityEvent OnEnemyTurnStart = new UnityEvent();
    public UnityEvent OnEnemyTurnEnd = new UnityEvent();

    private void Start()
    {
        OnPlayerTurnStart.AddListener(StartPlayerTurn);
        OnPlayerTurnEnd.AddListener(EndPlayerTurn);
        OnEnemyTurnStart.AddListener(StartEnemyTurn);
        OnEnemyTurnEnd.AddListener(EndEnemyTurn);
        currentTurn = Turn.Player;
        OnPlayerTurnStart.Invoke();
    }

    public void StartPlayerTurn()
    {
        Debug.Log("Player Turn Started");
        Player player = GameManager.Instance.b_Player.GetComponent<Player>();
        player.m_isTurn = true;
    }

    public void EndPlayerTurn()
    {
        Debug.Log("Player Turn Ended");
        currentTurn = Turn.Enemy;
        OnEnemyTurnStart.Invoke();
    }

    public void StartEnemyTurn()
    {
        Debug.Log("Enemy Turn Started");
        OnEnemyTurnEnd.Invoke();
    }

    public void EndEnemyTurn()
    {
        Debug.Log("Enemy Turn Ended");
        currentTurn = Turn.Player;
        OnPlayerTurnStart.Invoke();
    }
}
