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
}
