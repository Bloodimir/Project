using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Sahne : MonoBehaviour
{


    public void LoadScene(int level)
    { 
        SceneManager.LoadScene(level);
    }
}