using System.IO;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BaseEnemy", menuName = "Create Enemy")]
public class EnemySO : ScriptableObject
{
    public string EnemyName;
    public string Description;
    public int MaxHealthSO;

    public Spell Attack;
    public UnityEvent OnAttack = new UnityEvent();

    public void Cast(GameObject caster, GameObject target)
    {
        Attack.ApplySpellEffect(caster, target);
        TurnManager.Instance.EndEnemyTurn();
    }
}
