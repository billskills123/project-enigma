using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnScript : MonoBehaviour {
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private int playersToSpawn;

    public void OnStart() {
        for (int i = 0; i < playersToSpawn; i++) {
            GameObject newPlayer = Instantiate(playerPrefab, new Vector3(3, 4, 0), Quaternion.identity);

            if(i == 0) {
                newPlayer.GetComponent<PlayerScript>().characterName = "Richard";
            } else {
                newPlayer.GetComponent<PlayerScript>().characterName = "Raymond";
            }
        }
    }
}