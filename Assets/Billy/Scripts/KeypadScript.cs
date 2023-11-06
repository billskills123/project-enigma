using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeypadScript : MonoBehaviour {
    [Header("Objects")]
    [SerializeField] private Image keypad;
    [SerializeField] private GameObject splitscreenDivider;
    [SerializeField] private AudioSource keypadAudioSource;
    [SerializeField] private GameObject keypadObject;
    [SerializeField] private GameObject keypadTextObject;
    [SerializeField] private TMP_Text keypadText;
    public GameObject keypadTrigger;
    [SerializeField] private AudioClip[] keypadBeeps;
    public TMP_Text noteText;

    public string correctPassword = ""; //Password can be set to anything
    private string currentGuess = "";
    private string keypadType = "";

    public void CloseKeypad() {
        StartCoroutine(UILerp("KeypadClose", keypad, Vector2.zero, ""));

        GameObject[] playerArray = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in playerArray) { 
            player.GetComponent<PlayerScript>().inKeypadTrigger = false;
        }
    }

    public void OpenKeypad() {
        EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        eventSystem.SetSelectedGameObject(GameObject.Find("Submit"));
    }

    public IEnumerator UILerp(string lerpType, Image uiObject, Vector2 endPosition, string newKeypadType) {
        if (lerpType == "KeypadOpen") {
            keypad.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            endPosition = new Vector2(0, 0);
            keypadType = newKeypadType;
        }
        else if (lerpType == "KeypadClose") {
            endPosition = new Vector2(0, 1000);
        }
        else {
            Debug.LogError("Error. lerpType not set up properly");
        }

        //Create Variables
        Vector2 startPosition = uiObject.rectTransform.anchoredPosition;
        float time = 0;

        //Lerp to actually move the UI
        while (time < 1.0f) {
            uiObject.rectTransform.anchoredPosition = LerpLibrary.UILerp(startPosition, endPosition, LerpLibrary.InOutBackEase(time));
            time += Time.deltaTime;
            yield return null;
        }
        uiObject.rectTransform.anchoredPosition = endPosition;
        yield return new WaitUntil(() => uiObject.rectTransform.anchoredPosition == endPosition && lerpType == "KeypadClose");
        splitscreenDivider.SetActive(true);
        gameObject.SetActive(false);
    }

    public void KeypadFunction(string keypadInput) {
        //Checks to see if the current input can be added to the display
        if (keypadText.text.Length < correctPassword.Length && keypadInput != "Clear" && keypadInput != "Submit") {
            currentGuess = keypadText.text += keypadInput;
            keypadAudioSource.clip = keypadBeeps[Random.Range(0, 9)];
            keypadAudioSource.Play();
        }
        //If the player clicks submit it checks if the guess is correct or not
        else if (keypadInput == "Submit") {
            if (keypadText.text.Length == correctPassword.Length && currentGuess == correctPassword) {
                keypadText.text = "Correct!";
                keypad.GetComponent<Image>().color = new Color(0, 1, 0, 1);
                keypadAudioSource.clip = keypadBeeps[10];
                keypadAudioSource.Play();
                StartCoroutine(CorrectGuess());
            }
            else if (currentGuess != correctPassword) {
                keypadText.text = "Incorrect!";
                currentGuess = "";
                keypad.GetComponent<Image>().color = new Color(1, 0, 0, 1);
                keypadAudioSource.clip = keypadBeeps[11];
                keypadAudioSource.Play();
                StartCoroutine(IncorrectGuess());
            }
        }
        //If the player presses clear it empties the current guess
        else if (keypadInput == "Clear") {
            keypadText.text = "";
        }
        //Resets the guess functionality on a clear or start event
        else if (keypadText.text.Length == 0) {
            currentGuess = "";
            keypad.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
        }
    }

    //Allows the keypad to turn red on an incorrect guess
    private IEnumerator IncorrectGuess() {
        yield return new WaitForSeconds(2.5f);
        keypadText.text = "";
        keypad.GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

    //Allows the keypad to turn green on a correct guess
    private IEnumerator CorrectGuess() {
        yield return new WaitForSeconds(2.5f);
        keypadText.text = "";
        keypadTrigger.SetActive(false);
        UnlockPlayer();
        StartCoroutine(UILerp("KeypadClose", keypad, Vector2.zero, ""));

        GameObject[] playerArray = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in playerArray) {
            player.GetComponent<PlayerScript>().inKeypadTrigger = false;
        }
    }

    //Unlocks the player when they exit out of the keypad
    public void UnlockPlayer() {
        Cursor.lockState = CursorLockMode.Locked;
        GameObject[] playerArray = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] playerCanvas = GameObject.FindGameObjectsWithTag("PlayerCanvas");

        for (int i = 0; i < playerArray.Length; i++) {
            playerArray[i].GetComponent<PlayerScript>().unlocked = true;
            playerCanvas[i].GetComponent<Canvas>().enabled = true;

            if(keypadType == "Hallway") {
                playerArray[i].GetComponent<PlayerScript>().roomsUnlocked = true;
            }
        }
    }
}