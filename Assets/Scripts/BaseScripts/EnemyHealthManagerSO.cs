using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(
    fileName = "New EnemyHealth Manager",
    menuName = "Base/New EnemyHealthManager",
    order = 1
)]
public class EnemyHealthManagerSO : HealthManagerSO
{
    [System.NonSerialized]
    public UnityEvent<int> EnemyHealthChangedEvent;

    public override void OnEnable()
    {
        CurrentHealth = MaxHealth;
        if (healthChangedEvent == null)
        {
            EnemyHealthChangedEvent = new UnityEvent<int>();
        }
    }

    public override void DamageOverTime(int damage, int duration)
    {
        for (int i = 0; i < duration; i++)
        {
            TakeDamage(damage);
        }
    }

    public override void Heal(int amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        EnemyHealthChangedEvent.Invoke(CurrentHealth);
    }

    public override void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        EnemyHealthChangedEvent.Invoke(CurrentHealth);
        Debug.Log("Health: " + CurrentHealth + name);
    }

    public override void SetMaxHealth(int amount)
    {
        MaxHealth = amount;
    }

    public override int GetCurrentHealth()
    {
        return CurrentHealth;
    }

    public override void Reset()
    {
        CurrentHealth = MaxHealth;
        EnemyHealthChangedEvent.Invoke(CurrentHealth);
    }
}
