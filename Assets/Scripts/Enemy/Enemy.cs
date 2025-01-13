using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemySO b_enemyData;
    private HealthManagerSO m_healthManager;


    void Start()
    {
        m_healthManager = ScriptableObject.CreateInstance<HealthManagerSO>();
        m_healthManager.b_maxHealth = b_enemyData.b_maxHealthSO;
        m_healthManager.OnEnable();
    }


    public void TakeDamage(int damage)
    {
        m_healthManager.TakeDamage(damage);
    }

    public void DamageOverTime(int damage, int duration)
    {
        m_healthManager.DamageOverTime(damage, duration);
    }

    public void Heal(int amount)
    {
        m_healthManager.Heal(amount);
    }
}
