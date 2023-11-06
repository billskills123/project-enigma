using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    [Header("Objects")]
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Image keypad;
    [SerializeField] private Image dialogueBox;
    [SerializeField] private Image splitscreenDivider;
    [SerializeField] private FadeScript splitscreenDividerFadeScript;
    [SerializeField] private Image blackScreen;
    [SerializeField] private Image endGameScreen;
    [SerializeField] private Image newspaper;
    [SerializeField] private Animator newspaperAnimator;
    [SerializeField] private FadeScript newspaperFadeScript;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Light2D[] lightArray;
    [SerializeField] private Light2D globalLight;
    [SerializeField] private GameObject fireplaceObject;
    [SerializeField] private GameObject enterFinalRoomTrigger;

    [Header("Pause Menu Objects")]
    [SerializeField] private Canvas pauseMenuCanvas;
    [SerializeField] private Image pauseMenu;
    [SerializeField] private Image optionsMenu;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TMP_Text soundSliderText;
    [SerializeField] private TMP_Text musicSliderText;

    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;

    [Header("Game Settings")]
    [SerializeField] private float timer;
    [SerializeField] private bool timerEnabled;
    public bool doorsLocked = false;
    public int playersInKitchen;
    public int playersInLivingRoom;
    public int itemNum = 0;

    //Relating to the light plate puzzles
    int index = 0;
    string currentGuess = "";

    //Resets the timer
    private void Start() {
        timer = 0.0f;
        mainCanvas.gameObject.SetActive(true);
        StartCoroutine(StartGame());
    }

    //Coroutine for fading into the game
    private IEnumerator StartGame() {
        gameObject.GetComponent<PlayerSpawnScript>().OnStart(); //Used to spawn the players
        yield return new WaitForSeconds(2.5f); //Delay used to make sure the scene is fully loaded

        mainCanvas.GetComponent<FadeScript>().CanvasFade("Close", blackScreen.gameObject, 1.0f); //Fades the blackscreen away

        yield return new WaitUntil(() => blackScreen.gameObject.GetComponent<CanvasGroup>().alpha == 0);
        blackScreen.gameObject.SetActive(false);
        splitscreenDivider.gameObject.SetActive(true);
        splitscreenDividerFadeScript.CanvasFade("Open", splitscreenDivider.gameObject, 2.5f);
        timerEnabled = true;

        GameObject[] playerArray = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < playerArray.Length; i++) {
            playerArray[i].GetComponent<PlayerScript>().unlocked = true;
        }
    }

    //Updates the timer
    private void Update() {
        if (timerEnabled) {
            timer += Time.deltaTime;
        }
    }

    //Makes the pause menu appear
    public IEnumerator PauseMenuAppear() {
        mainCanvas.gameObject.SetActive(false);
        pauseMenuCanvas.gameObject.SetActive(true);
        timerEnabled = false;
        StartCoroutine(keypad.GetComponent<KeypadScript>().UILerp("KeypadOpen", pauseMenu, Vector2.zero, ""));
        yield return new WaitUntil(() => pauseMenu.rectTransform.anchoredPosition == new Vector2(0, 0));
        EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        eventSystem.SetSelectedGameObject(GameObject.Find("ResumeButton"));
    }

    //Makes the pause menu disappear
    public IEnumerator PauseMenuDisappear() {
        StartCoroutine(keypad.GetComponent<KeypadScript>().UILerp("KeypadClose", pauseMenu, Vector2.zero, ""));
        timerEnabled = true;
        yield return new WaitUntil(() => pauseMenu.rectTransform.anchoredPosition == new Vector2(0, 1000));
        pauseMenuCanvas.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(true);
    }

    //Unpauses the game
    public void UnPause() {
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(PauseMenuDisappear());
        GameObject[] playerArray = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < playerArray.Length; i++) {
            playerArray[i].GetComponent<PlayerScript>().ShowCanvas();
            playerArray[i].GetComponent<PlayerScript>().unlocked = true;
        }
    }

    //Calls the open coroutine
    public void OptionsMenuOpen() {
        StartCoroutine(OptionsMenuOpenCoroutine());
    }

    //Closes the pause menu and opens the options menu
    private IEnumerator OptionsMenuOpenCoroutine() {
        StartCoroutine(keypad.GetComponent<KeypadScript>().UILerp("KeypadClose", pauseMenu, Vector2.zero, ""));
        yield return new WaitUntil(() => pauseMenu.rectTransform.anchoredPosition == new Vector2(0, 1000));

        pauseMenu.gameObject.SetActive(false);
        optionsMenu.gameObject.SetActive(true);

        StartCoroutine(keypad.GetComponent<KeypadScript>().UILerp("KeypadOpen", optionsMenu, Vector2.zero, ""));

        EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        eventSystem.SetSelectedGameObject(GameObject.Find("CloseButton"));
    }

    //Calls the close coroutine
    public void OptionsMenuClose() {
        StartCoroutine(OptionsMenuCloseCoroutine());
    }

    //Closes the options menu and opens the pause menu
    private IEnumerator OptionsMenuCloseCoroutine() {
        StartCoroutine(keypad.GetComponent<KeypadScript>().UILerp("KeypadClose", optionsMenu, Vector2.zero, ""));
        yield return new WaitUntil(() => optionsMenu.rectTransform.anchoredPosition == new Vector2(0, 1000));

        optionsMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(true);

        StartCoroutine(keypad.GetComponent<KeypadScript>().UILerp("KeypadOpen", pauseMenu, Vector2.zero, ""));

        EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        eventSystem.SetSelectedGameObject(GameObject.Find("ResumeButton"));
    }

    //Changes the slider text values
    public void ChangeSliderText(string sliderType) {
        if (sliderType == "Sound") {
            soundSliderText.text = soundSlider.value.ToString() + "%";
            audioSource.Play();
        } else if (sliderType == "Music") {
            musicSliderText.text = musicSlider.value.ToString() + "%";
        }
    }

    //Returns to the menu
    public void ReturnToMenu() {
        SceneManager.LoadScene("MainMenu"); //Change to the menu scene
    }

    //Used for ending the game.
    public IEnumerator EndGame() {
        timerEnabled = false;
        GameObject[] playerArray = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < playerArray.Length; i++) {
            playerArray[i].GetComponent<PlayerScript>().unlocked = false;
            StartCoroutine(playerArray[i].GetComponent<PlayerScript>().FadeUI());
        }

        yield return new WaitUntil(() => playerArray[0].GetComponent<PlayerScript>().interactTextVisible == false);
        blackScreen.gameObject.SetActive(true);
        mainCanvas.GetComponent<FadeScript>().CanvasFade("Open", blackScreen.gameObject, 1.0f);

        yield return new WaitUntil(() => blackScreen.GetComponent<CanvasGroup>().alpha == 1);
        newspaper.gameObject.SetActive(true);
        newspaperAnimator.SetTrigger("ThrowPaper");

        yield return new WaitForSeconds(3f);
        newspaperFadeScript.CanvasFade("Close", newspaper.gameObject, 2.5f);
        yield return new WaitUntil(() => newspaper.GetComponent<CanvasGroup>().alpha == 0);

        endGameScreen.gameObject.SetActive(true);
        StartCoroutine(keypad.GetComponent<KeypadScript>().UILerp("KeypadOpen", endGameScreen, Vector2.zero, ""));

        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);
        timerText.text = "Time Taken: " + minutes + " minutes " + seconds + " seconds";

        EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        eventSystem.SetSelectedGameObject(GameObject.Find("MenuButton"));
        Cursor.lockState = CursorLockMode.None;
    }

    //Used for changing the lights
    public void LightPlate(char plateValue, Light2D currentLight) {
        string correctCode = "12345678";

        currentLight.color = new(0, 1, 0, 1);

        if (index < correctCode.Length) {
            if (plateValue == correctCode[index]) {
                Debug.Log("Correct Guess");
                index++;
                currentGuess += plateValue;

                if (index == correctCode.Length && currentGuess != correctCode) {
                    index = 0;
                    currentGuess = "";

                    foreach (Light2D light in lightArray) {
                        StartCoroutine(ChangeLightRed(light));
                    }
                }
                else if (index == correctCode.Length && currentGuess == correctCode) {
                    GameObject[] playerArray = GameObject.FindGameObjectsWithTag("Player");
                    for (int i = 0; i < playerArray.Length; i++) {
                        playerArray[i].GetComponent<PlayerScript>().canExitRooms = true;
                        playerArray[i].GetComponent<PlayerScript>().completedLightPuzzle = true;

                        if (playerArray[i].GetComponent<PlayerScript>().currentLightLevel == playerArray[i].GetComponent<PlayerScript>().lowLevelLight) {
                            globalLight.intensity = playerArray[i].GetComponent<PlayerScript>().lowLevelLight;
                        } else {
                            globalLight.intensity = playerArray[i].GetComponent<PlayerScript>().maxLevelLight;
                        }
                    }
                }
            }
            else {
                Debug.Log("Incorrect Guess. Starting Over");
                index = 0;
                currentGuess = "";

                foreach (Light2D light in lightArray) {
                    StartCoroutine(ChangeLightRed(light));
                }
            }
        }
    }

    private IEnumerator ChangeLightRed(Light2D light) {
        yield return new WaitForSeconds(2.5f);
        light.color = new(1, 0, 0, 1);
    }

    public void LockDoors() {
        if(playersInKitchen == 1 && playersInLivingRoom == 1) {
            GameObject.Find("Kitchen").GetComponent<AudioSource>().Play();
            globalLight.intensity = 0;

            GameObject[] playerArray = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < playerArray.Length; i++) {
                playerArray[i].GetComponent<PlayerScript>().canExitRooms = false;
            }
        }
    }

    //Chuck items onto le pedestal
    public void PedestalItems() {
        itemNum++;
        GameObject[] playerArray = GameObject.FindGameObjectsWithTag("Player");

        if (itemNum == 4 /*PROBABLY WANT TO MOVE THIS BIT SOMEWHERE ELSE ONCE SMALL ROOM IS ADDED && playerArray[0].GetComponent<PlayerScript>().completedLightPuzzle == true */) {
            StartCoroutine(FireplaceLerp(new Vector3(22.67f, 38.578f, 0f), 1f));
        }
    }

    private IEnumerator FireplaceLerp(Vector3 endPosition, float duration) {
        float t = 0;
        Vector3 startPosition = fireplaceObject.transform.position;
        fireplaceObject.GetComponent<AudioSource>().Play();

        while (t < duration) {
            fireplaceObject.transform.position = Vector3.Lerp(startPosition, endPosition, LerpLibrary.InOutEase(t));
            t += Time.deltaTime;
            yield return null;
        }
        fireplaceObject.transform.position = endPosition;
        fireplaceObject.GetComponent<AudioSource>().Stop();
        enterFinalRoomTrigger.SetActive(true);
    }
}