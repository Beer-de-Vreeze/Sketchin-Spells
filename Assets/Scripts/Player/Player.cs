using System.Collections.Generic;
using System.IO;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField]
    public HealthManagerSO m_health;

    [SerializeField]
    public ManaManagerSO b_mana;
    private Sprite m_playerSprite;
    internal bool m_isTurn = false;

    private void Start()
    {
        LoadSprite();
        TurnManager.Instance.OnPlayerTurnStart.AddListener(() =>
        {
            m_isTurn = true;
        });
        TurnManager.Instance.OnPlayerTurnEnd.AddListener(() =>
        {
            m_isTurn = false;
        });
    }


    private void LoadSprite()
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
            m_playerSprite = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f)
            );
        }
        else
        {
            m_playerSprite = Resources.Load<Sprite>("DefaultPlayerIcon");
        }
    }
}
