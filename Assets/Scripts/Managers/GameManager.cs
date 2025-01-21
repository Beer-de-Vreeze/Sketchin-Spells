using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{     
    public Player b_Player;


    void Start()
    {
        b_Player = FindFirstObjectByType<Player>();
    }

    public void DrawPlayerSketch()
    {
        UIManager.Instance.OpenSketchCanvas(SketchType.Player, "Player","You");
    }

    public void DrawEnemySketch(EnemySO enemy)
    {
        UIManager.Instance.OpenSketchCanvas(SketchType.Enemy, enemy.name, enemy.b_description);
    }

    public void DrawSpellSketch(SpellSO spell)
    {
        UIManager.Instance.OpenSketchCanvas(SketchType.Spell, spell.name, spell.b_description);
    }
}