using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject MenuPanel;
    public void panelopen()
    {
        MenuPanel.SetActive(true);
    }
    public void panelclose()
    {
        MenuPanel.SetActive(false);
    }
    public void ChangeScene(string sceneName) // declares string so we can type in the name of the scene later on
    {
        SceneManager.LoadScene(sceneName); //changes scene to the string we typed
    }
}



