using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookcaseScript : MonoBehaviour {
    public int totalStrength;
    public Rigidbody2D BookcaseRigidbody;
    private bool soundEnabled;

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.collider.CompareTag("Player") && totalStrength != 2) {
            soundEnabled = false;
            BookcaseRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            BookcaseRigidbody.velocity = new(0f, 0f);
        }
        else if (transform.position.x > -0.3f && totalStrength == 2) {
            soundEnabled = true;
            BookcaseRigidbody.velocity = new(-0.2f, 0f);
            BookcaseRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void PlaySound() {
        if(soundEnabled == true) {
            gameObject.GetComponent<AudioSource>().UnPause();
        }
    }

    private void StopSound() {
        if (soundEnabled == false) {
            gameObject.GetComponent<AudioSource>().Pause();
        }
    }

    private void Update() {
        if(BookcaseRigidbody.velocity == new Vector2(-0.2f, 0f)) {
            PlaySound();
        }
        else {
            StopSound();
        }
    }
}
