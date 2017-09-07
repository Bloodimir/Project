using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class ForceScale
{
    public Image filledImage;              
    public float growSpeed = 0.5F;        
    public Colors[] colors = new Colors[4]; 
}

[System.Serializable]
public class Colors
{
    public float scaleValue;              
    public Color color = Color.white;        
}

public class GameManager : MonoBehaviour 
{
    public ForceScale forceScale;      
    public bool enableAutoJump = true;

    private Player player;
    public Text kuvvet;
    public Text hız;
    private float time;
    private float power;
    private Color targetColor;
    private LevelGenerator levelGen;
    private GameObject text;
    private HUD hud;
    private PlayerCamera playerCam;
    private bool chill;

	void Start () 
	{

        levelGen = GetComponent<LevelGenerator>();
        hud = GetComponent<HUD>();
        playerCam = FindObjectOfType<PlayerCamera>();
        player = FindObjectOfType<Player>();

        targetColor = forceScale.colors[0].color;
	}
	
	void Update ()
	{

        if (!Game.isGameStarted)
            return;
        forceScale.filledImage.enabled = player.IsGrounded();       

        if (player.IsGrounded())
            time += forceScale.growSpeed * Time.deltaTime; 
        else
            time = 0;                                                                

        power = Mathf.PingPong(time, 1);
        forceScale.filledImage.fillAmount = power;
        kuvvet.text = "KUVVET: " + Mathf.Round(power/forceScale.growSpeed*100) + " N";

        if (power > 0.99F && !chill && enableAutoJump)
            player.Jump();

        if (power < forceScale.colors[0].scaleValue)
            targetColor = forceScale.colors[0].color;
        else if (power > forceScale.colors[0].scaleValue && power < forceScale.colors[1].scaleValue)
            targetColor = forceScale.colors[1].color;
        else if (power > forceScale.colors[1].scaleValue && power < forceScale.colors[2].scaleValue)
            targetColor = forceScale.colors[2].color;
        else
            targetColor = forceScale.colors[3].color;

        forceScale.filledImage.color = Color.Lerp(forceScale.filledImage.color, targetColor, 4.5F * Time.deltaTime);
	}

    public void AddCollectedCoins(int value)
    {
        hud.AddAirCoins(value);
    }



    public void AddCoin(int value, bool reward = false)
    {
        hud.AddCoin(value, reward);

    }

    public void AddScore()
    {
        hud.AddScore(1);
    }

    public void AddDeath()
    {
        hud.AddDeath(1);
    }

    public void ResetForceScale()
    {
        power = 0;  
        time = 0;  
    }

    public void Restart()
    {
        levelGen.PlaceBlocks();             
        ResetScore();                      
        playerCam.ResetCameraPosition();    
    }

    void ResetScore()
    {
        hud.ResetScore();
    }

    public float Power()
    {
        return power;
    }

    public void CheckBlock(Collider2D block)
    {
        chill = levelGen.GetBlockStatus(block);
    }

    public void ResetPlayer()
    {
        player.Restart();
    }
}