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
        if (item.b_itemType == ItemType.Gold)
        {
            AddGold(item.b_value);
            return;
        }
        else if(item.b_itemType == ItemType.Artifact)
        {
            if (EquipedArtifact != null)
            {
                Debug.Log("You already have an artifact equipped");
                return;
            }
            else
            {
               item.Equip();
            }
        }
        //add a max of 2 potions
        if (item.b_itemType == ItemType.Potion)
        {
            if (b_inventory.Count >= 2)
            {
                Debug.Log("Inventory is full");
                return;
            }
            else
            {
                b_inventory.Add(item);
            }
        }
        if (item.b_itemType == ItemType.Spell)
        {
            if (b_inventory.Count >= 5)
            {
                Debug.Log("SpellBook is full");
                return;
            }
            else
            {
                b_inventory.Add(item);
            }
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
