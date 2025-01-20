using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public HealthManagerSO m_health;

    [SerializeField]
    public ManaManagerSO b_mana;

    [SerializeField]
    private Enemy target;

    private Sprite m_playerSprite;

    bool m_isTurn = false;

    private void Start()
    {
        target = FindFirstObjectByType<Enemy>();
        LoadSprite();
        Debug.Log(target);
    }

    private void LoadSprite()
    {
        string path = Path.Combine(
            Application.persistentDataPath,
            "sketches",
            "Player",
            "Player" + ".png"
        );
        if (File.Exists(path) && path != Resources.Load<Sprite>("DefaultPlayerIcon").name)
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
            m_playerSprite = null;

            // open the sketcher window so the player can draw a custom spell for the game
            Sketcher.Instance.OpenSketcher(SketchType.Spell, "Player");
            Sketcher.Instance.OnImageSaved += (path) =>
            {
                LoadSprite();
            };
        }
    }
}
