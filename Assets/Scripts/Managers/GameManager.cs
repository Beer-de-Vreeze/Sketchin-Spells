using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    public Player b_Player;

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
        DialogueManager.Instance.OnDialogueEnd.AddListener(OnDialogueEnd);
    }

    private void OnDialogueEnd()
    {
        UIManager.Instance.CloseDialogueCanvas();
    }

    public void ResetGame()
    {
        string path = Path.Combine(Application.persistentDataPath, "sketches");
        string newPath = path + "run" + Random.Range(0, 1000000000);
        Directory.Move(path, newPath);
        Directory.CreateDirectory(path);
    }
}
