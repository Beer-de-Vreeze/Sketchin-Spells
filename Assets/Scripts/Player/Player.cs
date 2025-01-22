using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField]
    public HealthManagerSO m_health;

    [SerializeField]
    public ManaManagerSO b_mana;
    private SpriteRenderer m_spriteRenderer;

    internal string b_playerName = "Player";
    internal string b_playerDescription = "This is you";

    private void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
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
            m_spriteRenderer.sprite = sprite;
            Debug.Log("Player sprite loaded");
        }
    }
}
