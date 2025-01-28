using System.IO;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour
{
    [SerializeField]
    internal EnemySO EnemyData;

    public EnemyHealthManagerSO Health;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    public Sprite Sketch;

    public UnityEvent OnDeath = new UnityEvent();

    protected virtual void OnEnable()
    {
        if (EnemyData == null)
        {
            Debug.LogError("EnemyData is not assigned.");
            return;
        }
        _spriteRenderer = GetComponent<SpriteRenderer>();
        TurnManager.Instance.OnEnemyTurnStart.AddListener(OnTurnStart);
        Sketcher.Instance.OnImageSaved.AddListener(() => LoadSprite());
        Sketcher.Instance.OnImageSaved.AddListener(() => SetSpriteSize());
        //make an enemy health manager
        Health = ScriptableObject.CreateInstance<EnemyHealthManagerSO>();
        Health.Reset();
        Health.SetMaxHealth(EnemyData.MaxHealth);
    }

    protected virtual void SetSpriteSize()
    {
        _spriteRenderer.size = new Vector2(2.54f, 3.99f);
        transform.localScale = new Vector3(108f, 108f, 108f);
    }

    protected virtual void OnTurnStart()
    {
        EnemyData.Cast(this.gameObject, GameManager.Instance.Player.gameObject);
        TurnManager.Instance.EndEnemyTurn();
    }

    protected virtual void Start()
    {
        if (EnemyData == null)
        {
            Debug.LogError("EnemyData is not assigned.");
            return;
        }
        UIManager.Instance.PlayerUI.SetTarget(this.gameObject);
    }

    protected virtual void Update()
    {
        if (Health.CurrentHealth <= 0)
        {
            Death();
        }
    }

    public void Reset()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = null;
    }

    internal virtual void LoadSprite()
    {
        string path = Path.Combine(
            Application.persistentDataPath,
            "sketches",
            "Enemy",
            EnemyData.EnemyName + ".png"
        );
        if (File.Exists(path))
        {
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
            Sprite sprite = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f)
            );
            Sketch = sprite;
            _spriteRenderer.sprite = Sketch;
            Debug.Log($"Enemy sprite loaded" + EnemyData.EnemyName + path);
        }
        else
        {
            Debug.LogError("Sprite file not found at path: " + path);
        }
    }

    protected virtual void Death()
    {
        if (Health.CurrentHealth <= 0)
        {
            OnDeath.Invoke();
            TurnManager.Instance.HandleEnemyDeath();
            Destroy(gameObject);
        }
        OnDeath.RemoveAllListeners();
        TurnManager.Instance.OnEnemyTurnStart.RemoveListener(OnTurnStart);
    }
}
