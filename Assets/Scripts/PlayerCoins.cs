using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCoins : MonoBehaviour
{
    public int playerCoins = 0;

    [SerializeField] TextMeshProUGUI coinText;

    
    void Update(){
        //Displays the current player coins
        coinText.text = playerCoins.ToString();
    }  

    public void AddCoinsToPlayer(int coinAmount){
        playerCoins += coinAmount;
    }

    public void SubtractCoinsFromPlayer(int coinAmount){
        playerCoins -= coinAmount;
    }
}
