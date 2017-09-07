using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class SoundEffects
{
    public AudioClip jumpSFX;          
    public AudioClip landSFX;          
    public AudioClip deathSFX;         
    public AudioClip collectSFX;        
}

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour 
{
    public SpriteRenderer graphics;                        
    public float gravityScale = 2.65F;                     
    public Vector3 jumpForce = new Vector3(75, 95, 0);    
    public float respawnDelay = 0.5F;                     
    public SoundEffects soundEffects;                      

    private bool isGrounded;
    public int death;
    private Rigidbody2D rb2d;
    private GameManager gm;
    private Vector3 spawnPosition;
    private Transform thisT;
    public ScoreHUD x;
    private Collider2D curBlock;
    private AudioSource[] sources = new AudioSource[2];

    void Awake()
    {
        gameObject.tag = "Player";
    }

	void Start () 
	{
        thisT = transform;
        rb2d = GetComponent<Rigidbody2D>();
        gm = FindObjectOfType<GameManager>();
        for (int i = 0; i < sources.Length; i++)
            sources[i] = gameObject.AddComponent<AudioSource>();

        rb2d.isKinematic = true;
        graphics.enabled = false;

        rb2d.gravityScale = gravityScale;
        spawnPosition = thisT.position;
        StartCoroutine(WaitForStart());
        death = PlayerPrefs.GetInt("Death");
        Game.gamesCount = death;

    }

    IEnumerator WaitForStart()
    {
        while (!Game.isGameStarted)
            yield return null;
        rb2d.isKinematic = false;
        graphics.enabled = true;
    }

    public void Jump()
    {
        if (!IsGrounded())
            return;
        rb2d.AddForce(jumpForce * gm.Power() * 10);                   
        isGrounded = false;                                    
        curBlock.enabled = false;                              
        Game.PlaySound(sources[0], soundEffects.jumpSFX, 0.5F);
    }

    public bool IsGrounded()
    {
        return isGrounded && rb2d.velocity.magnitude <= 0.01F && rb2d.angularVelocity < 0.05F &&
            curBlock != null && thisT.position.y > curBlock.bounds.max.y;
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("DeathZone"))
        {
            Game.PlaySound(sources[0], soundEffects.deathSFX, 0.35F, 0.8F);     
            StartCoroutine(Respawn());                                         
            rb2d.velocity = Vector3.zero;                                
                                                  
            curBlock = null;                                                    
            Game.gamesCount++;
            {
                PlayerPrefs.SetInt("Death", Game.gamesCount);
                gm.AddDeath();
            }

        }
        else if (col.CompareTag("Coin"))
        {
                gm.AddCoin(2);     

            Game.PlaySound(sources[1], soundEffects.collectSFX);  
            col.gameObject.SetActive(false);
        }
           else
        {
            gm.AddScore();
            }

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        rb2d.freezeRotation = false;                            
        isGrounded = true;                                      

        if (curBlock != col.collider)
        {
            Game.PlaySound(sources[0], soundEffects.landSFX); 
            gm.ResetForceScale();                           
            curBlock = col.collider;
            gm.CheckBlock(curBlock);                           
        }
    }

    void OnCollisionStay2D()
    {
        isGrounded = true;
    }

    void OnTriggerExit2D()
    {
        isGrounded = false;
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);  
        rb2d.velocity = Vector3.zero;                 
        thisT.eulerAngles = Vector3.zero;              
        thisT.position = spawnPosition;            
        rb2d.freezeRotation = true;                   
        gm.Restart();                                  
        gm.ResetForceScale();                       
    }

    public void Restart()
    {
        rb2d.velocity = Vector3.zero;          
        thisT.eulerAngles = Vector3.zero;      
        thisT.position = spawnPosition;       
        rb2d.freezeRotation = true;          
        rb2d.isKinematic = true;              
        graphics.enabled = false;             
        gm.Restart();                         
        gm.ResetForceScale();                 
        StartCoroutine(WaitForStart());
    }
}
