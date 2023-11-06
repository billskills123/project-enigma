using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightPlateScript : MonoBehaviour {
    [SerializeField] private GameManager gameManager;
    [SerializeField] private new Light2D light;
    [SerializeField] private char plateValue;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            gameObject.GetComponent<AudioSource>().Play();
            gameManager.LightPlate(plateValue, light);
        }
    }
}