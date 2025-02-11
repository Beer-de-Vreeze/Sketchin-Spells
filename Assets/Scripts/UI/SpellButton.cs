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

    private void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(CastSpell);
        }
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
            Player player = GameManager.Instance.Player?.GetComponent<Player>();
            if (player != null && player.Mana != null)
            {
                if (player.Mana.CurrentMana >= Spell.SpellData.ManaCost)
                {
                    UIManager.Instance.PlayerUI.CastSpell(Spell, UIManager.Instance.PlayerUI.Target);
                    player.Mana.CurrentMana -= Spell.SpellData.ManaCost;
                    player.Mana.ManaChangedEvent.Invoke(player.Mana.CurrentMana);
                    Debug.Log($"Casting spell: {Spell.SpellData.SpellName}, Type: {Spell.SpellData.SpellType}");
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
    }
}
