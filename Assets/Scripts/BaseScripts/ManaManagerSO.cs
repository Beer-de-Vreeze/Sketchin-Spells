using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ManaManagerSO", menuName = "Base/New ManaManagerSO")]
public class ManaManagerSO : ScriptableObject
{
    public int MaxMana = 10;
    public int CurrentMana = 10;
    public UnityEvent<int> ManaChangedEvent;

    public void OnEnable()
    {
        CurrentMana = MaxMana;
        if (ManaChangedEvent == null)
        {
            ManaChangedEvent = new UnityEvent<int>();
        }
    }

    public void UseMana(int amount)
    {
        CurrentMana -= amount;
        if (CurrentMana < 0)
        {
            CurrentMana = 0;
        }
        ManaChangedEvent.Invoke(CurrentMana);
    }

    public void RestoreMana(int amount)
    {
        CurrentMana += amount;
        if (CurrentMana > MaxMana)
        {
            CurrentMana = MaxMana;
        }
        ManaChangedEvent.Invoke(CurrentMana);
    }

    public void SetMaxMana(int amount)
    {
        MaxMana = amount;
    }

    public int GetCurrentMana()
    {
        return CurrentMana;
    }

    public void Reset()
    {
        CurrentMana = MaxMana;
        ManaChangedEvent.Invoke(CurrentMana);
    }
}
