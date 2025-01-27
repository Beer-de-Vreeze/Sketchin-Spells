using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : Singleton<DialogueManager>
{
    private Queue<string> _sentences;

    public Dialogue[] Dialogues;

    [SerializeField]
    private TextMeshProUGUI _dialogueText;

    [SerializeField, Range(0.01f, 1f), Tooltip("The lower the value, the faster the typing speed.")]
    private float _typingSpeed = 0.1f;

    [SerializeField, Range(0.5f, 5f), Tooltip("The time between messages.")]
    private float _timeBetweenMessages = 1.5f;

    [SerializeField, Tooltip("Dialogue to test the dialogue manager. Not USED IN THE GAME.")]
    private Dialogue _currentDialogue;

    public UnityEvent OnDialogueEnd = new UnityEvent();

    void OnEnable()
    {
        _sentences = new Queue<string>();
    }

    private void OnDisable()
    {
        OnDialogueEnd.RemoveAllListeners();
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

        if (dialogue.Messages == null)
        {
            Debug.LogError("Dialogue messages are null.");
            return;
        }

        _sentences.Clear();
        _currentDialogue = dialogue;

        foreach (string sentence in dialogue.Messages.Where(m => !string.IsNullOrEmpty(m)))
        {
            _sentences.Enqueue(sentence);
        }

        if (_sentences.Count == 0)
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
        if (Dialogues.Length > 0 && Dialogues[0] != null)
        {
            StartDialogue(Dialogues[0]);
        }
        else
        {
            Debug.LogError("No dialogues assigned to DialogueManager.");
        }
    }

    private void DisplayNextSentence()
    {
        if (_sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = _sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        _dialogueText.text = "";
        string[] lines = sentence.ToUpper().Split(new[] { '\n' }, StringSplitOptions.None);

        foreach (string line in lines)
        {
            foreach (char letter in line.ToCharArray())
            {
                _dialogueText.text += letter;
                yield return new WaitForSeconds(_typingSpeed);
            }
            _dialogueText.text += "\n";
        }
        yield return new WaitForSeconds(_timeBetweenMessages);
        DisplayNextSentence();
    }

    private void EndDialogue()
    {
        OnDialogueEnd.Invoke();
        OnDialogueEnd.RemoveAllListeners();
        Debug.Log("End of conversation.");
    }
}
