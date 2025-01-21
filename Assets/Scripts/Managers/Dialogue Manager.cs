using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField]
    private Queue<string> sentences;
    [SerializeField]
    private TMPro.TextMeshProUGUI dialogueText;
    [SerializeField]
    private float typingSpeed = 0.05f;
    [SerializeField]
    private List<Dialogue> dialogues;

    void Start()
    {
        sentences = new Queue<string>();
        dialogues = new List<Dialogue>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        sentences.Clear();

        foreach (string sentence in dialogue.messages)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
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

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void EndDialogue()
    {
        Debug.Log("End of conversation.");
    }

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    [TextArea(3, 10)]
    public string[] messages;
}
}