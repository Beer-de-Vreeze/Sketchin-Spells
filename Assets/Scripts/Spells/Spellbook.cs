using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Spellbook : Singleton<Spellbook>
{
    private List<SpellSO> _spells = new();

    private List<SpellSO> _spellsInRuntime = new();
}
