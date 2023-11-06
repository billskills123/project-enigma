using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;


public class StoryScript : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    private int index;
    private bool nextLine;
    public float playTime = 0.5f;
    public float pauseTime = 0.5f;
    public AudioSource typewriter;


    public void StartHintBox()
    {
        GetComponent<AudioSource>().time = GetComponent<AudioSource>().clip.length * .8f;
        typewriter = GetComponent<AudioSource>();
        StartCoroutine(PlayPauseCoroutine());
        textComponent.text = string.Empty;
        StartDialogue(0);
    }
   

    IEnumerator PlayPauseCoroutine()
    {
        while (true)
        {
            typewriter.Play();
            yield return new WaitForSeconds(90f);
            LoadMainGame();
            //typewriter.Stop();
            //yield return new WaitForSeconds(100f);
        }
    }

    public void LoadMainGame() {
        SceneManager.LoadScene("MainGame");
    }

    void Update() {
        if (nextLine == true) {
            nextLine = false;
            if (textComponent.text == lines[index])
            {
                NextLine(); 
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue(int startPoint)
    {
        index = startPoint;
        StartCoroutine(TypeLine());
    }
    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
    void NextLine()
    {
        if (index < lines.Length - 5)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void OnNextLine() {
        nextLine = true;
    }
}