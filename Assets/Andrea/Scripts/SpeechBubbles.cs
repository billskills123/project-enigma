using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;

public class SpeechBubbles : MonoBehaviour
{
    [SerializeField] private GameObject textBubbleObject;
    [SerializeField] private Light2D globalLight;
    [SerializeField] private Vector3 fireplacePosition;
    [SerializeField] private TMP_Text textbubble;
    [SerializeField] private FadeScript fadeScript;
    //ontriggerenter collider or collision reference speech bubble enable 
    //switch statement collision.name
    //case string name of trigger example donut trigger colon then break name of object
    //ontriggerexit event that disables the speech bubble

    private void Start() {
        globalLight = GameObject.Find("GlobalLight").GetComponent<Light2D>();
    }

    public void OnEnter(Collider2D collider, string characterName) {

        if(characterName == "Raymond") { //Fat guy
            switch (collider.name) {
                case "KeypadTriggerBasement":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "Needs 8 digits. We got all the clues?";
                    break;
                case "CrowbarTrigger":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "This looks like it might be useful...";
                    break;
                case "BookcaseInteract":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "Help me move this thing..";
                    break;
                case "UVLightTrigger":
                    if (globalLight.intensity != 0) {
                        gameObject.SetActive(true);
                        fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                        textbubble.text = "Hm, let's try with the lights out... ";
                    }
                    break;
                case "RopeTrigger":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "Hmm... Reckon this could be used with something...  ";
                    break;
                case "Summer":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "A criminal with artistic taste...";
                    break;
                case "DonutTrigger":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "WHO would leave a half eaten donut?!";
                    break;
                case "KDoor":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "I think we should split up and look for clues.";
                    break;
                case "FishBowl":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "Who let this guy keep pets..";
                    break;
                case "LightPuzzleSolved":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "One step closer to getting out of here.";
                    break;
                case "GagTrigger":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "Well...";
                    break;
                case "KnifeInteract":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "Looks weighted.. Let's look around.";
                    break;
                case "Fireplace":
                    if (GameObject.Find("Fireplace").transform.position == fireplacePosition) {
                        gameObject.SetActive(true);
                        fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                        textbubble.text = "I wonder if this does something later..";
                    }
                    break;
                case "XRoom":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "Of course. A secret room.";
                    break;
                case "Key":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "Donuts here I come.";
                    break;
            }
        }
        
        else if(characterName == "Richard") { //Thin guy
            switch (collider.name) {
                case "KeypadTriggerBasement":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "Needs 8 digits. We got all the clues?";
                    break;
                case "CrowbarTrigger":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "This looks like it might be useful...";
                    break;
                case "LetterTrigger":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "The killer sure sounds desperate. Almost feel sorry for 'em";
                    break;
                case "BookcaseInteract":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "Help me move this thing..";
                    break;
                case "UVLightTrigger":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "Hm, let's try with the lights out... ";
                    break;
                case "RopeTrigger":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "Hmm... Reckon this could be used with something...  ";
                    break;
                case "HallwayEntrance2":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "Huh. Wonder if this guy likes poetry too?";
                    break;
                case "LDoor":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "You take that room, I've got this one.";
                    break;
                case "FishBowl":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "Poor fish...";
                    break;
                case "Ouija":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "A Ouija board? What sick games is the killer playing?";
                    break;
                case "Winter":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "Winter. Cold. Just like the killer.";
                    break;
                case "LightPuzzleComplete":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "Next time you tell us to split up. I'm not listening.";
                    break;
                case "GagTrigger":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "Hmm... Interesting.";
                    break;
                case "RopeInteract":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "Greyed out. Looks like something goes here.";
                    break;
                case "Fireplace":
                    if (GameObject.Find("Fireplace").transform.position == fireplacePosition) {
                        gameObject.SetActive(true);
                        fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                        textbubble.text = "Hm.. Looks like it might move.";
                    }
                    break;
                case "XRoom":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "Gotcha! Another case solved.";
                    break;
                case "Key":
                    gameObject.SetActive(true);
                    fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
                    textbubble.text = "Time to get out of here.";
                    break;
            }

        }
        
    }

    public void OnExit() {
        if (gameObject.activeSelf == true) {
            StartCoroutine(DisableBubble());
        }
    }

    public void CustomText(string speechText) {
        gameObject.SetActive(true);
        StartCoroutine(CustomTextFunction(speechText));
    }
    
    private IEnumerator CustomTextFunction(string speechText) {
        fadeScript.CanvasFade("Open", textBubbleObject, 2.5f);
        textbubble.text = speechText;
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(DisableBubble());
    }

    private IEnumerator DisableBubble() {
        fadeScript.CanvasFade("Close", textBubbleObject, 3.5f);
        yield return new WaitUntil(() => textBubbleObject.GetComponent<CanvasGroup>().alpha == 0);
        gameObject.SetActive(false);
    }
}
