using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        FindObjectOfType<NewDialogueManager>().StartDialogue(dialogue);
        FindObjectOfType<NewDialogueManager>().DisplayNextSentence();
       
    }
}
