using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickeringLightScript : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] private new Light2D light;
    [SerializeField] private float flickerSpeed = 10f;
    [SerializeField] private bool flickerEnabled = true; //Could be turn off in the main settings

    //Enables the flickering
    private void Start() {
        light = GetComponent<Light2D>();
        StartCoroutine(LightFlicker());
    }

    //Makes the lights randomly flicker
    private IEnumerator LightFlicker() {
        while (flickerEnabled) {
            light.intensity = Random.Range(0.2f, 1.0f);
            yield return new WaitForSeconds(Random.Range(0.2f, 2.0f) / flickerSpeed);
        }
    }
}