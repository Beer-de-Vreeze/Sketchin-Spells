using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField]
    public HealthManagerSO m_health;

    [SerializeField]
    public ManaManagerSO b_mana;

    [SerializeField]
    private Enemy target;

    private Sprite m_playerSprite;

    private PlayerUI m_playerUI;

    private string m_TestSpell = "Fireball";

    internal bool m_isTurn = false;

    public UnityEvent OnTurnStart = new UnityEvent();
    public UnityEvent OnTurnEnd = new UnityEvent();

    private void Start()
    {
        LoadSprite();
        Debug.Log(target);
    }

    private void Update() {
        if (m_isTurn)
        {
            if (IsMouseOverEnemy())
            {
                Spellbook.Instance.CastSpell(m_TestSpell, gameObject, target.gameObject);
                m_isTurn = false;
                OnTurnEnd.Invoke();
            }
        }
    }

    public bool IsMouseOverEnemy()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    target = hit.collider.GetComponent<Enemy>();
                    Debug.Log(target);
                    return true;
                }
            }
        }
        return false;
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
            UiManager.Instance.OpenCloseSketchCanvas(SketchType.Player, "Player");
            Sketcher.Instance.OnImageSaved += (path) =>
            {
                LoadSprite();
            };
        }
    }
}
