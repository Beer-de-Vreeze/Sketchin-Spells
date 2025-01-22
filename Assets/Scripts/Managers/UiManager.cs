using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    #region Variables
    [SerializeField]
    private HealthManagerSO m_healthManager;

    [SerializeField]
    private ManaManagerSO m_manaManager;

    [SerializeField]
    private GameObject m_sketchCanvas;

    [SerializeField]
    private GameObject m_playerCanvas;

    [SerializeField]
    private GameObject m_dialogueCanvas;

    [SerializeField]
    private GameObject m_gameCanvas;

    [SerializeField]
    private GameObject m_menuCanvas;

    [SerializeField]
    private Slider m_healthSlider;

    [SerializeField]
    private Slider m_manaSlider;

    [SerializeField]
    private TextMeshProUGUI m_healthText;

    [SerializeField]
    private TextMeshProUGUI m_manaText;
    internal PlayerUI b_playerUI;
    #endregion

    #region Unity
    private void OnEnable()
    {
        m_healthManager.healthChangedEvent.AddListener(UpdateHealthBar);
        m_manaManager.b_manaChangedEvent.AddListener(UpdateManaBar);
        m_healthManager.healthChangedEvent.AddListener(UpdateHealthText);
        m_manaManager.b_manaChangedEvent.AddListener(UpdateManaText);
        m_healthManager.healthChangedEvent.Invoke(m_healthManager.b_currentHealth);
        m_manaManager.b_manaChangedEvent.Invoke(m_manaManager.b_currentMana);
        b_playerUI = FindFirstObjectByType<PlayerUI>();
    }

    void Start()
    {
        //get the player health and mana from the healthSO and manaSO
        Player player = GameManager.Instance.b_Player.GetComponent<Player>();
        player.m_health = m_healthManager;
        player.b_mana = m_manaManager;
        CloseAllCanvas();
        OpenMenuCanvas();
    }
    #endregion

    #region UI Toggle
    public void OpenSketchCanvas(SketchType sketchType, string name, string description)
    {
        m_sketchCanvas.SetActive(true);
        Sketcher.Instance.SetSketcher(sketchType, name, description);
    }

    public void CloseSketchCanvas()
    {
        m_sketchCanvas.SetActive(false);
    }

    public void OpenDialogueCanvas()
    {
        m_dialogueCanvas.SetActive(true);
    }

    public void CloseDialogueCanvas()
    {
        m_dialogueCanvas.SetActive(false);
    }

    public void OpenMenuCanvas()
    {
        m_menuCanvas.SetActive(true);
    }

    public void CloseMenuCanvas()
    {
        m_menuCanvas.SetActive(false);
    }

    public void OpenPlayerCanvas()
    {
        m_playerCanvas.SetActive(true);
    }

    public void ClosePlayerCanvas()
    {
        m_playerCanvas.SetActive(false);
    }

    public void OpenGameCanvas()
    {
        m_gameCanvas.SetActive(true);
    }

    public void CloseGameCanvas()
    {
        m_gameCanvas.SetActive(false);
    }

    public void CloseAllCanvas()
    {
        CloseSketchCanvas();
        CloseDialogueCanvas();
        CloseMenuCanvas();
        ClosePlayerCanvas();
        CloseGameCanvas();
    }

    #endregion
    public void GetALlSpritesRenderersOfforoOn()
    {
        SpriteRenderer[] allSpriteRenderers = FindObjectsByType<SpriteRenderer>(
            FindObjectsSortMode.None
        );
        foreach (SpriteRenderer spriteRenderer in allSpriteRenderers)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
        }
    }

    #region Update
    public void UpdateHealthBar(int amount)
    {
        m_healthSlider.value = ConvertIntToFloat(amount);
    }

    public void UpdateManaBar(int amount)
    {
        m_manaSlider.value = ConvertIntToFloat(amount);
    }

    public void UpdateHealthText(int amount)
    {
        //68/100
        m_healthText.text = amount.ToString() + "/" + m_healthManager.b_maxHealth.ToString();
    }

    public void UpdateManaText(int amount)
    {
        //8/10
        m_manaText.text = amount.ToString() + "/" + m_manaManager.b_maxMana.ToString();
    }
    #endregion

    #region Utility
    public void DisplayMessage(string message)
    {
        TextMeshProUGUI messageText = Instantiate(m_healthText, m_healthText.transform.parent);
        messageText.text = message;
        Destroy(messageText.gameObject, 2f);
    }

    private float ConvertIntToFloat(int amount)
    {
        return (float)amount;
        ;
    }
    #endregion
}
