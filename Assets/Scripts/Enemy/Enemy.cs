using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public EnemySO b_enemyData;
    public HealthManagerSO m_healthManager;

    private void OnEnable() 
    {
        TurnManager.Instance.OnEnemyTurnStart.AddListener(OnTurnStart);
    }

    private void OnTurnStart()
    {
        b_enemyData.Attack(this.gameObject, GameManager.Instance.b_Player.gameObject);
    }


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
