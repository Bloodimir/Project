using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorialPanel;       
    public GameObject jumpTooltip;          
    public GameObject chillTooltip;
    public GameObject moneyTooltip;

    private GameManager gm;
        public Shop shop; 
    public bool enableTutorial = true;
    private AudioSource source;

    private bool firstPlay;

	void Start ()
    {
        firstPlay = PlayerPrefs.HasKey("played");
	}
	
	public void StartTutorial ()
    {
        if (!firstPlay && enableTutorial)
            StartCoroutine(ShowTutorial());
        else
            tutorialPanel.SetActive(false);
	}

    IEnumerator ShowTutorial()
    {
        yield return new WaitForSeconds(0.75F);     
        Time.timeScale = 0.0001F;              jumpTooltip.SetActive(true);     
    }

    public void ProceedTutorial()
    {
        jumpTooltip.SetActive(false); 
        chillTooltip.SetActive(true);  
    }

    public void BedavacıTutorial()
    {
        chillTooltip.SetActive(false);
        moneyTooltip.SetActive(true);
    }

    public void EndTutorial()
    {
        moneyTooltip.SetActive(false);
        tutorialPanel.SetActive(false);
                Time.timeScale = 1;
        PlayerPrefs.SetString("played", "");
    }
    }

