using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinedInput;

    public int createInputCharacterLimit;
    public int joinedInputCharacterLimit;

    public TextMeshProUGUI roomAlreadyExistsText;

    void Start(){
        createInput.characterLimit = createInputCharacterLimit;
        joinedInput.characterLimit = joinedInputCharacterLimit;
        roomAlreadyExistsText.alpha = 0;
    }

    public void CreateRoom(){
        
        PhotonNetwork.CreateRoom(createInput.text);
        if(SceneManager.GetActiveScene().name == "Lobby")
        StartCoroutine("SetAlphaOfText");
    }

    public void JoinRoom(){
        PhotonNetwork.JoinRoom(joinedInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("MultiplayerScene");
    }

    private IEnumerator SetAlphaOfText(){
        yield return new WaitForSeconds(1);
        roomAlreadyExistsText.alpha = 1;
        StartCoroutine("TurnOffText");
    }

    private IEnumerator TurnOffText(){
        yield return new WaitForSeconds(3);
        roomAlreadyExistsText.alpha = 0;
        
    }


    
}
