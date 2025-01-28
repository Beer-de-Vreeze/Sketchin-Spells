using Unity.VisualScripting;
using UnityEngine;

public enum ItemType
{
    Consumable,
    Spell,
    Artifact,
    Potion,
    Gold,
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
}

public class ItemSO : ScriptableObject, CollectInteface
{
    public string ItemName;
    public string Description;
    public ItemType ItemType;
    public Rarity Rarity;
    public Sprite Icon;
    public int Value;

    public void Collect()
    {
        Inventory.Instance.AddItem(this);
    }

    public virtual void Use()
    {
        Debug.Log($"{ItemName} used");
        Inventory.Instance.PInventory.Remove(this);
    }

    public void Sell()
    {
        Inventory.Instance.AddGold(Value);
        Inventory.Instance.PInventory.Remove(this);
    }
}
