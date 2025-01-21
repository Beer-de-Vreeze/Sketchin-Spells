using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class Inventory : Singleton<Inventory>
{
    public List<ItemSO> b_inventory = new();
    public ItemSO EquipedArtifact;
    public int b_gold;

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
            case ItemType.Artifact:
                EquipArtifact(item);
                break;
            case ItemType.Potion:
                AddPotion(item);
                break;
            case ItemType.Spell:
                AddSpell(item);
                break;
            default:
                b_inventory.Add(item);
                break;
        }
    }

    private void EquipArtifact(ItemSO item)
    {
        if (EquipedArtifact != null)
        {
            Debug.Log("You already have an artifact equipped");
        }
        else
        {
            item.Equip();
            EquipedArtifact = item;
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

    private void AddSpell(ItemSO item)
    {
        if (b_inventory.Count >= 5)
        {
            Debug.Log("SpellBook is full");
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
    }

    public void RemoveGold(int gold)
    {
        b_gold -= gold;
    }
    
    public void EquipItem(ItemSO item)
    {
        item.Equip();
    }
}
