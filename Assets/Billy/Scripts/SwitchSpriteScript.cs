using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSpriteScript : MonoBehaviour {
    [Header("Sprite Settings")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;
    public int currentSprite;

    public void ChangeSprite(int spriteNumber) {
        spriteRenderer.sprite = sprites[spriteNumber];
        currentSprite = spriteNumber;
    }
}