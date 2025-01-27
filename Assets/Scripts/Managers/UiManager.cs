using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    #region Variables
    [SerializeField]
    private HealthManagerSO _healthManager;

    [SerializeField]
    private ManaManagerSO _manaManager;

    [SerializeField]
    public GameObject _sketchCanvas;

    [SerializeField]
    private GameObject _playerCanvas;

    [SerializeField]
    private GameObject _dialogueCanvas;

    [SerializeField]
    internal GameObject GameCanvas;

    [SerializeField]
    private GameObject _menuCanvas;

    [SerializeField]
    public GameObject ENDGAMECanvas;

    [SerializeField]
    private Slider _healthSlider;

    [SerializeField]
    private Slider _manaSlider;

    [SerializeField]
    private TextMeshProUGUI _healthText;

    [SerializeField]
    private TextMeshProUGUI _manaText;
    internal PlayerUI PlayerUI;
    #endregion

    #region Unity
    private void OnEnable()
    {
        _healthManager.healthChangedEvent.AddListener(UpdateHealthBar);
        _manaManager.ManaChangedEvent.AddListener(UpdateManaBar);
        _healthManager.healthChangedEvent.AddListener(UpdateHealthText);
        _manaManager.ManaChangedEvent.AddListener(UpdateManaText);
        _healthManager.healthChangedEvent.Invoke(_healthManager.CurrentHealth);
        _manaManager.ManaChangedEvent.Invoke(_manaManager.CurrentMana);
        GameManager.Instance.Player.GetComponent<Player>().OnPlayerDeath.AddListener(ENDGAME);
        PlayerUI = FindFirstObjectByType<PlayerUI>();
    }

    void Start()
    {
        //get the player health and mana from the healthSO and manaSO
        Player player = GameManager.Instance.Player.GetComponent<Player>();
        player.Health = _healthManager;
        player.Mana = _manaManager;
        CloseAllCanvas();
        OpenMenuCanvas();
    }
    #endregion

    #region UI Toggle
    public void OpenSketchCanvas(SketchType sketchType, string name, string description)
    {
        _sketchCanvas.SetActive(true);
        Sketcher.Instance.SetSketcher(sketchType, name, description);
    }

    public void CloseSketchCanvas()
    {
        _sketchCanvas.SetActive(false);
    }

    public void OpenDialogueCanvas()
    {
        _dialogueCanvas.SetActive(true);
    }

    public void CloseDialogueCanvas()
    {
        _dialogueCanvas.SetActive(false);
    }

    public void OpenMenuCanvas()
    {
        _menuCanvas.SetActive(true);
    }

    public void CloseMenuCanvas()
    {
        _menuCanvas.SetActive(false);
    }

    public void OpenPlayerCanvas()
    {
        _playerCanvas.SetActive(true);
    }

    public void ClosePlayerCanvas()
    {
        _playerCanvas.SetActive(false);
    }

    public void OpenGameCanvas()
    {
        GameCanvas.SetActive(true);
    }

    public void CloseGameCanvas()
    {
        GameCanvas.SetActive(false);
    }

    public void OpenEndGameCanvas()
    {
        ENDGAMECanvas.SetActive(true);
    }

    public void CloseEndGameCanvas()
    {
        ENDGAMECanvas.SetActive(false);
    }

    public void CloseAllCanvas()
    {
        CloseSketchCanvas();
        CloseDialogueCanvas();
        CloseMenuCanvas();
        ClosePlayerCanvas();
        CloseGameCanvas();
        CloseEndGameCanvas();
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
        _healthSlider.value = ConvertIntToFloat(amount);
    }

    public void UpdateManaBar(int amount)
    {
        _manaSlider.value = ConvertIntToFloat(amount);
    }

    public void UpdateHealthText(int amount)
    {
        //68/100
        _healthText.text = amount.ToString() + "/" + _healthManager.MaxHealth.ToString();
    }

    public void UpdateManaText(int amount)
    {
        //8/10
        _manaText.text = amount.ToString() + "/" + _manaManager.MaxMana.ToString();
    }

    private void ENDGAME()
    {
        CloseAllCanvas();
        OpenEndGameCanvas();
    }
    #endregion

    #region Utility
    public void DisplayMessage(string message)
    {
        TextMeshProUGUI messageText = Instantiate(_healthText, _healthText.transform.parent);
        messageText.text = message;
        Destroy(messageText.gameObject, 2f);
    }

    private float ConvertIntToFloat(int amount)
    {
        return (float)amount;
        ;
    }
    #endregion

    public void ResetUIManager()
    {
        CloseAllCanvas();
        OpenMenuCanvas();
    }
}
