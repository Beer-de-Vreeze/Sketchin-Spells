using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Spellbook : Singleton<Spellbook>
{
    private List<SpellSO> spells = new();

    

    public SpellSO GetSpell(string spellName)
    {
        return spells.Find(spell => spell.b_spellName == spellName);
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

