using System;
using System.Collections;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

// public enum SpellElement
// {
//     Fire,
//     Ice,
//     Earth,
//     Lightning,
//     DarkMagic,
//     Support
// }

public enum SpellTarget
{
    Self,
    Target,
}

public enum SpellType
{
    Projectile,
    Melee,
    Heal,
    Shield
}

// public enum SpellEffect
// {
//     Stunning,
//     Buff,
//     Debuff,
//     DOT,
//     None
// }

[CreateAssetMenu(fileName = "BaseSpell", menuName = "Create Spell")]
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

    [Header("Spell Type")]
    public SpellType b_spellType;
    public SpellTarget b_spellTarget;
    // public SpellElement b_spellElement;

    // [Header("Spell Effects")]
    // public SpellEffect b_spellEffect;

    public UnityEvent OnSpriteLoaded = new UnityEvent();

    private void OnEnable()
    {
        b_icon = Resources.Load<Sprite>("DefaultSpellIcon");
        Sketcher.Instance.OnImageSaved.AddListener(LoadSprite);
    }

    public void ApplySpellEffect(GameObject caster, GameObject target)
    {       
        HealthManagerSO healthManager = target.GetComponent<HealthManagerSO>();

        // Apply spell type effects
        switch (b_spellType)
        {
            case SpellType.Melee:
            case SpellType.Projectile:
                if (target != null)
                {
                    
                    healthManager.TakeDamage(b_damage);
                }
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

        // // Apply spell element effects
        // switch (b_spellElement)
        // {
        //     case SpellElement.Fire:
        //         // Implement fire logic
        //         break;
        //     case SpellElement.Ice:
        //         // Implement ice logic
        //         break;
        //     case SpellElement.Earth:
        //         // Implement earth logic
        //         break;
        //     case SpellElement.Lightning:
        //         // Implement lightning logic
        //         break;
        //     case SpellElement.DarkMagic:
        //         // Implement dark magic logic
        //         break;
        //     case SpellElement.Support:
        //         // Implement support logic
        //         break;
        //     default:
        //         break;
        // }

        // // Apply spell effect
        // switch (b_spellEffect)
        // {
        //     case SpellEffect.Buff:
        //         if (healthManager != null)
        //         {
        //             healthManager.Heal(b_damage);
        //         }
        //         break;
        //     case SpellEffect.Debuff:
        //     case SpellEffect.Stunning:
        //         if (target != null)
        //         {
        //             healthManager.TakeDamage(b_damage);
        //         }
        //         break;
        //     case SpellEffect.DOT:
        //         if (target != null)
        //         {
        //             healthManager.TakeDamage(b_damage);
        //         }
        //         break;
        //     case SpellEffect.None:
        //         break;
        // };
    }

    private void AttackAnimation(GameObject caster, GameObject target)
    {
        b_icon = Resources.Load<Sprite>(b_spellName);
        Instantiate(b_icon);
        Vector2.Lerp(caster.transform.position, target.transform.position, 0.5f);
        Destroy(b_icon,0.5f);
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
                PlayerUI playerUI = GameManager.Instance.b_Player.GetComponent<PlayerUI>();
                playerUI.SetSpellToButton(0,this);

            OnSpriteLoaded.Invoke();

        }
        else if (b_icon == null)
        {
            b_icon = Resources.Load<Sprite>("DefaultSpellIcon");
        }
    }
}
