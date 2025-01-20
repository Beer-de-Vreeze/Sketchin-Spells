using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemySO b_enemyData;
    public HealthManagerSO m_healthManager;


    void Start()
    {
        
        m_healthManager = ScriptableObject.CreateInstance<HealthManagerSO>();
        m_healthManager.SetMaxHealth(b_enemyData.b_maxHealthSO);
        m_healthManager.OnEnable();
    }

    void Update()
    {
        if (m_healthManager.b_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
