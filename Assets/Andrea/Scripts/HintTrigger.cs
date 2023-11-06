using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintTrigger : MonoBehaviour
{
    public GameObject Convo;


    public StoryScript triggerScript;
    public GameObject DialogueBox;

    private void Awake()
    {
        Convo.SetActive(false);
    }
    private void OnTriggerEnter2D( Collider2D Other )
    {
       
        if (Other.tag == "Player") 
        {
            DialogueBox.SetActive(true);
            triggerScript.StartHintBox();

            Convo.SetActive(true);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            DialogueBox.SetActive(false);
            Convo.SetActive(false);
        }
    }

}
