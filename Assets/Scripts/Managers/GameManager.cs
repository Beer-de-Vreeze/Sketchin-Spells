using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public Player Player;

    private int _run = 1;

    private void Start()
    {
        PlayerPrefs.SetInt("run", _run);
    }

    public IEnumerator StartGameSequence()
    {
        Debug.Log("Starting game sequence");

        // Start the first dialogue and sketching sequence
        yield return StartDialogueAndSketch(
            1,
            SketchType.Player,
            Player.name,
            Player.PlayerDescription
        );
        Player.LoadSprite();
        UIManager.Instance.OpenGameCanvas();

        // Start the second dialogue and sketching sequence
        WaveManager.Instance.SpawnEnemy(0);
        Enemy enemy = TurnManager.Instance.CurrentEnemy;
        yield return new WaitUntil(() => enemy != null);
        yield return StartDialogueAndSketch(
            2,
            SketchType.Enemy,
            enemy.EnemyData.EnemyName,
            enemy.EnemyData.Description
        );
        // Start the third dialogue and sketching sequence
        if (
            UIManager.Instance.PlayerUI != null
            && UIManager.Instance.PlayerUI.SpellButtons != null
            && UIManager.Instance.PlayerUI.SpellButtons.Count > 0
        )
        {
            enemy.LoadSprite();
            yield return StartDialogueAndSketch(
                3,
                SketchType.Spell,
                UIManager
                    .Instance.PlayerUI.SpellButtons[0]
                    .GetComponent<SpellButton>()
                    .Spell.SpellData.SpellName,
                UIManager
                    .Instance.PlayerUI.SpellButtons[0]
                    .GetComponent<SpellButton>()
                    .Spell.SpellData.Description
            );
            while (
                UIManager.Instance.PlayerUI.SpellButtons[0].GetComponent<Button>().interactable
                == false
            )
            {
                Debug.Log("Waiting for spell to be unlocked");
                UIManager
                    .Instance.PlayerUI.SpellButtons[0]
                    .GetComponent<SpellButton>()
                    .UnlockSpell();
                yield return null;
            }

            yield return new WaitUntil(
                () =>
                    UIManager.Instance.PlayerUI.SpellButtons[0].GetComponent<Button>().interactable
                    == true
            );
            TurnManager.Instance.StartBattle();
        }
        else
        {
            Debug.LogError("PlayerUI or SpellButtons not initialized.");
            yield break;
        }
    }

    public IEnumerator StartDialogueAndSketch(
        int dialogueIndex,
        SketchType sketchType,
        string name,
        string description
    )
    {
        bool dialogueEnded = false;
        bool imageSaved = false;

        // Start the dialogue
        UIManager.Instance.ClosePlayerCanvas();
        UIManager.Instance.OpenDialogueCanvas();
        DialogueManager.Instance.StartDialogue(DialogueManager.Instance.Dialogues[dialogueIndex]);
        DialogueManager.Instance.OnDialogueEnd.AddListener(() => dialogueEnded = true);

        // Wait for the dialogue to end
        yield return new WaitUntil(() => dialogueEnded);

        // Open the sketcher
        UIManager.Instance.OpenSketchCanvas(sketchType, name, description);
        UIManager.Instance.CloseDialogueCanvas();
        UIManager.Instance.CloseGameCanvas();
        Sketcher.Instance.OnImageSaved.AddListener(() => imageSaved = true);

        // Wait for the sketching to be done using OnImageSaved event
        yield return new WaitUntil(() => imageSaved);
        UIManager.Instance.CloseSketchCanvas();
        UIManager.Instance.OpenGameCanvas();
        UIManager.Instance.OpenPlayerCanvas();
    }

    [ContextMenu("Reset Game")]
    public void ResetGame()
    {
        string path = Path.Combine(Application.persistentDataPath, "sketches");
        string newPath = path + " run " + _run;
        Directory.Move(path, newPath);
        Directory.CreateDirectory(path);
        PlayerPrefs.SetInt("run", _run + 1);
        PlayerPrefs.Save();

        // Reset player
        Player.Reset();

        // Reset spell buttons
        foreach (Button button in UIManager.Instance.PlayerUI.SpellButtons)
        {
            button.GetComponent<SpellButton>().Reset();
        }

        foreach (
            Enemy enemy in WaveManager.Instance.Waves.Select(wave =>
                wave.Enemy.GetComponent<Enemy>()
            )
        )
        {
            enemy.Reset();
            Destroy(enemy.gameObject);
        }

        // Reset other managers
        WaveManager.Instance.ResetWaveManager();
        UIManager.Instance.ResetUIManager();
        TurnManager.Instance.ResetTurnManager();
        Debug.Log("Game reset");
    }
}
