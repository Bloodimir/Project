using UnityEngine;
using System.Collections;

public class Donate : MonoBehaviour
{
    public int Coins = 25;                  
    private int currentCoins;          

    public void DonateCoins()
    {
        currentCoins = PlayerPrefs.GetInt("Coins");    
        currentCoins += Coins;                        
        PlayerPrefs.SetInt("Coins", currentCoins);     
    }
}