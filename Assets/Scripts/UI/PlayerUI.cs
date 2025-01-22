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
    [SerializeField]
    public List<SpellSO> spells = new List<SpellSO>();

    internal Button b_endTurnButton;

    private void Start()
    {
        b_endTurnButton = GetComponentInChildren<Button>();
        b_endTurnButton.onClick.AddListener(() =>
        {
            TurnManager.Instance.EndPlayerTurn();
        });
    }


    public void CastSpell(SpellSO spell, GameObject target)
    {
        if (spell != null)
        {
            //check who is casting the spell
            if (target.CompareTag("Player"))
            {
                Player player = target.GetComponent<Player>();
                if (player.b_mana.b_currentMana >= spell.b_manaCost)
                {
                    player.b_mana.b_currentMana -= spell.b_manaCost;
                    spell.ApplySpellEffect(target, this.target.gameObject);
                }
            }
            else if (target.CompareTag("Enemy"))
            {
                Enemy enemy = target.GetComponent<Enemy>();
                spell.ApplySpellEffect(target, this.target.gameObject);
            }
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
        //set the icon of the spell
        spellButtons[buttonIndex].GetComponentInChildren<Image>().sprite = spell.b_icon;
    }
}
