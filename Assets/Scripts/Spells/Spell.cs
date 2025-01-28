using System.IO;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Spell : MonoBehaviour
{
    public SpellSO SpellData;
    public Sprite Sketch;
    private SpriteRenderer _spriteRenderer;

    private void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Sketcher.Instance.OnImageSaved.AddListener(() => LoadSprite());
        Sketcher.Instance.OnImageSaved.AddListener(() => SetSpriteSize());
    }

    private void SetSpriteSize()
    {
        _spriteRenderer.size = new Vector2(2.54f, 3.99f);
        transform.localScale = new Vector3(108f, 108f, 108f);
    }

    public void ApplySpellEffect(GameObject caster, GameObject target)
    {
        Debug.Log(
            $"Applying spell effect to {target.name} from {caster.name} with {SpellData.SpellName} dealing {SpellData.Damage} damage."
        );
        switch (SpellData.SpellType)
        {
            case SpellType.Projectile:
                if (target.CompareTag("Enemy"))
                {
                    Enemy enemy = target.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.Health.TakeDamage(SpellData.Damage);
                        Debug.Log($"Dealt {SpellData.Damage} damage to {target.name}");
                    }
                }
                else if (target.CompareTag("Player"))
                {
                    Player player = target.GetComponent<Player>();
                    if (player != null)
                    {
                        player.Health.TakeDamage(SpellData.Damage);
                        Debug.Log($"Dealt {SpellData.Damage} damage to {target.name}");
                    }
                }
                break;
            case SpellType.Heal:
                if (target.CompareTag("Enemy"))
                {
                    Enemy enemy = target.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.Health.Heal(SpellData.Damage);
                        Debug.Log($"Healed {SpellData.Damage} health for {target.name}");
                    }
                }
                else if (target.CompareTag("Player"))
                {
                    Player player = target.GetComponent<Player>();
                    if (player != null)
                    {
                        player.Health.Heal(SpellData.Damage);
                        Debug.Log($"Healed {SpellData.Damage} health for {target.name}");
                    }
                }
                break;
            default:
                break;
        }
    }

    //make an attack animation for the spell
    public void AnimateProjectileSpell(GameObject caster, GameObject target)
    {
        // Implement projectile spell animation
        //lerp the spell from the player to the target

        GameObject spellInstance = Instantiate(gameObject, transform.position, Quaternion.identity);
        spellInstance.transform.SetParent(UIManager.Instance.GameCanvas.transform, false);
        spellInstance.transform.position = Vector3.Lerp(
            caster.transform.position,
            target.transform.position,
            1f
        );
        //when the spell reaches the target, apply the spell effect
        ApplySpellEffect(spellInstance, target);
        Destroy(spellInstance, 1f);
    }

    public void AnimateHealSpell(GameObject caster, GameObject target)
    {
        // Implement heal spell animation
        //lerp the spell from the player to the target

        GameObject spellInstance = Instantiate(gameObject, transform.position, Quaternion.identity);
        spellInstance.transform.SetParent(UIManager.Instance.GameCanvas.transform, false);
        //lerp the scale of the spell to make it look like it's healing the target from small to big
        spellInstance.transform.localScale = Vector3.Lerp(
            caster.transform.localScale,
            target.transform.localScale,
            1f
        );
        ApplySpellEffect(spellInstance, target);
        Destroy(spellInstance, 1f);
    }

    public void Reset()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Sketch = null;
        _spriteRenderer.sprite = null;
    }

    internal void LoadSprite()
    {
        string path = Path.Combine(
            Application.persistentDataPath,
            "sketches",
            "Spell",
            SpellData.SpellName + ".png"
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
            Debug.Log($"Spell sprite loaded" + SpellData.SpellName + path);
        }
        else
        {
            Debug.LogError("Sprite file not found at path: " + path);
        }
    }
}
