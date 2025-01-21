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
    private GameObject DialogueCanvas;
    [SerializeField]
    private GameObject MenuCanvas;

    [SerializeField]
    private Slider m_healthSlider;

    [SerializeField]
    private Slider m_manaSlider;

    [SerializeField]
    private TextMeshProUGUI m_healthText;

    [SerializeField]
    private TextMeshProUGUI m_manaText;

    [SerializeField]
    private TextMeshProUGUI M_goldText;
    [System.NonSerialized]
    public UnityEvent OnSketchCanvasOpenOrClose = new UnityEvent();
    #endregion

    #region Unity
    private void OnEnable()
    {
        m_healthManager.healthChangedEvent.AddListener(UpdateHealthBar);
        m_manaManager.b_manaChangedEvent.AddListener(UpdateManaBar);
        Inventory.Instance.m_GoldChangedEvent.AddListener(UpdateGoldText);
        m_healthManager.healthChangedEvent.AddListener(UpdateHealthText);
        m_manaManager.b_manaChangedEvent.AddListener(UpdateManaText);
        OnSketchCanvasOpenOrClose.AddListener(OpenClosePlayerCanvas);
    }

    void Start()
    {
        //get the player health and mana from the healthSO and manaSO
        Player player = GameManager.Instance.b_Player.GetComponent<Player>();
        player.m_health = m_healthManager;
        player.b_mana = m_manaManager;
        m_sketchCanvas.SetActive(false);
    }
    #endregion

    #region Sketch Canvas
    public void OpenSketchCanvas(SketchType sketchType, string name, string description)
    {
        Sketcher.Instance.SetSketcher(sketchType, name, description);
        m_sketchCanvas.SetActive(true);
        OnSketchCanvasOpenOrClose.Invoke();
    }

    public void CloseSketcher()
    {
        m_sketchCanvas.SetActive(false);
        OnSketchCanvasOpenOrClose.Invoke();
    }
    #endregion

    #region UI Toggle
    public void OpenClosePlayerCanvas()
    {
        m_playerCanvas.SetActive(!m_playerCanvas.activeSelf);
    }

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
    #endregion

    #region Update
    public void UpdateHealthBar(int amount)
    {
        m_healthSlider.value = ConvertIntToFloatDecimal(amount);
    }

    public void UpdateManaBar(int amount)
    {
        m_manaSlider.value = ConvertIntToFloatDecimal(amount);
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

    public void UpdateGoldText(int amount)
    {
        //254G
        M_goldText.text = amount.ToString() + "G";
    }
    #endregion

    #region Utility
    public void DisplayMessage(string message)
    {
        TextMeshProUGUI messageText = Instantiate(m_healthText, m_healthText.transform.parent);
        messageText.text = message;	
        Destroy(messageText.gameObject, 2f);
    }

    private float ConvertIntToFloatDecimal(int amount)
    {
        return (float)amount / 100;
    }
    #endregion
}
