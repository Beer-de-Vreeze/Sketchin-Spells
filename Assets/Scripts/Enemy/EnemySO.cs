using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseEnemy", menuName = "Enemies/Create Enemy")]
public class EnemySO : ScriptableObject
{
    public string b_enemyName;
    public string b_description;
    public Image b_sketch;
    public int b_maxHealthSO;
    private HealthManagerSO m_health;
    public SpellSO b_attack;

    private void Start() 
    {
        m_health.b_maxHealth = b_maxHealthSO;
    }

    public void Attack(GameObject caster, GameObject target)
    {
        b_attack.ApplySpellEffect(caster, target);
    }
}
