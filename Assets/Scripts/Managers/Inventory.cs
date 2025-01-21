using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class Inventory : Singleton<Inventory>
{
    public List<ItemSO> b_inventory = new();
    public int b_gold;
    public UnityEvent<int> m_GoldChangedEvent;

    void Start()
    {
        b_gold = 0;
    }

    public void AddItem(ItemSO item)
    {
        switch (item.b_itemType)
        {
            case ItemType.Gold:
                AddGold(item.b_value);
                break;
            case ItemType.Potion:
                AddPotion(item);
                break;
            default:
                b_inventory.Add(item);
                break;
        }
    }


    private void AddPotion(ItemSO item)
    {
        if (b_inventory.Count >= 2)
        {
            Debug.Log("Inventory is full");
        }
        else
        {
            b_inventory.Add(item);
        }
    }

    public void UseItem(ItemSO item)
    {
        item.Use();
    }

    public void AddGold(int gold)
    {
        b_gold += gold;
        m_GoldChangedEvent.Invoke(b_gold);
    }

    public void RemoveGold(int gold)
    {
        b_gold -= gold;
        m_GoldChangedEvent.Invoke(b_gold);
    }
}
