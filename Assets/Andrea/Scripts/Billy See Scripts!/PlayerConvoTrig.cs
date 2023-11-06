using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConvoTrig : MonoBehaviour
{
    public DialogueTrigger trigger;
    public void TriggerDialogue()
        {
        trigger.StartDialogue();
    }

}
//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Player") == true)
//                trigger.StartDialogue();
//    }
//}
