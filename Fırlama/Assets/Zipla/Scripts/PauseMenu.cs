using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class Shop
{
    public SpriteRenderer playerSprite;                
    public Text shopCoinsText;                      
    public ButtonIcons unlockButtonIcons;             
    public AudioClip unlockSFX;                        
    public Characters[] characters = new Characters[7]; 
}

[System.Serializable]
public class Characters
{
    public string characterName = "İsim";               
    public Sprite characterSprite;                    
    public Sprite coinSprite;                          
    public int characterPrice = 100;                  
    public GameObject priceObject;                     
    public Text priceText;                           
    public UnityEngine.UI.Button unlockButton;                      
    public Image buttonIcon;                           
    public bool unlocked;
    public bool selected;
}

[System.Serializable]
public class ButtonIcons
{
    public Sprite unlocked;                            
    public Sprite selected;                            
}

[RequireComponent(typeof(GameManager))]
public class PauseMenu : MonoBehaviour 
{
    public UnityEngine.UI.Button pauseButton;
    public UnityEngine.UI.Button dailyButton;
    public UnityEngine.UI.Button continueShopButton;
    public UnityEngine.UI.Button continueDailyButton;
    public UnityEngine.UI.Button exitButton;                     

    public GameObject menuPanel;                 
    public GameObject HUDPanel;                    
    public GameObject pausePanel;
    public GameObject dailyPanel;


    public AudioClip clickSFX;                     
    public Shop shop;                              
    public SpriteRenderer[] coinsPoolSprites;      

    private HUD hud;
    private GameManager gm;
    private AudioSource source;
    public bool _çalıştı = false;
    public bool _çalıştı2 = false;
    public bool _çalıştı3 = false;
    public bool _çalıştı4 = false;
    public bool _çalıştı5 = false;
    public bool _çalıştı6 = false;
    public bool _çalıştı7 = false;
    public bool _çalıştı8 = false;
    public bool _çalıştı9 = false;
    public bool _çalıştı10 = false;
    public bool _çalıştı11 = false;
    public bool _çalıştı12 = false;
    public bool _çalıştı13 = false;
    public bool _çalıştı14 = false;
    public bool _çalıştı15 = false;
    public bool _çalıştı16 = false;
    public bool _çalıştı17 = false;
    public bool _çalıştı18 = false;
    public bool _çalıştı19 = false;
    public bool _çalıştı20 = false;
    public bool _çalıştı21 = false;
    public bool _çalıştı22 = false;
    public bool _çalıştı23 = false;
    public bool _çalıştı24 = false;

    void Start () 
	{
        source = GetComponent<AudioSource>();
        hud = GetComponent<HUD>();
        gm = GetComponent<GameManager>();
        

        pausePanel.SetActive(false);    
        SetUpListeners();                      
        Market();                     
	}
    private void Update()
    {
        Say();
    }
    void SetUpListeners()
    {
        pauseButton.onClick.AddListener(Durdur);
        dailyButton.onClick.AddListener(Günlük);     
        continueShopButton.onClick.AddListener(DevamShop);
        continueDailyButton.onClick.AddListener(DevamDaily);

        exitButton.onClick.AddListener(()=>            
        {
            StartCoroutine(Menuye());
        });

        for (int i = 0; i < shop.characters.Length; i++)
        {
            int index = i;
            shop.characters[index].unlockButton.onClick.AddListener(() =>
            {
                KarakterAl(index);
            });
        }
    }

    void Durdur()
    {
        Game.PlaySound(source, clickSFX);                        
        Time.timeScale = 0.00001F;                                 
        HUDPanel.SetActive(false);                            
        pausePanel.SetActive(true);                                 
        shop.shopCoinsText.text = hud.CoinsCount().ToString();    
    }

    void Günlük()
    {
        HUDPanel.SetActive(false);
        dailyPanel.SetActive(true);
        shop.shopCoinsText.text = hud.CoinsCount().ToString();

    }

    void DevamShop()
    {
        Game.PlaySound(source, clickSFX);                           
        KaydetKar();                                           
        pausePanel.SetActive(false);
        Time.timeScale = 1;                                         
        HUDPanel.SetActive(true);
    }

    void DevamDaily()
    {
        dailyPanel.SetActive(false);
        Time.timeScale = 1;
        HUDPanel.SetActive(true);
    }

    void Say()
    {
            if (Game.gamesCount == 14 && !_çalıştı)
            {
            _çalıştı = true;
                gm.AddCoin(25, true);
            Game.gamesCount++;

        }
        {
            if (Game.gamesCount == 29 && !_çalıştı2)
            {
                _çalıştı2 = true;
                gm.AddCoin(25, true);
                Game.gamesCount++;

            }
            {
                if (Game.gamesCount == 44 && !_çalıştı3)
                {
                    _çalıştı3 = true;
                    gm.AddCoin(25, true);
                    Game.gamesCount++;

                }
                {
                    if (Game.gamesCount == 59 && !_çalıştı4)
                    {
                        _çalıştı4 = true;
                        gm.AddCoin(25, true);
                        Game.gamesCount++;

                    }
                    {
                        if (Game.gamesCount ==74  && !_çalıştı5)
                        {
                            _çalıştı5 = true;
                            gm.AddCoin(25, true);
                            Game.gamesCount++;

                        }
                        {
                            if (Game.gamesCount == 89 && !_çalıştı6)
                            {
                                _çalıştı6 = true;
                                gm.AddCoin(25, true);
                                Game.gamesCount++;

                            }
                            {
                                if (Game.gamesCount == 104 && !_çalıştı7)
                                {
                                    _çalıştı7 = true;
                                    gm.AddCoin(25, true);
                                    Game.gamesCount++;

                                }
                                {
                                    if (Game.gamesCount ==119  && !_çalıştı8)
                                    {
                                        _çalıştı8 = true;
                                        gm.AddCoin(25, true);
                                        Game.gamesCount++;

                                    }
                                    {
                                        if (Game.gamesCount == 134 && !_çalıştı9)
                                        {
                                            _çalıştı9 = true;
                                            gm.AddCoin(25, true);
                                            Game.gamesCount++;

                                        }
                                        {
                                            if (Game.gamesCount == 159 && !_çalıştı10)
                                            {
                                                _çalıştı10 = true;
                                                gm.AddCoin(25, true);
                                                Game.gamesCount++;

                                            }
                                            {
                                                if (Game.gamesCount == 174 && !_çalıştı11)
                                                                                                    {
                                                    _çalıştı11 = true;
                                                    gm.AddCoin(25, true);
                                                    Game.gamesCount++;

                                                }
                                                {
                                                    if (Game.gamesCount == 199 && !_çalıştı12)
                                                    {
                                                        _çalıştı12 = true;
                                                        gm.AddCoin(25, true);
                                                        Game.gamesCount++;
                                                    }

                                                }
                                                {
                                                    if (Game.gamesCount == 214 && !_çalıştı13)
                                                    {
                                                        _çalıştı13 = true;
                                                        gm.AddCoin(25, true);
                                                        Game.gamesCount++;

                                                    }
                                                    {
                                                        if (Game.gamesCount == 229 && !_çalıştı14)
                                                        {
                                                            _çalıştı14 = true;
                                                            gm.AddCoin(25, true);
                                                            Game.gamesCount++;

                                                        }
                                                        {
                                                            if (Game.gamesCount == 244 && !_çalıştı5)
                                                            {
                                                                _çalıştı15 = true;
                                                                gm.AddCoin(25, true);
                                                                Game.gamesCount++;

                                                            }
                                                            {
                                                                if (Game.gamesCount == 259 && !_çalıştı16)
                                                                {
                                                                    _çalıştı16 = true;
                                                                    gm.AddCoin(25, true);
                                                                    Game.gamesCount++;

                                                                }
                                                                {
                                                                    if (Game.gamesCount == 274 && !_çalıştı17)
                                                                    {
                                                                        _çalıştı17 = true;
                                                                        gm.AddCoin(25, true);
                                                                        Game.gamesCount++;

                                                                    }
                                                                    {
                                                                        if (Game.gamesCount == 299 && !_çalıştı18)
                                                                        {
                                                                            _çalıştı8 = true;
                                                                            gm.AddCoin(25, true);
                                                                            Game.gamesCount++;

                                                                        }
                                                                        {
                                                                            if (Game.gamesCount == 314 && !_çalıştı9)
                                                                            {
                                                                                _çalıştı9 = true;
                                                                                gm.AddCoin(25, true);
                                                                                Game.gamesCount++;

                                                                            }
                                                                            {
                                                                                if (Game.gamesCount == 329 && !_çalıştı20)
                                                                                {
                                                                                    _çalıştı20 = true;
                                                                                    gm.AddCoin(25, true);
                                                                                    Game.gamesCount++;

                                                                                }
                                                                                {
                                                                                    if (Game.gamesCount == 344 && !_çalıştı21)
                                                                                    {
                                                                                        _çalıştı21 = true;
                                                                                        gm.AddCoin(25, true);
                                                                                        Game.gamesCount++;

                                                                                    }
                                                                                    {
                                                                                        if (Game.gamesCount == 359 && !_çalıştı22)
                                                                                        {
                                                                                            _çalıştı22 = true;
                                                                                            gm.AddCoin(25, true);
                                                                                            Game.gamesCount++;

                                                                                        }
                                                                                        {
                                                                                            if (Game.gamesCount == 374 && !_çalıştı23)
                                                                                            {
                                                                                                _çalıştı23 = true;
                                                                                                gm.AddCoin(25, true);
                                                                                                Game.gamesCount++;

                                                                                            }
                                                                                            {
                                                                                                if (Game.gamesCount == 399 && !_çalıştı24)
                                                                                                {
                                                                                                    _çalıştı24 = true;
                                                                                                    gm.AddCoin(25, true);
                                                                                                    Game.gamesCount++;

                                                                                        }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }



    void KarakterAl(int index)
    {
        if (!shop.characters[index].unlocked && hud.CoinsCount() >= shop.characters[index].characterPrice)
        {
            shop.characters[index].unlocked = true;                                      
            Game.PlaySound(source, shop.unlockSFX);                                       
            shop.characters[index].buttonIcon.sprite = shop.unlockButtonIcons.unlocked;    
            shop.characters[index].priceObject.SetActive(false);                           
            hud.DecreaseCoinsCount(shop.characters[index].characterPrice);                 
            shop.shopCoinsText.text = hud.CoinsCount().ToString();                       
        }
        else if (shop.characters[index].unlocked && !shop.characters[index].selected)
        {
            shop.characters[index].selected = true;                                                                
            Game.PlaySound(source, clickSFX);                                               
            shop.characters[index].buttonIcon.sprite = shop.unlockButtonIcons.selected;   
            shop.characters[index].unlockButton.interactable = false;                      
            shop.playerSprite.sprite = shop.characters[index].characterSprite;             
            SetCoinSprite(shop.characters[index].coinSprite);                             

            for (int i = 0; i < shop.characters.Length; i++)
            {
                if (i != index && shop.characters[i].selected)
                {
                    shop.characters[i].selected = false;                                    
                    shop.characters[i].unlockButton.interactable = true;                   
                    shop.characters[i].buttonIcon.sprite = shop.unlockButtonIcons.unlocked;
                }
            }
        }
    }

    void KaydetKar()
    {
        for (int i = 0; i < shop.characters.Length; i++)
        {
            Game.SetBool("Unlocked" + shop.characters[i].characterName, shop.characters[i].unlocked);
            Game.SetBool("Selected" + shop.characters[i].characterName, shop.characters[i].selected);
        }
    }

    void Market()
    {
        for (int i = 0; i < shop.characters.Length; i++)
        {
            if (PlayerPrefs.HasKey("Unlocked" + shop.characters[i].characterName))
                shop.characters[i].unlocked = Game.GetBool("Unlocked" + shop.characters[i].characterName);
            if (PlayerPrefs.HasKey("Selected" + shop.characters[i].characterName))
                shop.characters[i].selected = Game.GetBool("Selected" + shop.characters[i].characterName);
        }

        BakkalHesabı();
    }

    void BakkalHesabı()
    {
        for (int i = 0; i < shop.characters.Length; i++)
        {
            shop.characters[i].priceText.text = shop.characters[i].characterPrice.ToString();
            if (shop.characters[i].unlocked)
            {
                shop.characters[i].priceObject.SetActive(false);
                shop.characters[i].buttonIcon.sprite = shop.unlockButtonIcons.unlocked;
            }
            if (shop.characters[i].selected)
            {
                shop.characters[i].unlockButton.interactable = false;                   
                shop.characters[i].buttonIcon.sprite = shop.unlockButtonIcons.selected; 
                shop.playerSprite.sprite = shop.characters[i].characterSprite;        
                SetCoinSprite(shop.characters[i].coinSprite);                           
                shop.characters[i].unlockButton.interactable = false;                  
            }
        }
    }

    


    void SetCoinSprite(Sprite sprite)
    {
        for (int i = 0; i < coinsPoolSprites.Length; i++)
        {
            coinsPoolSprites[i].sprite = sprite;
        }
    }

    IEnumerator Menuye()
    {
        Game.PlaySound(source, clickSFX);

        if (source.isPlaying)
            yield return null;

        pausePanel.SetActive(false);    
        menuPanel.SetActive(true);      
        Game.isGameStarted = false;     
        gm.ResetPlayer();             
        gm.Restart();                  
        Time.timeScale = 1;             
    }
}