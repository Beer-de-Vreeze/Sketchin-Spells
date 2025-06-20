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
        if (PlayerhealthChangedEvent == null)
        {
            PlayerhealthChangedEvent = new UnityEvent<int>();
        }
        // Also initialize the base health event
        if (healthChangedEvent == null)
        {
            healthChangedEvent = new UnityEvent<int>();
        }
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

        PlayerhealthChangedEvent.Invoke(CurrentHealth);
        Debug.Log("Player Health: " + CurrentHealth + "/" + MaxHealth + " (" + name + ")");
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
        Debug.Log("Resetting health");
        CurrentHealth = MaxHealth;
        PlayerhealthChangedEvent.Invoke(CurrentHealth);
    }
}
