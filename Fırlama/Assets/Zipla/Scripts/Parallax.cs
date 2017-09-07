using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour 
{
    public Layers[] layers = new Layers[1];    

    private Transform camT;
    private Vector3 origin;
    private float camOffset;

	void Start () 
    {
        camT = Camera.main.transform;

        origin = camT.position;

        for (int i = 0; i < layers.Length; i++)
            layers[i].background.sortingOrder = layers[i].sortingOrder;
	}
	
	void Update () 
    {
        camOffset = (origin - camT.position).sqrMagnitude / 100;

        for (int i = 0; i < layers.Length; i++)
            layers[i].background.material.mainTextureOffset = new Vector2(camOffset * layers[i].scrollSpeed / 100, 0);
	}
}

[System.Serializable]
public class Layers
{
    public MeshRenderer background;         
    public float scrollSpeed = 10;         
    public int sortingOrder;          
}
