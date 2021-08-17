using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinedInput;

    public void CreateRoom(){
        PhotonNetwork.CreateRoom(createInput.text);
    }

    public void JoinRoom(){
        PhotonNetwork.JoinRoom(joinedInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("MultiplayerScene");
    }

    
}
