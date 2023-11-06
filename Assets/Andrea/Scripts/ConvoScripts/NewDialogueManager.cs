using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[System.Serializable]
public class NewDialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Queue<string> sentences;
    public Queue<string> charnames;

    public string[] names;
    
    public Image Currenticon;
    public Sprite CharacterOne;
    public Sprite CharacterTwo;

    void Start()
    {
        sentences = new Queue<string>();
        charnames = new Queue<string>();
        
    }
    public void StartDialogue(Dialogue dialogue)
    {

        nameText.text = dialogue.name;
        sentences.Clear();

        

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        foreach (string name in names)
        {
            charnames.Enqueue(name);
            if (name == "Raymond")
            {
                Currenticon.sprite = CharacterOne;
            }
            else if (name == "Richard")
            {
                Currenticon.sprite = CharacterTwo;
            }
            
            

        }

        DisplayNextSentence();
        DisplayNextName();
    }
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    public void DisplayNextName()
    {
        if (charnames.Count == 0)
        {
            EndDialogue();
            return;
        }
        string name = charnames.Dequeue();
        nameText.text = name;
        if (name == "Raymond")
        {
            Currenticon.sprite = CharacterOne;
        }
        else if (name == "Richard")
        {
            Currenticon.sprite = CharacterTwo;
        }


    }

    void EndDialogue()
    {
        Debug.Log("End of conversation.");
    }
}

   
