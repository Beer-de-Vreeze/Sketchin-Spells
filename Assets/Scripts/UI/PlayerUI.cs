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
        EndTurnButton.onClick.AddListener(() =>
        {
            TurnManager.Instance.EndPlayerTurn();
        });
    }

    public void CastSpell(Spell spell, GameObject target)
    {
        if (spell != null)
        {
            if (target != null)
            {
                if (target.CompareTag("Enemy"))
                {
                    spell.AnimateProjectileSpell(this.gameObject, target);
                }
                else if (target.CompareTag("Player"))
                {
                    spell.AnimateHealSpell(this.gameObject, target);
                }
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
