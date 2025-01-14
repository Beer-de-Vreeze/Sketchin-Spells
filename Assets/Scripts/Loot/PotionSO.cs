using UnityEngine;

[CreateAssetMenu(fileName = "BasePotion", menuName = "Item/Potion")]
public class PotionSO : ItemSO
{
    public enum PotionType
    {
        Health,
        Mana,
    }
    public int b_healAmount;

    public override void Use()
    {
        base.Use();
        Debug.Log("Potion used");
    }
}
