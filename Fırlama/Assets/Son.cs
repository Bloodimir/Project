using UnityEngine;
using System.Collections;

public class Son : MonoBehaviour
{
    public GameObject tutorialPanel;
    public GameObject jumpTooltip;
    public GameObject chillTooltip;
    public bool enableTutorial = true;

    void Start()
    {
    }

    public void StartTutorial()
    {
        if (enableTutorial)
            StartCoroutine(ShowTutorial());
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

    public void EndTutorial()
    {
        chillTooltip.SetActive(false);
        tutorialPanel.SetActive(false);
                Time.timeScale = 1;
    }
}
