using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "BasePotion", menuName = "Item/Potion")]
public class PotionSO : ItemSO
{
    public enum PotionType
    {
        Health,
        Mana,
    }

    public PotionType Potion;
    public int HealAmount;

    public override void Use()
    {
        base.Use();
        switch (Potion)
        {
            case PotionType.Health:
                this.GetComponent<HealthManagerSO>().Heal(HealAmount);
                break;
            case PotionType.Mana:
                this.GetComponent<ManaManagerSO>().RestoreMana(HealAmount);
                break;
        }
    }
}
