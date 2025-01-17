using System.IO;
using UnityEngine;

public enum SpellElement
{
    Fire,
    Ice,
    Earth,
    Lightning,
    DarkMagic,
    Support
}

public enum SpellTarget
{
    Self,
    Enemy,
}

public enum SpellType
{
    Projectile,
    Melee,
    Heal,
    Line,
    Area
}

public enum SpellEffect
{
    Stunning,
    Buff,
    Debuff,
    DOT,
    None
}

[CreateAssetMenu(fileName = "BaseSpell", menuName = "Spells/Create Spell")]
public class SpellSO : ScriptableObject
{
    [Header("Spell Information")]
    public string b_spellName;
    public string b_description;
    public Sprite b_icon; //Resources.Load<Sprite>("DefaultSpellIcon");

    [Header("Spell Stats")]
    public int b_damage;
    public int b_manaCost;
    public int b_amount;
    public int b_cooldown;

    [Header("Spell Type")]
    public SpellType b_spellType;
    public SpellTarget b_spellTarget;
    public SpellElement b_spellElement;

    [Header("Spell Effects")]
    public SpellEffect b_spellEffect;

    public void ApplySpellEffect(GameObject caster, GameObject target)
    {
        if (b_spellTarget == SpellTarget.Self)
        {
            target = caster;
        }

        Enemy enemy = target.GetComponent<Enemy>();
        HealthManagerSO healthManager = target.GetComponent<HealthManagerSO>();

        // Apply spell type effects
        switch (b_spellType)
        {
            case SpellType.Melee:
            case SpellType.Projectile:
                if (enemy != null)
                {
                    enemy.m_healthManager.TakeDamage(b_damage);
                }
                break;
            case SpellType.Area:
            case SpellType.Line:

                break;
            case SpellType.Heal:
                if (healthManager != null)
                {
                    healthManager.Heal(b_damage);
                }
                break;
            default:
                break;
        }

        // Apply spell element effects
        switch (b_spellElement)
        {
            case SpellElement.Fire:
                // Implement fire logic
                break;
            case SpellElement.Ice:
                // Implement ice logic
                break;
            case SpellElement.Earth:
                // Implement earth logic
                break;
            case SpellElement.Lightning:
                // Implement lightning logic
                break;
            case SpellElement.DarkMagic:
                // Implement dark magic logic
                break;
            case SpellElement.Support:
                // Implement support logic
                break;
            default:
                break;
        }

        // Apply spell effect
        switch (b_spellEffect)
        {
            case SpellEffect.Buff:
                if (healthManager != null)
                {
                    healthManager.Heal(b_damage);
                }
                break;
            case SpellEffect.Debuff:
            case SpellEffect.Stunning:
                if (enemy != null)
                {
                    enemy.m_healthManager.TakeDamage(b_damage);
                }
                break;
            case SpellEffect.DOT:
                if (enemy != null)
                {
                    enemy.m_healthManager.DamageOverTime(b_damage, b_amount);
                }
                break;
            case SpellEffect.None:
                break;
        }
    }

    public void LoadSprite()
    {
        string path = Path.Combine(
            Application.persistentDataPath,
            "sketches",
            "Spells",
            b_spellName + ".png"
        );
        if (File.Exists(path) && path != Resources.Load<Sprite>("DefaultSpellIcon").name)
        {
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
            b_icon = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f)
            );
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
