using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {
    [Header("Player Objects")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerSprite;
    [SerializeField] private Canvas playerCanvas;
    [SerializeField] private GameObject speechCanvas;
    [SerializeField] private TMP_Text interactTextObject;
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject journal;
    [SerializeField] private GameObject loveLetter;
    [SerializeField] private SpeechBubbles speechBubbles;
    [SerializeField] private GameObject itemList;
    [SerializeField] private GameObject ropeTrigger;
    [SerializeField] private GameObject ropePedestalTrigger;
    [SerializeField] private GameObject crowbarTrigger;
    [SerializeField] private GameObject crowbar;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private RuntimeAnimatorController[] animationControllers;

    [Header("Camera Settings")]
    [SerializeField] private Camera playerCamera;

    [Header("Player Attributes")]
    public string characterName = "";
    [SerializeField] private Vector2 playerMoveValue;
    [SerializeField] private float playerSpeed = 3.0f;
    [SerializeField] private bool interactEnabled = false;
    [SerializeField] private bool inTrigger = false;
    public bool inKeypadTrigger = false;
    public bool hasCrowbar = false;
    public bool hasRope = false;
    public bool hasGag = false;
    public bool hasHandcuffs = false;
    public bool hasKnife = false;
    public bool roomsUnlocked = false;
    public bool canExitRooms = true;
    public bool completedLightPuzzle = false;
    public bool hasKey = false;
    [SerializeField] private Vector3 tpDestination;
    public bool unlocked;
    public bool interactTextVisible;

    [Header("Lighting Settings")]
    public float lowLevelLight = 0.0f; //Default setting is 0.0f. Set to 1.0f for TESTING ONLY. <-- HAS TO BE CHANGED IN THE PLAYER PREFAB
    public float maxLevelLight = 0.75f; 
    public float currentLightLevel = 0f; 

    private string interactHelpText;
    private const string interactHelpTextFormat = "{Interact} To Interact!";
    private string triggerName;
    private GameObject mainCanvas;
    private FadeScript fadeScript;

    //Checks the player is in the trigger and displays the interact text
    private void OnTriggerEnter2D(Collider2D collision) {
        //Sets up the function for "interact" triggers
        if (collision.CompareTag("InteractTrigger")) {
            inTrigger = true;
            triggerName = collision.name;

            //Allows the interact text to fade in
            interactTextObject.gameObject.SetActive(true);
            interactTextObject.GetComponent<FadeScript>().CanvasFade("Open", interactTextObject.gameObject, 5.0f);

            interactHelpText = interactHelpTextFormat.Replace("{Interact}", GetComponent<PlayerInput>().actions["Interact"].GetBindingDisplayString());
            interactTextObject.text = interactHelpText;
        }
        //Sets up the function for "standing" triggers
        else if (collision.CompareTag("StandingTrigger")) {
            triggerName = collision.name;
            inTrigger = true;

            //Play sounds for the UV Light
            if(collision.name == "UVLightTrigger" && GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>().intensity == lowLevelLight) {
                AudioSource[] audioSources = collision.GetComponents<AudioSource>();
                audioSources[0].Play();
            }
        }
        speechBubbles.OnEnter(collision, characterName); //Fades in the speech bubble and ensures it displays correct text
    }

    //Checks the player is within the trigger and has pressed the interact button
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("InteractTrigger") && interactEnabled == true) {
            //Switch statement to handle functions for each trigger
            switch (triggerName){
                case "MonitorLightTrigger":
                    MonitorLightTrigger(collision);
                    break;
                case "KeypadTriggerBasement":
                    KeypadTrigger("Basement", collision);
                    inKeypadTrigger = true;
                    break;
                case "KeypadTriggerHallway":
                    KeypadTrigger("Hallway", collision);
                    inKeypadTrigger = true;
                    break;
                case "LightTrigger":
                    StartCoroutine(LightTrigger(collision));
                    break;
                case "CrowbarTrigger":
                    StartCoroutine(PickupCrowbar());
                    break;
                case "RopeTrigger":
                    StartCoroutine(PickupRope());
                    break;
                case "KeyTrigger":
                    StartCoroutine(PickupKey());
                    break;
                case "GagTrigger":
                    StartCoroutine(PickupGag());
                    break;
                case "HandcuffsTrigger":
                    StartCoroutine(PickupHandcuffs());
                    break;
                case "KnifeTrigger":
                    StartCoroutine(PickupKnife());
                    break;
                case "RopePedestalTrigger":
                    if(hasRope == true) {
                        GameObject rope = GameObject.Find("Rope");
                        collision.gameObject.SetActive(false);
                        ropeTrigger.SetActive(true);
                        rope.transform.position = new(15.809f, 34.754f, 0f);
                        rope.GetComponent<SpriteRenderer>().color = new(1, 1, 1, 1);
                        hasRope = false;
                        gameManager.GetComponent<GameManager>().PedestalItems();
                    } else {
                        speechBubbles.CustomText("I need a rope");
                    }
                    break;
                case "GagPedestalTrigger":
                    if (hasGag == true) {
                        GameObject gag = GameObject.Find("Gag");
                        gag.transform.position = new(23.418f, 34.526f, 0f);
                        gag.GetComponent<SpriteRenderer>().color = new(1, 1, 1, 1);
                        hasGag = false;
                        gameManager.GetComponent<GameManager>().PedestalItems();
                    } else {
                        speechBubbles.CustomText("I need a gag");
                    }
                    break;
                case "HandcuffPedestalTrigger":
                    if (hasHandcuffs == true) {
                        GameObject handcuffs = GameObject.Find("Handcuffs");
                        handcuffs.transform.position = new(21.528f, 36.065f, 0f);
                        handcuffs.transform.localScale = new(0.6f, 0.6f, 0.6f);
                        handcuffs.GetComponent<SpriteRenderer>().color = new(1, 1, 1, 1);
                        hasHandcuffs = false;
                        gameManager.GetComponent<GameManager>().PedestalItems();
                    } else {
                        speechBubbles.CustomText("I need some handcuffs");
                    }
                    break;
                case "KnifePedestalTrigger":
                    if (hasKnife == true) {
                        GameObject knife = GameObject.Find("Knife");
                        knife.transform.position = new(18.288f, 36.087f, 0f);
                        knife.GetComponent<SpriteRenderer>().color = new(1, 1, 1, 1);
                        hasKnife = false;
                        gameManager.GetComponent<GameManager>().PedestalItems();
                    } else {
                        speechBubbles.CustomText("I need a knife");
                    }
                    break;
                case "JournalTrigger":
                    StartCoroutine(OpenPaper(journal, collision));
                    break;
                case "LetterTrigger":
                    StartCoroutine(OpenPaper(loveLetter, collision));
                    break;
                case "ExitBasementTrigger":
                    tpDestination = GameObject.Find("HallwayTP").transform.position;
                    StartCoroutine(TeleportPlayer(tpDestination));
                    break;
                case "EnterBasementTrigger":
                    tpDestination = GameObject.Find("BasementTP").transform.position;
                    StartCoroutine(TeleportPlayer(tpDestination));
                    break;
                case "EnterLivingRoomTrigger":
                    if (roomsUnlocked == true) {
                        if (gameManager.GetComponent<GameManager>().playersInLivingRoom == 0 && completedLightPuzzle == false) {
                            tpDestination = GameObject.Find("LivingRoomTP").transform.position;
                            StartCoroutine(TeleportPlayer(tpDestination));
                            gameManager.GetComponent<GameManager>().playersInLivingRoom++;
                            gameManager.GetComponent<GameManager>().LockDoors();
                            canExitRooms = false;
                        }
                        else if (gameManager.GetComponent<GameManager>().playersInLivingRoom > 0 && completedLightPuzzle == false) {
                            speechBubbles.CustomText("Hmmm the door is locked...");
                        }
                        else {
                            tpDestination = GameObject.Find("LivingRoomTP").transform.position;
                            StartCoroutine(TeleportPlayer(tpDestination));
                        }
                    } 
                    else {
                        speechBubbles.CustomText("Hmmm the door is locked...");
                    }
                    break;
                case "EnterKitchenTrigger":
                    if (roomsUnlocked == true) {
                        if (gameManager.GetComponent<GameManager>().playersInKitchen == 0 && completedLightPuzzle == false) {
                            tpDestination = GameObject.Find("KitchenTP").transform.position;
                            StartCoroutine(TeleportPlayer(tpDestination));
                            gameManager.GetComponent<GameManager>().playersInKitchen++;
                            gameManager.GetComponent<GameManager>().LockDoors();
                            canExitRooms = false;
                        } 
                        else if (gameManager.GetComponent<GameManager>().playersInKitchen > 0 && completedLightPuzzle == false) {
                            speechBubbles.CustomText("Hmmm the door is locked...");
                        } 
                        else {
                            tpDestination = GameObject.Find("KitchenTP").transform.position;
                            StartCoroutine(TeleportPlayer(tpDestination));
                        }
                    } 
                    else {
                        speechBubbles.CustomText("Hmmm the door is locked...");
                    }
                    break;
                case "ExitKitchenTrigger":
                    if (canExitRooms == true) {
                        tpDestination = GameObject.Find("KitchenHallwayTP").transform.position;
                        StartCoroutine(TeleportPlayer(tpDestination));
                        gameManager.GetComponent<GameManager>().playersInKitchen--;
                    } 
                    else {
                        speechBubbles.CustomText("Hmmm the door is locked...");
                    }
                    break;
                case "ExitLivingRoomTrigger":
                    if (canExitRooms == true) {
                        tpDestination = GameObject.Find("LivingRoomHallwayTP").transform.position;
                        StartCoroutine(TeleportPlayer(tpDestination));
                        gameManager.GetComponent<GameManager>().playersInLivingRoom--;
                    } else {
                        speechBubbles.CustomText("Hmmm the door is locked...");
                    }
                    break;
                case "ExitHouseTrigger":
                    if (hasKey == true) {
                        StartCoroutine(gameManager.GetComponent<GameManager>().EndGame()); //-----USED FOR EXITING GAME-----
                    } else {
                        speechBubbles.CustomText("Hmmm we need a key...");
                    }
                    break;
                case "LivingRoomKitchenTrigger":
                    if (canExitRooms == true) {
                        tpDestination = GameObject.Find("KitchenTP2").transform.position;
                        StartCoroutine(TeleportPlayer(tpDestination));
                        gameManager.GetComponent<GameManager>().playersInLivingRoom--;
                        gameManager.GetComponent<GameManager>().playersInKitchen++;
                        gameManager.GetComponent<GameManager>().LockDoors();
                    } else {
                        speechBubbles.CustomText("Hmmm the door is locked...");
                    }
                    break;
                case "KitchenLivingRoomTrigger":
                    if (canExitRooms == true) {
                        tpDestination = GameObject.Find("LivingRoomTP2").transform.position;
                        StartCoroutine(TeleportPlayer(tpDestination));
                        gameManager.GetComponent<GameManager>().playersInKitchen--;
                        gameManager.GetComponent<GameManager>().playersInLivingRoom++;
                        gameManager.GetComponent<GameManager>().LockDoors();
                    } else {
                        speechBubbles.CustomText("Hmmm the door is locked...");
                    }
                    break;
                case "EnterFinalRoomTrigger":
                    tpDestination = GameObject.Find("FinalRoomTP").transform.position;
                    StartCoroutine(TeleportPlayer(tpDestination));
                    break;
                case "ExitFinalRoomTrigger":
                    tpDestination = GameObject.Find("LivingRoomTP3").transform.position;
                    StartCoroutine(TeleportPlayer(tpDestination));
                    break;
                default:
                    Debug.LogError("Error: " + triggerName + " is not a valid INTERACT trigger."); //Error statement showing which trigger is not set up correctly
                    break;
            }
            interactEnabled = false; 
        }
        else if(collision.CompareTag("StandingTrigger") && inTrigger == true) {
            //Switch statement to handle functions for each trigger
            switch (triggerName) {
                case "UVLightTrigger":
                    UVLightTrigger(collision);
                    break;
                default:
                    Debug.LogError("Error: " + triggerName + " is not a valid STANDING trigger."); //Error statement showing which trigger is not set up correctly
                    break;
            }
        }
    }

    //Opens the journal or letter
    private IEnumerator OpenPaper(GameObject paperType, Collider2D trigger) {
        //Fade out the interact text
        interactTextObject.GetComponent<FadeScript>().CanvasFade("Close", interactTextObject.gameObject, 5.0f);
        yield return new WaitUntil(() => interactTextObject.gameObject.GetComponent<CanvasGroup>().alpha == 0);
        interactTextObject.gameObject.SetActive(false);

        if(paperType == journal) {
            journal.SetActive(true);
            journal.GetComponent<FadeScript>().CanvasFade("Open", paperType, 2.0f); //Open the journal
        }
        else {
            loveLetter.SetActive(true);
            loveLetter.GetComponent<FadeScript>().CanvasFade("Open", paperType, 2.0f); //Open the letter
        }

        trigger.GetComponent<AudioSource>().Play(); //Play the sound
    }

    //Teleports the player to a room with a fading screen. When setting up make sure the TP destination does not end in a trigger.
    private IEnumerator TeleportPlayer(Vector3 tpDestination) {
        unlocked = false;
        interactTextObject.GetComponent<FadeScript>().CanvasFade("Close", interactTextObject.gameObject, 5.0f);

        //Fade out the text before starting the fade to black
        yield return new WaitUntil(() => interactTextObject.gameObject.GetComponent<CanvasGroup>().alpha == 0);
        interactTextObject.gameObject.SetActive(false);
        blackScreen.SetActive(true);
        blackScreen.GetComponent<FadeScript>().CanvasFade("Open", blackScreen, 1.0f);

        //Wait until the screen is black and play the door sound. 
        yield return new WaitUntil(() => blackScreen.GetComponent<CanvasGroup>().alpha == 1);
        GameObject.Find("Teleports").GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(player.GetComponent<AudioSource>().clip.length); //Wait until the sound is finished before teleporting the player and fading back to normal
        blackScreen.GetComponent<FadeScript>().CanvasFade("Close", blackScreen, 1.0f);
        player.transform.position = tpDestination;

        //Wait until the black has faded before allowing the player to move
        yield return new WaitUntil(() => blackScreen.GetComponent<CanvasGroup>().alpha == 0);
        blackScreen.SetActive(false);
        unlocked = true;
    }

    //Code relating to the crowbar
    private IEnumerator PickupCrowbar() {
        GameObject crowbar = GameObject.Find("Crowbar");
        crowbar.GetComponent<AudioSource>().Play();

        //Wait until the sound has finished and then make the crowbar disappear and give the player the crowbar
        yield return new WaitForSeconds(crowbar.GetComponent<AudioSource>().clip.length);
        crowbar.SetActive(false);
        hasCrowbar = true;
    }
    
    //Code relating to the rope
    private IEnumerator PickupRope() {
        GameObject rope = GameObject.Find("Rope");
        GameObject ropeTrigger = GameObject.Find("RopeTrigger");
        rope.GetComponent<AudioSource>().Play();

        //Wait until the sound has finished and then make the rope disappear and give the player the crowbar
        yield return new WaitForSeconds(rope.GetComponent<AudioSource>().clip.length);
        rope.GetComponent<SpriteRenderer>().color = new(1, 1, 1, 0);
        ropeTrigger.SetActive(false);
        ropePedestalTrigger.SetActive(true);
        hasRope = true;

        if (gameManager.GetComponent<GameManager>().itemNum > 0) {
            gameManager.GetComponent<GameManager>().itemNum--;
        }
    }

    private IEnumerator PickupKey() {
        GameObject key = GameObject.Find("Key");
        key.GetComponent<AudioSource>().Play();

        //Wait until the sound has finished and then make the rope disappear and give the player the crowbar
        yield return new WaitForSeconds(key.GetComponent<AudioSource>().clip.length);
        key.SetActive(false);
        hasKey = true;
    }

    //Code relating to the rope
    private IEnumerator PickupGag() {
        GameObject gag = GameObject.Find("Gag");
        GameObject gagTrigger = GameObject.Find("GagTrigger");
        gag.GetComponent<AudioSource>().Play();

        //Wait until the sound has finished and then make the rope disappear and give the player the crowbar
        yield return new WaitForSeconds(gag.GetComponent<AudioSource>().clip.length);
        gag.GetComponent<SpriteRenderer>().color = new(1, 1, 1, 0);
        gagTrigger.SetActive(false);
        hasGag = true;
    }

    //Code relating to the rope
    private IEnumerator PickupHandcuffs() {
        GameObject handcuffs = GameObject.Find("Handcuffs");
        GameObject handcuffsTrigger = GameObject.Find("HandcuffsTrigger");
        handcuffs.GetComponent<AudioSource>().Play();

        //Wait until the sound has finished and then make the rope disappear and give the player the crowbar
        yield return new WaitForSeconds(handcuffs.GetComponent<AudioSource>().clip.length);
        handcuffs.GetComponent<SpriteRenderer>().color = new(1, 1, 1, 0);
        handcuffsTrigger.SetActive(false);
        hasHandcuffs = true;
    }

    //Code relating to the rope
    private IEnumerator PickupKnife() {
        GameObject knife = GameObject.Find("Knife");
        GameObject knifeTrigger = GameObject.Find("KnifeTrigger");
        knife.GetComponent<AudioSource>().Play();

        //Wait until the sound has finished and then make the rope disappear and give the player the crowbar
        yield return new WaitForSeconds(knife.GetComponent<AudioSource>().clip.length);
        knife.GetComponent<SpriteRenderer>().color = new(1, 1, 1, 0);
        knifeTrigger.SetActive(false);
        hasKnife = true;
    }

    //Code relating to the monitor light
    private void MonitorLightTrigger(Collider2D monitorLightTrigger) {
        monitorLightTrigger.GetComponentInChildren<Light2D>().enabled = !monitorLightTrigger.GetComponentInChildren<Light2D>().enabled; //Turn the light on or off
        monitorLightTrigger.GetComponent<AudioSource>().Play(); //Play the click sound
    }

    //Code relating to the keypad
    private void KeypadTrigger(string keypadType, Collider2D keypadTrigger) {
        Transform splitscreenDivider = mainCanvas.transform.Find("SplitscreenDivider");
        Transform keypad = mainCanvas.transform.Find("Keypad");
        GameObject[] playerArray = GameObject.FindGameObjectsWithTag("Player");

        //Freeze both players and hide any text on the screen
        for (int i = 0; i < playerArray.Length; i++) {
            playerArray[i].GetComponent<PlayerScript>().unlocked = false;
            playerArray[i].GetComponentInChildren<Canvas>().enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        interactTextObject.GetComponent<FadeScript>().CanvasFade("Close", interactTextObject.gameObject, 5.0f);
        splitscreenDivider.gameObject.SetActive(false);

        //Open the keypad
        keypad.gameObject.SetActive(true);
        keypad.GetComponent<KeypadScript>().KeypadFunction("");
        keypad.GetComponent<KeypadScript>().OpenKeypad();
        StartCoroutine(keypad.GetComponent<KeypadScript>().UILerp("KeypadOpen", keypad.gameObject.GetComponent<Image>(), Vector2.zero, keypadType));


        //Set keypad password
        if(keypadType == "Basement") {
            keypad.GetComponent<KeypadScript>().correctPassword = "14121982";
            keypad.GetComponent<KeypadScript>().keypadTrigger = keypadTrigger.gameObject;
            keypad.GetComponent<KeypadScript>().noteText.text = "Code: dd/mm/yyyy";
        }
        else if(keypadType == "Hallway") {
            keypad.GetComponent<KeypadScript>().correctPassword = "3274";
            keypad.GetComponent<KeypadScript>().keypadTrigger = keypadTrigger.gameObject;
            keypad.GetComponent<KeypadScript>().noteText.text = "Code: _ _ _ _";
        }
    }

    //Code relating to the main lights
    private IEnumerator LightTrigger(Collider2D collision) {
        if (GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>().intensity == lowLevelLight && hasCrowbar == true) {
            //CALLED WHEN THE PLAYER HASNT OPENED THE BOX AND THE PLAYER HAS A CROWBAR
            hasCrowbar = false;
            crowbarTrigger.SetActive(false);
            crowbar.transform.localPosition = new Vector3(7.789f, 5.756f, 0.4f);
            crowbar.SetActive(true);
            GameObject.Find("PowerBox").GetComponent<SwitchSpriteScript>().ChangeSprite(1); //Swap the powerbox to the open version
            GameObject.Find("PowerBox").GetComponent<AudioSource>().Play(); //Plays opening sound
        }
        else if (GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>().intensity == lowLevelLight && GameObject.Find("PowerBox").GetComponent<SwitchSpriteScript>().currentSprite == 1 && hasRope == false) {
            //CALLED WHEN THE BOX HAS BEEN OPENED AND THE PLAYER DOESNT HAVE THE ROPE
            collision.GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(collision.GetComponent<AudioSource>().clip.length); //Wait until the sound has finished and turn on the lights
            GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>().intensity = maxLevelLight;
            currentLightLevel = 0.75f;

            //Wait 5 seconds and turn the lights off again
            yield return new WaitForSeconds(2.5f);
            GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>().intensity = lowLevelLight;
            currentLightLevel = 0f;
        }
        else if(GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>().intensity == lowLevelLight && GameObject.Find("PowerBox").GetComponent<SwitchSpriteScript>().currentSprite == 1 && hasRope == true) {
            //CALLED WHEN THE BOX IS OPEN AND THE PLAYER HAS THE ROPE
            hasRope = false;
            GameObject.Find("PowerBox").GetComponent<SwitchSpriteScript>().ChangeSprite(2); //Swap the powerbox to the rope version

            collision.GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(collision.GetComponent<AudioSource>().clip.length); //Wait until the sound has finished and turn on the lights
            GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>().intensity = maxLevelLight;
            currentLightLevel = 0.75f;
        }
        else if (GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>().intensity == maxLevelLight && GameObject.Find("PowerBox").GetComponent<SwitchSpriteScript>().currentSprite == 2 && hasRope == false) {
            //CALLED WHEN THE PLAYER HAS USED THE ROPE AND THE LIGHTS ARE ON
            hasRope = true;
            GameObject.Find("PowerBox").GetComponent<SwitchSpriteScript>().ChangeSprite(1); //Swap the powerbox to the rope version

            yield return new WaitForSeconds(2.5f);
            GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>().intensity = lowLevelLight;
            currentLightLevel = 0f;
        }
        else if (GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>().intensity == lowLevelLight && GameObject.Find("PowerBox").GetComponent<SwitchSpriteScript>().currentSprite == 0 && hasCrowbar == false) {
            //CALLED WHEN THE PLAYER HAS NOTHING AND THE BOX HASNT BEEN OPENED
            speechBubbles.CustomText("Hmmm... maybe I should use that crowbar?");
        }
    }

    //Code relating to the UV Light
    private void UVLightTrigger(Collider2D uvTrigger) {
        if (GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>().intensity == lowLevelLight) {
            uvTrigger.transform.Find("HiddenObjects").gameObject.SetActive(true); //Display the hidden objects
        }
        else if (GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>().intensity == maxLevelLight) {
            uvTrigger.transform.Find("HiddenObjects").gameObject.SetActive(false); //Ensure the hidden objects are hidden
            speechBubbles.CustomText("Hmmm maybe I should turn off the lights...");
        }
    }

    //Disables the interact text when the player exits the trigger
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("InteractTrigger")) {
            inTrigger = false;

            if (player.GetComponent<CanvasGroup>().alpha == 0.0f) {
                interactTextObject.gameObject.SetActive(false); //Wait until the text has faded before disabling it
            }

            if(collision.name == "JournalTrigger" || collision.name == "LetterTrigger") {
                StartCoroutine(UIExit(collision.name)); //Seperate function fot the pieces of paper
            }
            else {
                interactTextObject.GetComponent<FadeScript>().CanvasFade("Close", interactTextObject.gameObject, 5.0f);
            }
        }
        else if (collision.CompareTag("StandingTrigger")) {
            collision.transform.Find("HiddenObjects").gameObject.SetActive(false);

            if(collision.name == "UVLightTrigger") {
                collision.GetComponent<AudioSource>().Play();
            }
        }
        speechBubbles.OnExit(); //Fades out the speech bubble
    }

    //Fades UI for the love letter or journal
    private IEnumerator UIExit(string name) {
        if(name == "LetterTrigger") {
            if(loveLetter.GetComponent<CanvasGroup>().alpha == 1) {
                loveLetter.GetComponent<FadeScript>().CanvasFade("Close", loveLetter, 5.0f);
                yield return new WaitUntil(() => loveLetter.GetComponent<CanvasGroup>().alpha == 0); //Fade out the letter
                loveLetter.SetActive(false);
            }
            else {
                interactTextObject.GetComponent<FadeScript>().CanvasFade("Close", interactTextObject.gameObject, 5.0f);
                yield return new WaitUntil(() => interactTextObject.gameObject.GetComponent<CanvasGroup>().alpha == 0); //Fade out the interact text
                interactTextObject.gameObject.SetActive(false);
            }
        }
        else if(name == "JournalTrigger") {
            if (journal.GetComponent<CanvasGroup>().alpha == 1) {
                journal.GetComponent<FadeScript>().CanvasFade("Close", journal, 5.0f);
                yield return new WaitUntil(() => journal.GetComponent<CanvasGroup>().alpha == 0); //Fade out the journal
                journal.SetActive(false);
            }
            else {
                interactTextObject.GetComponent<FadeScript>().CanvasFade("Close", interactTextObject.gameObject, 5.0f);
                yield return new WaitUntil(() => interactTextObject.gameObject.GetComponent<CanvasGroup>().alpha == 0); //Fade out the interact text
                interactTextObject.gameObject.SetActive(false);
            }
        }
    }

    //Adds one strength to the totalStrength variable for the bookcase when the player is colliding
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Bookcase")) {
            GameObject.FindGameObjectWithTag("Bookcase").GetComponent<BookcaseScript>().totalStrength++; //Adds strength to the bookcase script. 2 strength needed to move it
        }
    }

    //Removes one strength to the totalStrength variable for the bookcase when the player is no longer colliding
    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Bookcase")) {
            GameObject.FindGameObjectWithTag("Bookcase").GetComponent<BookcaseScript>().totalStrength--; //Remove strength when not touching bookcase
        }
    }

    //Checks the player is within the trigger before allowing them to interact. Called when the "Interact" button is pressed
    private void OnInteract() {
        if (inTrigger == true) {
            interactEnabled = true;
        }
    }

    //Called when the player presses any "move" control
    private void OnMove(InputValue value) {
        playerMoveValue = value.Get<Vector2>();
    }

    private void OnInventory() {
        StartCoroutine(InventoryFade());
    }

    private IEnumerator InventoryFade() {
        if (itemList.activeSelf == false) {
            itemList.SetActive(true);
            yield return new WaitUntil(() => itemList.activeSelf == true);
            itemList.GetComponent<FadeScript>().CanvasFade("Open", itemList, 2.5f);
        }
        else if (itemList.activeSelf == true) {
            itemList.GetComponent<FadeScript>().CanvasFade("Close", itemList, 2.5f);
            yield return new WaitUntil(() => itemList.GetComponent<CanvasGroup>().alpha == 0);
            itemList.SetActive(false);
        }
    }

    //Pauses the game
    private void OnPause() {
        GameObject[] playerArray = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < playerArray.Length; i++) {
            if (playerArray[0].GetComponent<PlayerScript>().inKeypadTrigger == false && playerArray[1].GetComponent<PlayerScript>().inKeypadTrigger == false) {
                playerArray[i].GetComponent<PlayerScript>().unlocked = false;
                playerArray[i].GetComponent<PlayerScript>().HideUI();

                Cursor.lockState = CursorLockMode.None;
                StartCoroutine(gameManager.GetComponent<GameManager>().PauseMenuAppear());
            }
        }
    }

    //Used for fading out the UI. Called from other scripts
    public IEnumerator FadeUI() {
        interactTextObject.GetComponent<FadeScript>().CanvasFade("Close", interactTextObject.gameObject, 2.0f);
        yield return new WaitUntil(() => interactTextObject.gameObject.GetComponent<CanvasGroup>().alpha >= 1);
        interactTextVisible = false;
        playerCanvas.gameObject.SetActive(false);
    }

    //Used for hiding the player UI.
    public void HideUI() {
        playerCanvas.gameObject.SetActive(false);
    }

    //Enables the player canvas
    public void ShowCanvas() {
        playerCanvas.gameObject.SetActive(true);
    }

    //Moves the player based on a the X and Y dimension based on the input and keeps the camera with the player if the player is unlocked
    private void Update() {
        if (unlocked == true) {
            playerCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -1);
            transform.Translate(playerSpeed * Time.deltaTime * new Vector3(playerMoveValue.x, playerMoveValue.y, 0));
        }

        if(interactTextObject.GetComponent<CanvasGroup>().alpha == 1) {
            interactTextVisible = true;
        }
        else {
            interactTextVisible = false;
        }
    }

    //Used to set up the basic settings for the player
    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        unlocked = false;
        canExitRooms = true;

        //Find objects
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        mainCanvas = GameObject.Find("MainCanvas");
        fadeScript = mainCanvas.GetComponent<FadeScript>();
        GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>().intensity = lowLevelLight;
        ropeTrigger = GameObject.Find("RopeTrigger");
        ropePedestalTrigger = GameObject.Find("RopePedestalTrigger");
        crowbarTrigger = GameObject.Find("CrowbarTrigger");
        crowbar = GameObject.Find("Crowbar");

        //Set up the canvas scale
        playerCanvas.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        playerCanvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);

        if(characterName == "Richard") {
            playerSprite.GetComponent<SpriteRenderer>().sprite = sprites[0];
            playerSprite.GetComponent<Animator>().runtimeAnimatorController = animationControllers[0];
        }
        else if(characterName == "Raymond") {
            playerSprite.GetComponent<SpriteRenderer>().sprite = sprites[1];
            playerSprite.GetComponent<Animator>().runtimeAnimatorController = animationControllers[1];
        }
    }
}