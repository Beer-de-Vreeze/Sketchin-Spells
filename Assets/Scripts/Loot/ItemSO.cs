using Unity.VisualScripting;
using UnityEditor.AssetImporters;
using UnityEngine;

public enum ItemType
{
    Consumable,
    Spell,
    Artifact,
    Potion,
    Gold
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

public class ItemSO : ScriptableObject 
{ 
    public string b_itemName;
    public string b_description;
    public ItemType b_itemType;
    public Rarity b_rarity;
    public Sprite b_icon;
    public int b_value;



    public void Collect()
    {
        Inventory.Instance.AddItem(this);
    }

    public virtual void Use()
    {
        Debug.Log($"{b_itemName} used");
        Inventory.Instance.b_inventory.Remove(this);
    }
    public void Sell()
    {
        Inventory.Instance.AddGold(b_value);
        Inventory.Instance.b_inventory.Remove(this);
    }


    public void Equip()
    {
        Inventory.Instance.EquipedArtifact = this;
    }
}
