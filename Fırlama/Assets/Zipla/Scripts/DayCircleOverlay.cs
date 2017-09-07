using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class DayCircle
{
    public string timeName;                        
    public Color lightColor = Color.white;        
    public float transitionSpeed = 1;               
    public float duration = 10;                    
}

public class DayCircleOverlay : MonoBehaviour 
{
    public SpriteRenderer overlaySprite;                
    public DayCircle[] dayCircle = new DayCircle[4];    

    private int lightIndex;
    private float time;

	void Start () 
	{
        time = dayCircle[0].duration;
	}
	
	void Update ()
	{
        if (Time.time > time)
        {
            if (lightIndex < dayCircle.Length - 1)
            {
                lightIndex++;
                time = Time.time + dayCircle[lightIndex].duration;
            }
            else
            {
                lightIndex = 0;
                time = Time.time + dayCircle[lightIndex].duration;
            }
        }

        overlaySprite.color = Color.Lerp(overlaySprite.color, dayCircle[lightIndex].lightColor, 
                                                  dayCircle[lightIndex].transitionSpeed / 10 * Time.deltaTime);
	}
}