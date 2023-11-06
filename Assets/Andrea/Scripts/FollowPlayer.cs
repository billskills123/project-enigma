using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private GameObject Player;
    private Camera MainCamera;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        MainCamera = Camera.main;

    }
    private void Update()
    {
        var screenPos = MainCamera.WorldToScreenPoint(Player.transform.position);
        transform.position = screenPos;
    }
}
