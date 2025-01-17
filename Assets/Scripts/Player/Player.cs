using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private HealthManagerSO m_health;
    [SerializeField]
    private ManaManagerSO m_mana;
    [SerializeField]
    private Enemy target;
    private SpriteRenderer m_spriteRenderer;
    private void Start()
    {
        target = FindFirstObjectByType<Enemy>();
        Debug.Log(target);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // use createspell and then castspell
            Debug.Log("Casting Gale");
            Debug.Log("Target Health: " + target.m_healthManager.GetCurrentHealth());
        }
    }
}
