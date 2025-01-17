using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ManaManagerSO", menuName = "Base/New ManaManagerSO")]
public class ManaManagerSO : ScriptableObject
{
    public int b_maxMana = 10;
    public int b_currentMana = 10;
    public UnityEvent<int> b_manaChangedEvent;
    public void OnEnable()
    {
        b_currentMana = b_maxMana;
        if (b_manaChangedEvent == null)
        {
            b_manaChangedEvent = new UnityEvent<int>();
        }
    }   

    public void UseMana(int amount)
    {
        b_currentMana -= amount;
        if (b_currentMana < 0)
        {
            b_currentMana = 0;
        }
        b_manaChangedEvent.Invoke(b_currentMana);
    }

    public void RestoreMana(int amount)
    {
        b_currentMana += amount;
        if (b_currentMana > b_maxMana)
        {
            b_currentMana = b_maxMana;
        }
        b_manaChangedEvent.Invoke(b_currentMana);
    }

    public void SetMaxMana(int amount)
    {
        b_maxMana = amount;
    }
}
