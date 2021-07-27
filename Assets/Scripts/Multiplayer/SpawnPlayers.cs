using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;

    void Start(){
        PhotonNetwork.Instantiate(playerPrefab.name,new Vector3(0,0,0),playerPrefab.transform.rotation);
    }
}
