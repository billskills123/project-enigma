using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartOpening : MonoBehaviour
{
    public StoryScript hintScript;
    void Start()
    {

        hintScript.StartHintBox();
    }
}
