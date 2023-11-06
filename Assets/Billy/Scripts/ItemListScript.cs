using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemListScript : MonoBehaviour {
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private GameObject crowbarText;
    [SerializeField] private GameObject ropeText;
    [SerializeField] private GameObject keyText;
    [SerializeField] private GameObject gagText;
    [SerializeField] private GameObject handcuffText;
    [SerializeField] private GameObject knifeText;

    private void OnEnable() {
        //Function for showing the crowbar in the inventory
        if(playerScript.hasCrowbar == true) {
            crowbarText.SetActive(true);
        }
        else if(playerScript.hasCrowbar == false) {
            crowbarText.SetActive(false);
        }

        //Function for showing the rope in the inventory
        if (playerScript.hasRope == true) {
            ropeText.SetActive(true);
        }
        else if (playerScript.hasRope == false) {
            ropeText.SetActive(false);
        }

        //Function for showing the key in the inventory
        if (playerScript.hasKey == true) {
            keyText.SetActive(true);
        } else if (playerScript.hasKey == false) {
            keyText.SetActive(false);
        }

        //Function for showing the gag in the inventory
        if (playerScript.hasGag == true) {
            gagText.SetActive(true);
        } else if (playerScript.hasGag == false) {
            gagText.SetActive(false);
        }

        //Function for showing the handcuffs in the inventory
        if (playerScript.hasHandcuffs == true) {
            handcuffText.SetActive(true);
        } else if (playerScript.hasHandcuffs == false) {
            handcuffText.SetActive(false);
        }

        //Function for showing the knife in the inventory
        if (playerScript.hasKnife == true) {
            knifeText.SetActive(true);
        } else if (playerScript.hasKnife == false) {
            knifeText.SetActive(false);
        }
    }
} 