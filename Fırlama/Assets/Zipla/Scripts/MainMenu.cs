using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class Music
{
    public UnityEngine.UI.Button musicButton;            
    public Sprite onIcon;                  
    public Sprite offIcon;                 
    public AudioSource ambientSource;      
}

[System.Serializable]
public class Sounds
{
    public UnityEngine.UI.Button soundsButton;           
    public Sprite onIcon;                  
    public Sprite offIcon;                  
}

public class MainMenu : MonoBehaviour 
{
    public UnityEngine.UI.Button playButton;         
    public UnityEngine.UI.Button exitButton;      
    public Text highscoreText;
    public Text deathText;
    public GameObject menuPanel;       
    public GameObject HUDPanel;         
    public AudioClip clickSFX;          
    public Music music;                
    public Sounds sounds;               

    private AudioSource source;
    private Image soundImage, musicImage;
    private Tutorial tutorial;           

	void Start () 
	{
        source = GetComponent<AudioSource>();
        soundImage = sounds.soundsButton.GetComponent<Image>();
        musicImage = music.musicButton.GetComponent<Image>();
        tutorial = GetComponent<Tutorial>();

        HUDPanel.SetActive(false);     
        menuPanel.SetActive(true);      

        playButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(() =>
        {
            StartCoroutine(ExitGame());
        });
        sounds.soundsButton.onClick.AddListener(ToggleSound);
        music.musicButton.onClick.AddListener(ToggleMusic);

	    LoadDeath();
        LoadHighscore();
        LoadAudioSettings();
	}

    void StartGame()
    {
        Game.isGameStarted = true;             
        Game.PlaySound(source, clickSFX);       
        HUDPanel.SetActive(true);               
        menuPanel.SetActive(false);            

        if (tutorial)
            tutorial.StartTutorial();
    }

    void LoadHighscore()
    {
        if (PlayerPrefs.HasKey("High"))
            highscoreText.text = "REKOR: " + PlayerPrefs.GetInt("High").ToString();
    }
    void LoadDeath()
    {
        if (PlayerPrefs.HasKey("Death"))
            deathText.text = "ÖLÜM: " + PlayerPrefs.GetInt("Death").ToString();
    }

    IEnumerator ExitGame()
    {
        Game.PlaySound(source, clickSFX);   
        SaveSettings();                   

        if (source.isPlaying)
            yield return null;

        Debug.Log("Çıkış");
        Application.Quit();
    }

    void ToggleSound()
    {
        Game.sounds = !Game.sounds;                                       
        soundImage.sprite = Game.sounds ? sounds.onIcon : sounds.offIcon;  ;
    }

    void ToggleMusic()
    {
        Game.music = !Game.music;                                          
        musicImage.sprite = Game.music ? music.onIcon : music.offIcon;      
        if (Game.music)
            music.ambientSource.Play();
        else
            music.ambientSource.Pause();
    }

    void LoadAudioSettings()
    {
        if (PlayerPrefs.HasKey("Sounds"))
            Game.sounds = Game.GetBool("Sounds");
        soundImage.sprite = Game.sounds ? sounds.onIcon : sounds.offIcon;

        if (PlayerPrefs.HasKey("Music"))
            Game.music = Game.GetBool("Music");
        musicImage.sprite = Game.music ? music.onIcon : music.offIcon;

        if (Game.music)
            music.ambientSource.Play();
        else
            music.ambientSource.Pause();
    }

    void SaveSettings()
    {
        Game.SetBool("Sounds", Game.sounds);
        Game.SetBool("Music", Game.music);
    }
}