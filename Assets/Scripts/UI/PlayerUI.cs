using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    public GameObject Target;

    [SerializeField]
    public List<Button> SpellButtons = new List<Button>();

    internal Button EndTurnButton;

    private void Start()
    {
        EndTurnButton = GetComponentInChildren<Button>();
    }

    public void CastSpell(Spell spell, GameObject target)
    {
        if (spell != null && target != null)
        {
            Player player = GameManager.Instance.Player.GetComponent<Player>();
            if (player.Mana.CurrentMana >= spell.SpellData.ManaCost)
            {
                if (spell.SpellData.SpellType == SpellType.Projectile)
                {
                    spell.AnimateProjectileSpell(this.gameObject, target);
                }
                else if (spell.SpellData.SpellType == SpellType.Heal)
                {
                    spell.AnimateHealSpell(this.gameObject, target);
                }
                player.Mana.CurrentMana -= spell.SpellData.ManaCost;
                GameManager
                    .Instance.Player.GetComponent<Player>()
                    .Mana.ManaChangedEvent.Invoke(player.Mana.CurrentMana);
                TurnManager.Instance.EndPlayerTurn(); // Ensure the player's turn ends after casting a spell
            }
            else
            {
                Debug.Log("Not enough mana to cast the spell.");
            }
        }
    }

    public void SetTarget(GameObject target)
    {
        if (target != null)
        {
            Target = target;
        }
    }

    public void DisableSpellButtons()
    {
        foreach (Button button in SpellButtons)
        {
            button.interactable = false;
        }
    }
}
