using System.Collections;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Spell : MonoBehaviour
{
    public SpellSO SpellData;
    public Sprite Sketch;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private void OnEnable()
    {
        Sketcher.Instance.OnImageSaved.AddListener(() => LoadSprite());
        Sketcher.Instance.OnImageSaved.AddListener(() => SetSpriteSize());
    }

    private void SetSpriteSize()
    {
        _spriteRenderer.size = new Vector2(2.54f / 3, 3.99f / 3);
        transform.localScale = new Vector3(108f / 3, 108f / 3, 108f / 3);
    }

    public void ApplySpellEffect(GameObject caster, GameObject target)
    {
        if (SpellData == null)
        {
            Debug.LogError("SpellData is null");
            return;
        }

        if (target == null)
        {
            Debug.LogError("Target is null");
            return;
        }

        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer == null)
            {
                Debug.LogError("SpriteRenderer component is missing.");
                return;
            }
        }

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
        if (caster == null || target == null)
        {
            Debug.LogError("Caster or target is null");
            return;
        }
        StartCoroutine(ProjectileAnimationCoroutine(caster, target));
    }

    private IEnumerator ProjectileAnimationCoroutine(GameObject caster, GameObject target)
    {
        GameObject spellInstance = Instantiate(
            gameObject,
            caster.transform.position,
            Quaternion.identity
        );
        spellInstance.transform.SetParent(UIManager.Instance.GameCanvas.transform, false);
        spellInstance.GetComponent<Spell>().LoadSprite();
        float duration = 0.5f;
        float elapsed = 0f;
        Vector3 start = caster.transform.position;
        Vector3 end = target.transform.position;
        while (elapsed < duration)
        {
            spellInstance.transform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        spellInstance.transform.position = end;
        ApplySpellEffect(spellInstance, target);
        Destroy(spellInstance, 0.5f);
    }

    public void AnimateHealSpell(GameObject caster, GameObject target)
    {
        if (caster == null || target == null)
        {
            Debug.LogError("Caster or target is null");
            return;
        }
        StartCoroutine(HealAnimationCoroutine(caster, target));
    }

    private IEnumerator HealAnimationCoroutine(GameObject caster, GameObject target)
    {
        GameObject spellInstance = Instantiate(
            gameObject,
            target.transform.position,
            Quaternion.identity
        );
        spellInstance.transform.SetParent(UIManager.Instance.GameCanvas.transform, false);
        spellInstance.GetComponent<Spell>().LoadSprite();
        float duration = 0.5f;
        float elapsed = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = target.transform.localScale * 1.5f;
        spellInstance.transform.localScale = startScale;
        while (elapsed < duration)
        {
            spellInstance.transform.localScale = Vector3.Lerp(
                startScale,
                endScale,
                elapsed / duration
            );
            elapsed += Time.deltaTime;
            yield return null;
        }
        spellInstance.transform.localScale = endScale;
        ApplySpellEffect(spellInstance, target);
        Destroy(spellInstance, 0.5f);
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
            SetSpriteSize();
            Debug.Log($"Spell sprite loaded" + SpellData.SpellName + path);
        }
        else
        {
            Debug.LogError("Sprite file not found at path: " + path);
        }
    }
}
