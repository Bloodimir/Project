using UnityEngine;

public class Yokedici : MonoBehaviour
{
    private void Start()
    {
        Destroy(GameObject.Find("GameManagers"));
        Destroy(GameObject.Find("New Game Object"));
    }
}