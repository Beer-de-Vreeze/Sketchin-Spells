using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Health Manager", menuName = "Base/New HealthManager", order = 1)]
public class PlayerHealthManagerSO : HealthManagerSO
{
    [System.NonSerialized]
    public UnityEvent<int> PlayerhealthChangedEvent;

    public override void OnEnable()
    {
        CurrentHealth = MaxHealth;
        if (healthChangedEvent == null)
        {
            PlayerhealthChangedEvent = new UnityEvent<int>();
        }
    }

    public override void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        PlayerhealthChangedEvent.Invoke(CurrentHealth);
        Debug.Log("Health: " + CurrentHealth + name);
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
        PlayerhealthChangedEvent.Invoke(CurrentHealth);
    }

    public override void SetMaxHealth(int amount)
    {
        MaxHealth = amount;
        PlayerhealthChangedEvent.Invoke(MaxHealth);
    }

    public override int GetCurrentHealth()
    {
        return CurrentHealth;
    }

    private void Update()
    {
        if (CurrentHealth >= MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }

    public override void Reset()
    {
        PlayerhealthChangedEvent.Invoke(MaxHealth);
    }
}
