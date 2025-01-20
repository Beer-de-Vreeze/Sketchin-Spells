using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    private void Start() 
    {
        
    }

    public void CastSpell(string spellName,GameObject target)
    {
        Spellbook.Instance.CastSpell(spellName,GameManager.Instance.b_Player,target);
    }
}
