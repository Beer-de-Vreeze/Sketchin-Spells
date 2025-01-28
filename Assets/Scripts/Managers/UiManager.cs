using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    #region Variables
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
        GameManager
            .Instance.Player.GetComponent<Player>()
            .Health.PlayerhealthChangedEvent.AddListener(UpdateHealthBar);
        GameManager
            .Instance.Player.GetComponent<Player>()
            .Health.PlayerhealthChangedEvent.AddListener(UpdateHealthText);
        GameManager
            .Instance.Player.GetComponent<Player>()
            .Mana.ManaChangedEvent.AddListener(UpdateManaBar);
        GameManager
            .Instance.Player.GetComponent<Player>()
            .Mana.ManaChangedEvent.AddListener(UpdateManaText);
        GameManager.Instance.Player.GetComponent<Player>().OnPlayerDeath.AddListener(ENDGAME);
        PlayerUI = FindFirstObjectByType<PlayerUI>();
    }

    void Start()
    {
        //get the player health and mana from the healthSO and manaSO
        Player player = GameManager.Instance.Player.GetComponent<Player>();
        UpdateHealthBar(player.Health.CurrentHealth);
        UpdateHealthText(player.Health.CurrentHealth);
        UpdateManaBar(player.Mana.CurrentMana);
        UpdateManaText(player.Mana.CurrentMana);
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
        _healthText.text = amount.ToString() + "/" + GameManager.Instance.Player.GetComponent<Player>().Health.MaxHealth.ToString();
    }

    public void UpdateManaText(int amount)
    {
        //8/10
        _manaText.text = amount.ToString() + "/" + GameManager.Instance.Player.GetComponent<Player>().Mana.MaxMana.ToString();
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
