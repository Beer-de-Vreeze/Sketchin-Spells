using System.IO;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BaseEnemy", menuName = "Create Enemy")]
public class EnemySO : ScriptableObject
{
    public string b_enemyName;
    public string b_description;
    public Sprite b_sketch;
    public int b_maxHealthSO;
    private HealthManagerSO m_health;
    public SpellSO[] b_attacks;

    public UnityEvent OnSpriteLoaded = new UnityEvent();
    public UnityEvent OnAttack = new UnityEvent();


    private void Start()
    {
        m_health.b_maxHealth = b_maxHealthSO;
    }

    public void Attack(GameObject caster, GameObject target)
    {
        b_attacks[Random.Range(0, b_attacks.Length)].ApplySpellEffect(caster, target);
        OnAttack.Invoke();
    }

    public void LoadSprite()
    {
        string path = Path.Combine(
            Application.persistentDataPath,
            "sketches",
            "Enemy",
            b_enemyName + ".png"
        );
        if (File.Exists(path))
        {
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
            b_sketch = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f)
            );
            OnSpriteLoaded.Invoke();
        }
        else
        {
            b_sketch = Resources.Load<Sprite>("DefaultEnemyIcon");
        }
    }
}
