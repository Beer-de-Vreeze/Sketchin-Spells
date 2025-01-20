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

    public UnityEvent StartTurn = new UnityEvent();
    public UnityEvent EndTurn = new UnityEvent();

    private void Start()
    {
        StartTurn.AddListener(StartPlayerTurn);
        EndTurn.AddListener(EndPlayerTurn);
        StartTurn.AddListener(StartEnemyTurn);
        EndTurn.AddListener(EndEnemyTurn);
        currentTurn = Turn.Player;
        StartTurn.Invoke();
    }


    public void StartPlayerTurn()
    {
        Debug.Log("Player Turn Started");
    }

    public void EndPlayerTurn()
    {
        Debug.Log("Player Turn Ended");
        currentTurn = Turn.Enemy;
        StartTurn.Invoke();
    }

    public void StartEnemyTurn()
    {
        Debug.Log("Enemy Turn Started");
    }

    public void EndEnemyTurn()
    {
        Debug.Log("Enemy Turn Ended");
        currentTurn = Turn.Player;
        StartTurn.Invoke();
    }

}
