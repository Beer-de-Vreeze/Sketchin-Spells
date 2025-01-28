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
        UpdateButtonState();
        checkSpellUnlock();
    }

    public void Reset()
    {
        IsUnlocked = false;
        UpdateButtonState();
    }

    private void OnEnable()
    {
        checkSpellUnlock();
    }

    private void checkSpellUnlock()
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

    private void UpdateButtonState()
    {
        Button button = GetComponent<Button>();
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();

        if (button != null && text != null)
        {
            button.interactable = IsUnlocked;
            text.text = IsUnlocked ? Spell.SpellData.SpellName : "LOCKED";
        }
    }

    public void CastSpell()
    {
        Debug.Log("Casting spell");
        if (UIManager.Instance.PlayerUI.Target != null)
        {
            UIManager.Instance.PlayerUI.CastSpell(Spell, UIManager.Instance.PlayerUI.Target);
            Debug.Log(
                $"Casting spell: {Spell.SpellData.SpellName}, Type: {Spell.SpellData.SpellType}"
            );
        }
    }
}
