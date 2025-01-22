using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : Singleton<DialogueManager>
{
    private Queue<string> sentences;

    public Dialogue[] dialogues;

    [SerializeField]
    private TextMeshProUGUI dialogueText;

    [SerializeField, Range(0.01f, 1f), Tooltip("The lower the value, the faster the typing speed.")]
    private float typingSpeed = 0.1f;

    [SerializeField, Range(0.5f, 5f), Tooltip("The time between messages.")]
    private float timeBetweenMessages = 1.5f;

    [SerializeField, Tooltip("Dialogue to test the dialogue manager. Not USED IN THE GAME.")]
    private Dialogue currentDialogue;

    public UnityEvent OnDialogueEnd = new UnityEvent();

    void OnEnable()
    {
        sentences = new Queue<string>();
    }

    /// <summary>
    /// Starts the dialogue.
    /// needs a dialogue object to start the conversation.
    /// </summary>
    public void StartDialogue(Dialogue dialogue)
    {
        if (dialogue == null)
        {
            Debug.LogError("Dialogue is null.");
            return;
        }

        if (dialogue.messages == null)
        {
            Debug.LogError("Dialogue messages are null.");
            return;
        }

        sentences.Clear();
        currentDialogue = dialogue;

        foreach (string sentence in dialogue.messages.Where(m => !string.IsNullOrEmpty(m)))
        {
            sentences.Enqueue(sentence);
        }

        if (sentences.Count == 0)
        {
            Debug.LogWarning("No valid messages found in dialogue.");
            EndDialogue();
            return;
        }

        DisplayNextSentence();
    }

    [ContextMenu("Start Dialogue")]
    private void StartDialogueFromInspector()
    {
        if (dialogues.Length > 0 && dialogues[0] != null)
        {
            StartDialogue(dialogues[0]);
        }
        else
        {
            Debug.LogError("No dialogues assigned to DialogueManager.");
        }
    }

    private void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        string[] lines = sentence.ToUpper().Split(new[] { '\n' }, StringSplitOptions.None);

        foreach (string line in lines)
        {
            foreach (char letter in line.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
            dialogueText.text += "\n";
        }
        yield return new WaitForSeconds(timeBetweenMessages);
        DisplayNextSentence();
    }

    private void EndDialogue()
    {
        OnDialogueEnd.Invoke();
        OnDialogueEnd.RemoveAllListeners();
        Debug.Log("End of conversation.");
    }
}
