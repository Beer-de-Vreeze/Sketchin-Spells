using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public SpellSO SpellData;
    public Sprite Sketch;
    private SpriteRenderer _spriteRenderer;

    private void OnEnable()
    {
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
        HealthManagerSO healthManager = null;
        if(target.CompareTag("Player"))
        {
            healthManager = target.GetComponent<Player>().Health;
        }
        else if(target.CompareTag("Enemy"))
        {
            healthManager = target.GetComponent<Enemy>().HealthManager;
        }

        // Apply spell type effects
        switch (SpellData.SpellType)
        {
            case SpellType.Projectile:
                if (healthManager != null)
                {
                    healthManager.TakeDamage(SpellData.Damage);
                }
                break;
            case SpellType.Heal:
                if (healthManager != null) { }
                break;
            default:
                break;
        }

        // Apply spell element effects
        switch (SpellData.SpellElement)
        {
            case SpellElement.Fire:
                // Implement fire logic
                break;
            case SpellElement.Ice:
                // Implement ice logic
                break;
            case SpellElement.Earth:
                // Implement earth logic
                break;
            case SpellElement.Lightning:
                // Implement lightning logic
                break;
            case SpellElement.DarkMagic:
                // Implement dark magic logic
                break;
            case SpellElement.Support:
                // Implement support logic
                break;
            default:
                break;
        }

        // Apply spell effect
        switch (SpellData.SpellEffect)
        {
            case SpellEffect.Buff:
                if (healthManager != null)
                {
                    healthManager.Heal(SpellData.Damage);
                }
                break;
            case SpellEffect.Debuff:
            case SpellEffect.Stunning:
                if (target != null)
                {
                    healthManager.TakeDamage(SpellData.Damage);
                }
                break;
            case SpellEffect.DOT:
                if (target != null)
                {
                    healthManager.TakeDamage(SpellData.Damage);
                }
                break;
            case SpellEffect.None:
                break;
        }
    }

    //make an attack animation for the spell
    public void AnimateProjectileSpell(GameObject caster, GameObject target)
    {
        // Implement projectile spell animation
        //lerp the spell from the player to the target

        Instantiate(gameObject, transform.position, Quaternion.identity);
        gameObject.transform.position = Vector3.Lerp(
            caster.transform.position,
            target.transform.position,
            1f
        );
        //when the spell reaches the target, apply the spell effect
        ApplySpellEffect(gameObject, gameObject);
        Destroy(gameObject, 1f);
    }

    public void AnimateHealSpell(GameObject caster, GameObject target)
    {
        // Implement heal spell animation
        //lerp the spell from the player to the target

        Instantiate(gameObject, transform.position, Quaternion.identity);
        //lerp the scale of the spell to make it look like it's healing the target from small to big
        gameObject.transform.localScale = Vector3.Lerp(
            caster.transform.localScale,
            target.transform.localScale,
            1f
        );
        ApplySpellEffect(gameObject, gameObject);
        Destroy(gameObject, 1f);
    }

    public void Reset()
    {
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
