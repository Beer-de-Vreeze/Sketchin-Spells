using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Wave", order = 1)]
public class Wave : ScriptableObject
{
    public GameObject Enemy;
    public Transform SpawnPoint;
}
