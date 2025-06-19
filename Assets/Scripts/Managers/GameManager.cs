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
        // Find the next available run number by checking for existing folders
        int lastRun = PlayerPrefs.GetInt("run", 1);
        int nextRun = lastRun + 1;
        string sketchesBasePath = Path.Combine(Application.persistentDataPath, "sketches");
        string prevRunPath = sketchesBasePath + " run " + lastRun;
        while (Directory.Exists(prevRunPath))
        {
            lastRun++;
            prevRunPath = sketchesBasePath + " run " + lastRun;
        }
        // Move the old sketches folder if it exists
        if (Directory.Exists(sketchesBasePath))
        {
            Directory.Move(sketchesBasePath, prevRunPath);
        }
        // Find a free run number for the new session
        while (Directory.Exists(sketchesBasePath + " run " + nextRun))
        {
            nextRun++;
        }
        _run = nextRun;
        PlayerPrefs.SetInt("run", _run);
        PlayerPrefs.Save();
        // Create a new safe folder for sketches
        Directory.CreateDirectory(sketchesBasePath);

        // Unlock the first spell button and lock the others
        if (UIManager.Instance.PlayerUI != null && UIManager.Instance.PlayerUI.SpellButtons != null)
        {
            for (int i = 0; i < UIManager.Instance.PlayerUI.SpellButtons.Count; i++)
            {
                SpellButton spellButton = UIManager
                    .Instance.PlayerUI.SpellButtons[i]
                    .GetComponent<SpellButton>();
                if (spellButton != null)
                {
                    if (i == 0)
                    {
                        spellButton.UnlockSpell();
                    }
                    else
                    {
                        spellButton.Reset();
                    }
                }
            }
        }
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
        Debug.Log("Waiting for enemy to spawn");
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
            //wait until the sketch is saved
            yield return new WaitUntil(() => Sketcher.Instance.OnImageSaved != null);
            UIManager
                .Instance.PlayerUI.SpellButtons[0]
                .GetComponent<SpellButton>()
                .Spell.LoadSprite();

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
        _run++;
        string path = Path.Combine(Application.persistentDataPath, "sketches");
        string newPath = path + " run " + _run;

        try
        {
            if (Directory.Exists(path))
            {
                Directory.Move(path, newPath);
            }
            Directory.CreateDirectory(path);
        }
        catch (IOException ex)
        {
            Debug.LogError("Failed to reset directory: " + ex.Message);
            return;
        }

        PlayerPrefs.SetInt("run", _run);
        PlayerPrefs.Save();

        // Reset player
        Player.Reset();

        // Reset spell buttons
        foreach (Button button in UIManager.Instance.PlayerUI.SpellButtons)
        {
            button.GetComponent<SpellButton>().Reset();
        }

        // Destroy existing enemy
        GameObject existingEnemy = GameObject.Find("Enemy(Clone)");
        if (existingEnemy != null)
        {
            Destroy(existingEnemy);
        }

        // Reset other managers
        WaveManager.Instance.ResetWaveManager();
        UIManager.Instance.ResetUIManager();
        TurnManager.Instance.ResetTurnManager();

        // Reinitialize UI
        if (UIManager.Instance.PlayerUI != null && UIManager.Instance.PlayerUI.SpellButtons != null)
        {
            for (int i = 0; i < UIManager.Instance.PlayerUI.SpellButtons.Count; i++)
            {
                SpellButton spellButton = UIManager
                    .Instance.PlayerUI.SpellButtons[i]
                    .GetComponent<SpellButton>();
                if (spellButton != null)
                {
                    if (i == 0)
                    {
                        spellButton.UnlockSpell();
                    }
                    else
                    {
                        spellButton.Reset();
                    }
                }
            }
        }

        // Go back to menu canvas
        UIManager.Instance.CloseAllCanvas();
        UIManager.Instance.OpenMenuCanvas();

        Debug.Log("Game reset");

        // Start the game sequence immediately after reset
        StartCoroutine(StartGameSequence());
    }
}
