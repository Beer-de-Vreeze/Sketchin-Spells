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
            button.onClick.AddListener(() => CastSpell(button.GetComponent<SpellButton>().Spell, Target));
        }
    }

    public void CastSpell(Spell spell, GameObject target)
    {
        if (spell != null && target != null)
        {
            Player player = GameManager.Instance.Player?.GetComponent<Player>();
            if (player != null && player.Mana != null)
            {
                if (player.Mana.CurrentMana >= spell.SpellData.ManaCost)
                {
                    if (spell.SpellData.SpellType == SpellType.Projectile)
                    {
                        spell.ApplySpellEffect(this.gameObject, target);
                    }
                    else if (spell.SpellData.SpellType == SpellType.Heal)
                    {
                        spell.ApplySpellEffect(player.gameObject, player.gameObject);
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
