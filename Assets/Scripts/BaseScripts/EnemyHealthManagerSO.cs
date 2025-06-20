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
        if (EnemyHealthChangedEvent == null)
        {
            EnemyHealthChangedEvent = new UnityEvent<int>();
        }
        // Also initialize the base health event
        if (healthChangedEvent == null)
        {
            healthChangedEvent = new UnityEvent<int>();
        }
    }

    public override void DamageOverTime(int damage, int duration)
    {
        // For now, just apply the base damage once
        // TODO: Implement proper damage over time with coroutines
        TakeDamage(damage);
        Debug.Log(
            $"DamageOverTime called with {damage} damage for {duration} duration - applying {damage} damage once for now"
        );
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
        // Validate damage input
        if (damage < 0)
        {
            Debug.LogWarning("Negative damage value received: " + damage + ". Using 0 instead.");
            damage = 0;
        }

        CurrentHealth -= damage;

        // Ensure health doesn't go below 0
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }

        // Invoke both events for compatibility
        EnemyHealthChangedEvent.Invoke(CurrentHealth);
        healthChangedEvent.Invoke(CurrentHealth);
        Debug.Log("Enemy Health: " + CurrentHealth + "/" + MaxHealth + " (" + name + ")");
    }

    public override void SetMaxHealth(int amount)
    {
        MaxHealth = amount;
        CurrentHealth = MaxHealth;
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
