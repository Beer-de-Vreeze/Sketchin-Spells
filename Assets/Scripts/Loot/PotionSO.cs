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

    public PotionType b_potionType;
    public int b_healAmount;


    public override void Use()
    {
        base.Use();
        switch (b_potionType)
        {
            case PotionType.Health: 
            this.GetComponent<HealthManagerSO>().Heal(b_healAmount);
                break;
            case PotionType.Mana:
            this.GetComponent<ManaManagerSO>().RestoreMana(b_healAmount);
                break;
        }
    }
}
