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

    public void OnEnable()
    {
        CurrentHealth = MaxHealth;
        if (healthChangedEvent == null)
        {
            healthChangedEvent = new UnityEvent<int>();
        }
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        healthChangedEvent.Invoke(CurrentHealth);
        Debug.Log("Health: " + CurrentHealth + name);
    }

    public void DamageOverTime(int damage, int duration)
    {
        for (int i = 0; i < duration; i++)
        {
            TakeDamage(damage);
        }
    }

    public void Heal(int amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        healthChangedEvent.Invoke(CurrentHealth);
    }

    public void SetMaxHealth(int amount)
    {
        MaxHealth = amount;
    }

    public int GetCurrentHealth()
    {
        return CurrentHealth;
    }

    public void Reset()
    {
        CurrentHealth = MaxHealth;
        healthChangedEvent.Invoke(CurrentHealth);
    }
}
