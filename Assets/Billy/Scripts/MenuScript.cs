using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {
    [Header("Objects")]
    [SerializeField] private GameObject mainMenuObject;
    [SerializeField] private GameObject creditsScreen;
    [SerializeField] private GameObject skipButton;
    [SerializeField] private bool rollCredits;


    [Header("Canvases")]
    [SerializeField] private GameObject settingsCanvas;
    [SerializeField] private GameObject howToPlayCanvas;

    [Header("Images")]
    [SerializeField] private Image mainMenu;
    [SerializeField] private Image settingsMenu;
    [SerializeField] private Image howToPlayMenu;

    //Used for lerping the main menu
    private IEnumerator UILerp(string lerpType, Image uiObject, Vector2 endPosition) {
        if (lerpType == "Open") {
            endPosition = new Vector2(0, -30);
        }
        else if (lerpType == "Close") {
            endPosition = new Vector2(0, 1000);
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
        gameObject.SetActive(false);
    }

    //Starts the game coroutine
    public void StartGame() {
        StartCoroutine(StartGameCoroutine());
    }

    //Used for smoothly transitioning between scenes
    private IEnumerator StartGameCoroutine() {
        StartCoroutine(UILerp("Close", mainMenu, Vector2.zero));
        yield return new WaitUntil(() => mainMenu.rectTransform.anchoredPosition.y >= 1000);
        SceneManager.LoadScene("Introduction");
    }

    //Called by the open settings button
    public void OpenSettings() {
        StartCoroutine(OpenSettingsCoroutine());
    }

    //Closes the main menu and opens the settings menu
    private IEnumerator OpenSettingsCoroutine() {
        StartCoroutine(UILerp("Close", mainMenu, Vector2.zero));
        yield return new WaitUntil(() => mainMenu.rectTransform.anchoredPosition.y >= 1000);
        mainMenu.gameObject.SetActive(false);
        settingsCanvas.SetActive(true);

        StartCoroutine(UILerp("Open", settingsMenu, Vector2.zero));
        EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        eventSystem.SetSelectedGameObject(GameObject.Find("CloseSettingsButton"));
    }

    //Called by the close settings button
    public void CloseSettings() {
        StartCoroutine(CloseSettingsCoroutine());
    }

    //Closes the settings menu and opens the main menu
    private IEnumerator CloseSettingsCoroutine() {
        StartCoroutine(UILerp("Close", settingsMenu, Vector2.zero));
        yield return new WaitUntil(() => settingsMenu.rectTransform.anchoredPosition.y >= 1000);     
        settingsCanvas.SetActive(false);
        mainMenu.gameObject.SetActive(true);

        StartCoroutine(UILerp("Open", mainMenu, Vector2.zero));
        EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        eventSystem.SetSelectedGameObject(GameObject.Find("PlayButton"));
    }

    //Called by the how to play button
    public void OpenHowToPlay() {
        StartCoroutine(OpenHowToPlayCoroutine());
    }

    //Closes the main menu and opens the HTP menu
    private IEnumerator OpenHowToPlayCoroutine() {
        StartCoroutine(UILerp("Close", mainMenu, Vector2.zero));
        yield return new WaitUntil(() => mainMenu.rectTransform.anchoredPosition.y >= 1000);
        mainMenu.gameObject.SetActive(false);
        howToPlayCanvas.SetActive(true);

        StartCoroutine(UILerp("Open", howToPlayMenu, Vector2.zero));
        EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        eventSystem.SetSelectedGameObject(GameObject.Find("CloseHTPButton"));
    }

    //Called by the close how to play button
    public void CloseHowToPlay() {
        StartCoroutine(CloseHowToPlayCoroutine());
    }

    //Closes the HTP menu and opens the main menu
    private IEnumerator CloseHowToPlayCoroutine() {
        StartCoroutine(UILerp("Close", howToPlayMenu, Vector2.zero));
        yield return new WaitUntil(() => howToPlayMenu.rectTransform.anchoredPosition.y >= 1000);
        howToPlayCanvas.SetActive(false);
        mainMenu.gameObject.SetActive(true);

        StartCoroutine(UILerp("Open", mainMenu, Vector2.zero));
        EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        eventSystem.SetSelectedGameObject(GameObject.Find("PlayButton"));
    }

    //Starts the credit coroutine
    public void Credits() {
        StartCoroutine(CreditsCoroutine());
    }

    //Used for displaying the credits
    private IEnumerator CreditsCoroutine() {
        StartCoroutine(UILerp("Close", mainMenu, Vector2.zero));
        yield return new WaitUntil(() => mainMenu.rectTransform.anchoredPosition.y >= 1000); //Wait until the main menu is off screen


        mainMenu.gameObject.SetActive(false);
        creditsScreen.SetActive(true);
        skipButton.SetActive(true);
        rollCredits = true; //Start the credits
        EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        eventSystem.SetSelectedGameObject(GameObject.Find("SkipButton"));

        yield return new WaitUntil(() => creditsScreen.GetComponent<RectTransform>().anchoredPosition.y >= 1050); //Wait until the credits are off the screen
        rollCredits = false; //Stop the credits
        creditsScreen.GetComponent<RectTransform>().anchoredPosition = new(0, -1050); //Resets the credits position
        creditsScreen.SetActive(false);
        skipButton.SetActive(false);

        mainMenu.gameObject.SetActive(true);
        StartCoroutine(UILerp("Open", mainMenu, Vector2.zero)); //Reopen the main menu
        eventSystem.SetSelectedGameObject(GameObject.Find("PlayButton"));
    }

    //Used to skip the credits
    public void SkipCredits() {
        skipButton.SetActive(false);
        creditsScreen.SetActive(false);
        creditsScreen.GetComponent<RectTransform>().anchoredPosition = new(0, 1040);
        mainMenu.gameObject.SetActive(true);

        StartCoroutine(UILerp("Open", mainMenu, Vector2.zero));
        EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        eventSystem.SetSelectedGameObject(GameObject.Find("PlayButton"));
    }

    //Used to set up the player prefs for the first time
    private void Awake() {
        PlayerPrefs.SetFloat("MusicVolume", 100f);
        PlayerPrefs.SetFloat("SoundsVolume", 100f);
    }

    //Used for moving the credits
    private void Update() {
        if(rollCredits == true & creditsScreen.GetComponent<RectTransform>().anchoredPosition.y < 1050) {
            creditsScreen.transform.Translate(100.0f * Time.deltaTime * Vector3.up); //Move the credits
        }
    }

    //Exits out the game
    public void ExitGame() {
        Application.Quit();
    }
}