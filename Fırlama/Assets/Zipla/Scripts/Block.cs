using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Block : MonoBehaviour 
{
    public bool chillBlock;             
    public GameObject chillIcon;        

    IEnumerator Start()
    {
        
        while (!Game.isGameStarted)
            yield return null;

        
        SetChillBlock(chillBlock);
    }

    
    public void SetChillBlock(bool isChill)
    {
        
        chillBlock = isChill;

        
        if (chillIcon)
            chillIcon.SetActive(isChill);
    }
}