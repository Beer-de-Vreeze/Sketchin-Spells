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
    Heal,
    Shield,
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
    public string SpellName;
    public string Description;
    public Sprite Icon; //Resources.Load<Sprite>("DefaultSpellIcon");

    [Header("Spell Stats")]
    public int Damage;
    public int ManaCost;

    [Header("Spell Type")]
    public SpellType SpellType;
    public SpellTarget SpellTarget;

    // public SpellElement SpellElement;

    // [Header("Spell Effects")]
    // public SpellEffect SpellEffect;

    [NonSerialized]
    public UnityEvent OnSpriteLoaded = new UnityEvent();

    public void Reset()
    {
        // Reset any non-serialized fields or states here
        OnSpriteLoaded.RemoveAllListeners();
    }
}
