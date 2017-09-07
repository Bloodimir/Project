using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CoinSpawner : MonoBehaviour 
{
    public BoxCollider2D spawnArea;    
    public float spawnDelay;        
    public Transform[] coinsPool;   

    private float time;

    void Update()
    {
        for (int i = 0; i < coinsPool.Length; i++)
        {
            if (time < Time.time)
            {
                if (!coinsPool[i].gameObject.activeSelf)
                {
                    coinsPool[i].position = new Vector3(Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x), spawnArea.bounds.center.y, 0);
                    coinsPool[i].gameObject.SetActive(true);
                    time = Time.time + spawnDelay;
                    break;
                }
            }
        }
    }
}