using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class Spellbook : Singleton<Spellbook>
{
    private List<SpellSO> spells = new();

    

    public SpellSO GetSpell(string spellName)
    {
        //get the spells out of resources folder and in that flder spellSO only load the spell with the name spellName
        SpellSO spell = Resources.LoadAll<SpellSO>("SpellSO").ToList().Find(s => s.b_spellName == spellName);
        if (spell != null)
        {
            return spell;
        }
        else
        {
            //make a spellSO with random values and cast it to spellSO
            Debug.LogError("Spell not found");
            return null;
        }
    }

    //make a fucntion that makes a spellSO with random values and cast it to spellSO
    public SpellSO CreateSpell(string spellName)
    {
        SpellSO spell = ScriptableObject.CreateInstance<SpellSO>();
        spell.b_spellName = "Gale";
        spell.b_description = "This is a spell";
        spell.b_icon = Resources.Load<Sprite>("DefaultSpellIcon");
        spell.b_damage = 10;
        spell.b_manaCost = 10;
        spell.b_amount = 1;
        spell.b_cooldown = 10;
        spell.b_spellType = SpellType.Projectile;
        spell.b_spellEffect = SpellEffect.None;
        return spell;
    }
    public void AddSpell(SpellSO spell)
    {
        spells.Add(spell);
    }

    public void RemoveSpell(SpellSO spell)
    {
        spells.Remove(spell);
    }
}

