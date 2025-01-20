using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "BaseEnemy", menuName = "Enemies/Create Enemy")]
public class EnemySO : ScriptableObject
{
    public string b_enemyName;
    public string b_description;
    public Sprite b_sketch;
    public int b_maxHealthSO;
    private HealthManagerSO m_health;
    public SpellSO b_attack;

    private void Start() 
    {
        m_health.b_maxHealth = b_maxHealthSO;
    }

    public void Attack(GameObject caster, GameObject target)
    {
        b_attack.ApplySpellEffect(caster, target);
    }

 private void LoadSprite()
    {
        string path = Path.Combine(
            Application.persistentDataPath,
            "sketches",
            "Enemy",
            b_enemyName + ".png"
        );
        if (File.Exists(path) && path != Resources.Load<Sprite>("DefaultPlayerIcon").name)
        {
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
                b_sketch = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f)
            );
        }
        else
        {
            b_sketch = null;

            // open the sketcher window so the player can draw a custom spell for the game
            Sketcher.Instance.OpenSketcher(SketchType.Enemy, "Player");
            Sketcher.Instance.OnImageSaved += (path) =>
            {
                LoadSprite();
            };
        }
    }
}
