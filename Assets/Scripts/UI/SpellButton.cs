using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SpellButton : MonoBehaviour
{
    public Spell Spell;

    [SerializeField]
    internal bool IsUnlocked;

    public UnityEvent<int> UnlockedEvent;

    public void UnlockSpell()
    {
        IsUnlocked = true;
        CheckSpellUnlock();
    }

    public void Reset()
    {
        IsUnlocked = false;
        CheckSpellUnlock();
    }

    private void OnEnable()
    {
        CheckSpellUnlock();
    }

    private void CheckSpellUnlock()
    {
        if (Spell != null)
        {
            Button button = GetComponent<Button>();
            TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();

            if (button != null && text != null)
            {
                if (IsUnlocked == false)
                {
                    button.interactable = false;
                    text.text = "LOCKED";
                }
                else
                {
                    button.interactable = true;
                    text.text = Spell.SpellData.SpellName;
                }
            }
            else
            {
                Debug.LogError("Button or TextMeshProUGUI component is missing.");
            }
        }
        else
        {
            Debug.LogError("Spell is not assigned.");
        }
    }

    public void CastSpell()
    {
        Debug.Log("Casting spell");
        if (
            UIManager.Instance != null
            && UIManager.Instance.PlayerUI != null
            && UIManager.Instance.PlayerUI.Target != null
        )
        {
            UIManager.Instance.PlayerUI.CastSpell(Spell, UIManager.Instance.PlayerUI.Target);
            Debug.Log(
                $"Casting spell: {Spell.SpellData.SpellName}, Type: {Spell.SpellData.SpellType}"
            );
        }
    }
}
