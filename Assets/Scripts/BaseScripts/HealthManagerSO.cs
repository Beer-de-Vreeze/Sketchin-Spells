using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Health Manager", menuName = "Base/New HealthManager", order = 1)]
public class HealthManagerSO : ScriptableObject
{
    public int MaxHealth = 100;
    public int CurrentHealth = 100;

    [System.NonSerialized]
    public UnityEvent<int> healthChangedEvent;

    public virtual void OnEnable()
    {
        CurrentHealth = MaxHealth;
        if (healthChangedEvent == null)
        {
            healthChangedEvent = new UnityEvent<int>();
        }
    }

    public virtual void TakeDamage(int damage)
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

        healthChangedEvent.Invoke(CurrentHealth);
        Debug.Log("Health: " + CurrentHealth + "/" + MaxHealth + " (" + name + ")");
    }

    public virtual void DamageOverTime(int damage, int duration)
    {
        // For now, just apply the base damage once
        // TODO: Implement proper damage over time with coroutines
        TakeDamage(damage);
        Debug.Log(
            $"DamageOverTime called with {damage} damage for {duration} duration - applying {damage} damage once for now"
        );
    }

    public virtual void Heal(int amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        healthChangedEvent.Invoke(CurrentHealth);
    }

    public virtual void SetMaxHealth(int amount)
    {
        MaxHealth = amount;
    }

    public virtual int GetCurrentHealth()
    {
        return CurrentHealth;
    }

    public virtual void Reset()
    {
        CurrentHealth = MaxHealth;
        healthChangedEvent.Invoke(CurrentHealth);
    }
}
