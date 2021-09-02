using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject DeathCanvas;

    public GameObject PauseCanvas;

    public PlayerCoins playerCoins;

    void Start(){
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name,new Vector3(0,0,0),playerPrefab.transform.rotation);
        player.GetComponent<PlayerController>().DeathCanvas = DeathCanvas;
        player.GetComponent<PlayerController>()._pauseCanvas = PauseCanvas;
        player.GetComponent<PlayerController>().PlayerCoins = playerCoins;
    }
}
