using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : Singleton<Inventory>
{
    public List<ItemSO> PInventory = new();
    public int Gold;
    public UnityEvent<int> GoldChangedEvent;

    void Start()
    {
        Gold = 0;
    }

    public void AddItem(ItemSO item)
    {
        switch (item.ItemType)
        {
            case ItemType.Gold:
                AddGold(item.Value);
                break;
            case ItemType.Potion:
                AddPotion(item);
                break;
            default:
                PInventory.Add(item);
                break;
        }
    }

    private void AddPotion(ItemSO item)
    {
        if (PInventory.Count >= 2)
        {
            Debug.Log("Inventory is full");
        }
        else
        {
            PInventory.Add(item);
        }
    }

    public void UseItem(ItemSO item)
    {
        item.Use();
    }

    public void AddGold(int gold)
    {
        Gold += gold;
        GoldChangedEvent.Invoke(Gold);
    }

    public void RemoveGold(int gold)
    {
        Gold -= gold;
        GoldChangedEvent.Invoke(Gold);
    }
}
