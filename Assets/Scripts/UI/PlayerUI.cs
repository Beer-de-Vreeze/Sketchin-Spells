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
        foreach (Button button in SpellButtons)
        {
            // Use the SpellButton's CastSpell method instead of directly calling CastSpell
            SpellButton spellButton = button.GetComponent<SpellButton>();
            if (spellButton != null)
            {
                button.onClick.AddListener(() => spellButton.CastSpell());
            }
        }
    }

    public void CastSpell(Spell spell, GameObject target)
    {
        Debug.Log(
            $"CastSpell called - Spell: {(spell ? spell.SpellData.SpellName : "null")}, Target: {(target ? target.name : "null")}"
        );

        if (spell != null && target != null)
        {
            Player player = GameManager.Instance.Player?.GetComponent<Player>();
            if (player != null && player.Mana != null)
            {
                Debug.Log(
                    $"Player mana: {player.Mana.CurrentMana}/{player.Mana.MaxMana}, Spell cost: {spell.SpellData.ManaCost}"
                );

                if (player.Mana.CurrentMana >= spell.SpellData.ManaCost)
                {
                    Debug.Log(
                        $"Casting {spell.SpellData.SpellName} ({spell.SpellData.SpellType}) on {target.name} for {spell.SpellData.Damage} damage"
                    );

                    if (spell.SpellData.SpellType == SpellType.Projectile)
                    {
                        spell.AnimateProjectileSpell(this.gameObject, target); // Use animation
                    }
                    else if (spell.SpellData.SpellType == SpellType.Heal)
                    {
                        spell.AnimateHealSpell(player.gameObject, player.gameObject); // Use animation
                    }
                    else
                    {
                        spell.ApplySpellEffect(this.gameObject, target);
                    }
                    player.Mana.CurrentMana -= spell.SpellData.ManaCost;
                    player.Mana.ManaChangedEvent.Invoke(player.Mana.CurrentMana);
                    TurnManager.Instance.OnPlayerTurnEnd.Invoke();
                }
                else
                {
                    Debug.Log("Not enough mana to cast the spell.");
                }
            }
            else
            {
                Debug.Log("Player or mana is null.");
            }
        }
        else
        {
            Debug.LogError("Spell or target is null");
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
