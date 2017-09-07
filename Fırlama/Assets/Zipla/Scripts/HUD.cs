using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class ScoreHUD
{
    public Text scoreText;          
    public Text highscoreText;    
    public Text coinsText;
    public Text deathText;
}

public class HUD : MonoBehaviour
{
    public ScoreHUD scoreHUD;

    private int death ;
    private int score;
    private int highscore;
    private int coinsCount;
    private int deathcount;
    private int coins;


    void Start()
    {
        LoadHighscore();
        LoadCoins();

    }

    public void AddScore(int value)
    {
        score += value;

        scoreHUD.scoreText.text = score.ToString();
    }

    public void AddDeath(int value)
    {
        scoreHUD.deathText.text = "ÖLÜM: " + PlayerPrefs.GetInt("Death");
    }

    public void AddCoin(int value, bool reward = false)
    {
        coinsCount += value;
        scoreHUD.coinsText.text = coinsCount.ToString();
        if (reward)
            SaveCoins();
    }

    public void CounttoReward()
    {
       
    }

    public void AddAirCoins(int value)
    {
        StartCoroutine(AddCoinsWithDelay(value));
    }

    IEnumerator AddCoinsWithDelay(int value)
    {
        coins = coinsCount + value;
        while (coinsCount < coins)
        {
            coinsCount += 2;
            scoreHUD.coinsText.text = coinsCount.ToString();
            yield return new WaitForSeconds(0.1F);
        }
    }

    public void ResetScore()
    {

        SaveCoins();
        SaveHighscore();
        CheckHighScore();
        LoadCoins();
        score = 0;

        scoreHUD.scoreText.text = score.ToString();
    }

    void LoadHighscore()
    {
        if (PlayerPrefs.HasKey("High"))
            highscore = PlayerPrefs.GetInt("High");

        scoreHUD.highscoreText.text = "REKOR: " + highscore;
    }

    void LoadCoins()
    {
        if (PlayerPrefs.HasKey("Coins"))
            coinsCount = PlayerPrefs.GetInt("Coins");
        scoreHUD.coinsText.text = coinsCount.ToString();
    }




    void CheckHighScore()
    {
        if (score > highscore)
            highscore = score;
        scoreHUD.highscoreText.text = "REKOR: " + highscore;
    }


    void SaveHighscore()
    {
        PlayerPrefs.SetInt("High", highscore);
    }

    void SaveCoins()
    {
        PlayerPrefs.SetInt("Coins", coinsCount);
    }

    public int CoinsCount()
    {
        return coinsCount ;
    }

    public void DecreaseCoinsCount(int value)
    {
        coinsCount -= value;
        scoreHUD.coinsText.text = coinsCount.ToString();
    }
}