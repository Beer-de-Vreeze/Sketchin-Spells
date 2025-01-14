using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public enum SpellElement
{
    Fire,
    Ice,
    Earth,
    Lightning,
    DarkMagic,
    Support
}
public enum SpellType
{
    Projectile,
    Melee,
    Heal,
    SelfCast,
    Targeted,
    Line,
    Area
}

public enum SpellEffect
{
    Stunning,
    Buff,
    Debuff,
    DOT
}

[CreateAssetMenu(fileName = "BaseSpell", menuName = "Spells/Create Spell")]
public class SpellSO : ScriptableObject
{
    [Header("Spell Information")]
    public string b_spellName;
    public string b_description;
    public Sprite b_icon = Resources.Load<Sprite>("DefaultSpellIcon");

    [Header("Spell Stats")]
    public float b_damage;
    public int b_manaCost;
    public int b_amount;
    public int b_cooldown;

    [Header("Spell Type")]
    public SpellType b_spellType;

    [Header("Spell Effects")]
    public SpellEffect b_spellEffect;

    public void ApplySpellEffect(GameObject caster, GameObject target)
    {
        switch (b_spellType)
        {
            case SpellType.SelfCast:
                // Implement self cast logic	
                break;
            case SpellType.Targeted when true:
                // Implement targeted logic
                break;
            case SpellType.Melee:
                // Implement melee logic
                break;
            case SpellType.Projectile:
                // Implement projectile logic
                break;
            case SpellType.Area:
                // Implement area effect logic
                break;
            case SpellType.Line:
                // Implement line effect logic
                break;
            case SpellType.Heal:
                // Implement heal logic
                break;
            default:
                // Implement default logic
                break;
        }

        switch (b_spellEffect)
        {
            case SpellEffect.Buff:
                // Implement buff logic
                break;
            case SpellEffect.Debuff:
                // Implement debuff logic
                break;
            case SpellEffect.Stunning:
                // Implement stun logic
                break;
            case SpellEffect.DOT:
                // Implement DOT logic
                break;
            default:
                // Implement default logic
                break;
        }
    }

    public void LoadSprite()
    {
        string path = Path.Combine(Application.persistentDataPath, "sketches", "Spells", b_spellName + ".png");
        if(File.Exists(path) && path != Resources.Load<Sprite>("DefaultSpellIcon").name)
        {
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
            b_icon = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            b_icon = null;

            // open the sketcher window so the player can draw a custom spell for the game
            Sketcher.Instance.OpenSketcher(SketchType.Spell, b_spellName);
            Sketcher.Instance.OnImageSaved += (path) =>
            {
                LoadSprite();
            };
        }
    }
}
