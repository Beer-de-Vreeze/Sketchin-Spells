using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Wave", menuName = "Wave", order = 1)]
public class Wave : ScriptableObject
{
    public Queue<GameObject> b_enemies;
    public Transform b_spawnPoint;
}

