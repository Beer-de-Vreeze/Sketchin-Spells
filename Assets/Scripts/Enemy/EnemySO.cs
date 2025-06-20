using System.IO;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BaseEnemy", menuName = "Create Enemy")]
public class EnemySO : ScriptableObject
{
    public string EnemyName;
    public string Description;

    public int MaxHealth;

    public Spell Attack;
    public UnityEvent OnAttack = new UnityEvent();

    public void Cast(GameObject caster, GameObject target)
    {
        Attack.ApplySpellEffect(caster, target);
    }
}
