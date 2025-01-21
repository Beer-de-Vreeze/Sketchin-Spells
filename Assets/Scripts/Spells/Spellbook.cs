using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Spellbook : Singleton<Spellbook>
{
    private List<SpellSO> spells = new();

    private List<SpellSO> spellsInRuntime = new();

    public UnityEvent<SpellSO, GameObject, GameObject> OnSpellCast = new UnityEvent<SpellSO, GameObject, GameObject>();

    public void LoadAllSpells()
    {
        string[] spellPaths = Directory.GetFiles(
            Application.dataPath + "/Resources/Spells",
            "*.asset"
        );
        foreach (string spellPath in spellPaths)
        {
            string spellName = Path.GetFileNameWithoutExtension(spellPath);
            SpellSO spell = Resources.Load<SpellSO>("Spells/" + spellName);
            spells.Add(spell);
        }
    }

    //make a fucntion that makes a spellSO with random values and cast it to spellSO
    public SpellSO CreateSpell(string spellName)
    {
        SpellSO spell = ScriptableObject.CreateInstance<SpellSO>();
        spell.b_spellName = spellName;
        spell.b_description = "This is a spell";
        spell.b_icon = Resources.Load<Sprite>("DefaultSpellIcon");
        spell.b_damage = 10;
        spell.b_manaCost = 10;
        spell.b_amount = 1;
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

    public void CastSpell(string spellName, GameObject caster, GameObject target)
    {
        SpellSO spell = spells.FirstOrDefault(s => s.b_spellName == spellName);
        if (spell != null)
        {
            OnSpellCast.Invoke(spell, caster, target);
            //check who is casting the spell
            if (caster.CompareTag("Player"))
            {
                Player player = caster.GetComponent<Player>();
                if (player.b_mana.b_currentMana >= spell.b_manaCost)
                {
                    player.b_mana.b_currentMana -= spell.b_manaCost;
                    spell.ApplySpellEffect(caster, target);
                }
            }
            else if (caster.CompareTag("Enemy"))
            {
                Enemy enemy = caster.GetComponent<Enemy>();
                spell.ApplySpellEffect(caster, target);
            }
        }
    }
}
