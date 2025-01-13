using UnityEngine;

[CreateAssetMenu(fileName = "BasePotion", menuName = "Item/Potion")]
public class PotionSO : ScriptableObject
{
    [Header("Potion Information")]
    public string b_potionName;
    public string b_description;
    public Sprite b_icon = Resources.Load<Sprite>("DefaultPotionIcon");

    [Header("Potion Stats")]
    public int b_amount;
    public int b_cooldown;
    public bool b_isConsumable;

    public void ApplyPotionEffect(GameObject target)
    {
        // Implement potion effect logic
    }
}
