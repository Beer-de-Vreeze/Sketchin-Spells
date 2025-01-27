using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField]
    public HealthManagerSO Health;

    [SerializeField]
    public ManaManagerSO Mana;
    private SpriteRenderer _spriteRenderer;
    public UnityEvent OnPlayerDeath = new UnityEvent();

    internal string PlayerName = "Player";
    internal string PlayerDescription = "This is you";

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Health.healthChangedEvent.AddListener(Death);
    }

    internal void LoadSprite()
    {
        string path = Path.Combine(
            Application.persistentDataPath,
            "sketches",
            "Player",
            "Player" + ".png"
        );
        if (File.Exists(path))
        {
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
            Sprite sprite = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0, 0)
            );
            _spriteRenderer.sprite = sprite;
            Debug.Log("Player sprite loaded");
        }
    }

    private void Death(int health)
    {
        if (health <= 0)
        {
            OnPlayerDeath.Invoke();
        }
    }

    public void Reset()
    {
        Health.Reset();
        Mana.Reset();
        _spriteRenderer.sprite = null;
    }
}
