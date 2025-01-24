using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    public Player b_Player;

    private int m_run = 1;

    private void Start()
    {
        PlayerPrefs.SetInt("run", m_run);
    }

    public void StartDialogue(
        int dialogueIndex,
        SketchType sketchType,
        string name,
        string description
    )
    {
        UIManager.Instance.ClosePlayerCanvas();
        UIManager.Instance.OpenDialogueCanvas();
        DialogueManager.Instance.StartDialogue(DialogueManager.Instance.dialogues[dialogueIndex]);
        DialogueManager.Instance.OnDialogueEnd.AddListener(
            () => UIManager.Instance.OpenSketchCanvas(sketchType, name, description)
        );
        DialogueManager.Instance.OnDialogueEnd.AddListener(
            () => UIManager.Instance.CloseGameCanvas()
        );
        DialogueManager.Instance.OnDialogueEnd.AddListener(OnDialogueEnd);
    }

    private void OnDialogueEnd()
    {
        UIManager.Instance.CloseDialogueCanvas();
    }

    [ContextMenu("Reset Game")]
    public void ResetGame()
    {
        string path = Path.Combine(Application.persistentDataPath, "sketches");
        string newPath = path + "run " + m_run;
        Directory.Move(path, newPath);
        Directory.CreateDirectory(path);
        PlayerPrefs.SetInt("run", m_run + 1);
    }
}
