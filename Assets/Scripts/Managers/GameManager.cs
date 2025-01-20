using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{     
    public GameObject b_Player;
    public List<Enemy> b_enemies = new List<Enemy>();

    void Start()
    {
        b_Player = GameObject.Find("Player");
    }

    public void AddEnemyToList(Enemy script)
    {
        b_enemies.Add(script);
    }

    public void RemoveEnemyFromList(Enemy script)
    {
        b_enemies.Remove(script);
    }
}
