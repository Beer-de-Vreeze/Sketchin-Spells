using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    public Enemy target;
    [SerializeField]
    public List<Button> spellButtons = new List<Button>();
    public string[] playerSpells = new string[4];


    public void CastSpell(SpellSO spell, GameObject target)
    {
        //check if the player has enough mana to cast the spell
        if (GameManager.Instance.b_Player.b_mana.b_currentMana >= spell.b_manaCost)
        {
            //use the mana
            GameManager.Instance.b_Player.b_mana.UseMana(spell.b_manaCost);
            //cast the spell
            spell.ApplySpellEffect(GameManager.Instance.b_Player.gameObject, target);
        }
        else
        {
            Debug.Log("Not enough mana");
        }
    }

    public void SetTarget(Enemy enemy)
    {
        target = enemy;
    }

    public void SetSpellToButton(int buttonIndex, SpellSO spell)
    {
        spellButtons[buttonIndex].GetComponentInChildren<Text>().text = spell.b_spellName;
        spellButtons[buttonIndex].onClick.AddListener(() =>
        {
            CastSpell(spell, target.gameObject);
        });
        //set the icon of the spel
        spellButtons[buttonIndex].GetComponentInChildren<Image>().sprite = spell.b_icon;
    }
}
