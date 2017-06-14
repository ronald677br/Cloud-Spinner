using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.SocialPlatforms;


public class MenuManager : MonoBehaviour
{
    public Menu CurrentMenu;
    private int menuCheck;
    public Canvas canvas;
    public SaveLoad saveReference;
    private AudioSource music;
    private int hasPlayed;
    public bool tutorialCompleted;
    public Toggle MusicAssign;
    private float ScreenX, ScreenY;
    public GameObject Lock16_9, Lock3_2,Lock4_3,Lock5_3, Lock16_9_2, Lock3_2_2, Lock4_3_2, Lock5_3_2;
    public bool l16_9, l5_3, l4_3, l3_2;


  
    void Awake()
    {
        
        l16_9 = false;
        l5_3 = false;
        l4_3 = false;
        l3_2 = false;
        ScreenX = Screen.width;
        ScreenY = Screen.height;
      
        if ((ScreenX / ScreenY) < 0.76f && (ScreenX / ScreenY) > 0.74f)//4:3
        {
            Destroy(Lock16_9); Destroy(Lock3_2); Destroy(Lock5_3);
            Destroy(Lock5_3_2); Destroy(Lock16_9_2); Destroy(Lock3_2_2);
            l16_9 = false;
            l5_3 = false;
            l4_3 = true;
            l3_2 = false;
        }
        else if ((ScreenX / ScreenY) < 0.6f && 0.55f < (ScreenX / ScreenY)) //16:9
        {
            Destroy(Lock5_3); Destroy(Lock4_3); Destroy(Lock3_2);
            Destroy(Lock5_3_2); Destroy(Lock4_3_2); Destroy(Lock3_2_2);
            l16_9 = true;
            l5_3 = false;
            l4_3 = false;
            l3_2 = false;
        }
        else if ((ScreenX / ScreenY) < 0.7f && (ScreenX / ScreenY) > 0.5f) //3:2
        {
            Destroy(Lock16_9); Destroy(Lock4_3); Destroy(Lock5_3);
            Destroy(Lock16_9_2); Destroy(Lock4_3_2); Destroy(Lock5_3_2);
            l3_2 = true; l16_9 = false;
            l5_3 = false;
            l4_3 = false;
        }
        else if ((ScreenX / ScreenY) < 0.62f && (ScreenX / ScreenY) > 0.5f) // 5:3
        {
            Destroy(Lock16_9); Destroy(Lock4_3); Destroy(Lock3_2);
            Destroy(Lock16_9_2); Destroy(Lock4_3_2); Destroy(Lock3_2_2);
            l5_3 = true; l16_9 = false;            
            l4_3 = false;
            l3_2 = false;
        }
    }


    public void Start()
    {

        
        hasPlayed = PlayerPrefs.GetInt("HasPlayed");
        if (hasPlayed == 0)
        {
            tutorialCompleted = false;
        }
        else { tutorialCompleted = true; }
            music = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
            saveReference = GameObject.Find("SaveManager").GetComponent<SaveLoad>();
        if (saveReference.playerLifes <= 0 && SceneManager.GetActiveScene().buildIndex == 0) {
            CurrentMenu = GameObject.Find("Message").GetComponent<Menu>();
            ShowMenu(CurrentMenu);
        }
        if(SceneManager.GetActiveScene().buildIndex == 2) {
            CurrentMenu = GameObject.Find("TutorialUI_01").GetComponent<Menu>();
            ShowMenu(CurrentMenu);
            Time.timeScale = 0;
        }
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            CurrentMenu = GameObject.Find("TutorialStart").GetComponent<Menu>();
            ShowMenu(CurrentMenu);
            Time.timeScale = 0;
        }
        else { ShowMenu(CurrentMenu); }
        menuCheck = SceneManager.GetActiveScene().buildIndex;
        saveReference.LoadPlayerSettings();
        if (File.Exists(Application.persistentDataPath + "/savePSettings.bw"))
        {            
            GameObject.FindGameObjectWithTag("MusicToggle").GetComponent<Toggle>().isOn = saveReference.soundMusicStatus;
            
            if (menuCheck != 0)
            {
                GameObject.Find("SFX").GetComponent<Toggle>().isOn = saveReference.soundSFXStatus;
            }
            else { GameObject.FindGameObjectWithTag("MusicToggle").GetComponent<Toggle>().isOn = false;
                if (menuCheck != 0)
                {
                    GameObject.Find("SFX").GetComponent<Toggle>().isOn = saveReference.soundSFXStatus;
                }
            }
        }
        saveReference.LoadPlayerLifes();
    }
    void OnApplicationQuit()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0 || SceneManager.GetActiveScene().buildIndex != 1 || SceneManager.GetActiveScene().buildIndex != 2 || SceneManager.GetActiveScene().buildIndex != 3) SubtractBall();
    }
    void Update()
    {
        
        foreach(GameObject ballsLeft in GameObject.FindGameObjectsWithTag("BallsLeft"))
        {
            ballsLeft.GetComponent<Text>().text = saveReference.playerLifes.ToString();
        }

        saveReference.soundMusicStatus = GameObject.FindGameObjectWithTag("MusicToggle").GetComponent<Toggle>().isOn;
        if (menuCheck != 0)
        {
            saveReference.soundSFXStatus = GameObject.Find("SFX").GetComponent<Toggle>().isOn;            
        }
        if (saveReference.soundMusicStatus)
        {
            music.mute = true;
        }
        if(saveReference.soundMusicStatus == false) { music.mute = false; }
        if (saveReference.soundSFXStatus)
        {
            foreach (GameObject SFX in GameObject.FindGameObjectsWithTag("SFX"))
            {
                SFX.GetComponent<AudioSource>().mute = true;
            }
        }
        if(saveReference.soundSFXStatus == false) {
            foreach (GameObject SFX in GameObject.FindGameObjectsWithTag("SFX"))
            {
                SFX.GetComponent<AudioSource>().mute = false;
            }
        }
    }

    public void ShowMenu(Menu menu)
    {
        if (CurrentMenu != null)
        CurrentMenu.IsOpen = false;
        CurrentMenu = menu;
        CurrentMenu.IsOpen = true;
    }
    public void ShowStopMenu (Menu menu)
    {       
        if (CurrentMenu != null)
        CurrentMenu.IsOpen = false;
        CurrentMenu = menu;
        CurrentMenu.IsOpen = true;
        Time.timeScale = 0;
    }
    public void HideStopMenu (Menu menu)
    {
        if (CurrentMenu != null)
        CurrentMenu.IsOpen = false;
        CurrentMenu = menu;
        CurrentMenu.IsOpen = true;
        Time.timeScale = 1;
    }
    public void ExitAndApply()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            PlayerPrefs.SetInt("HasPlayed", 1);
            tutorialCompleted = true;
            saveReference.SavePlayerSettings();
        }
        else
        {
            saveReference.SavePlayerSettings();
        }
    }

    public void SubtractBall()
    {
        if (saveReference.playerLifes > 0)
        {
            saveReference.playerLifes--;
            saveReference.SavePlayerLifes();
        }       
    }
    public void LoadLevel(int levelToLoad)
    {
       if (saveReference.playerLifes <= 0)
        {
            LoadingScreenManager.LoadScene(0);
        }
       else if(tutorialCompleted == false && SceneManager.GetActiveScene().buildIndex != 2) { LoadingScreenManager.LoadScene(2); }        
        else { LoadingScreenManager.LoadScene(levelToLoad); }
    }
    public void BuyInfiniteLifes()
    {
        IAPManager.Instance.BuyNonConsumable();
    }
    public void Buy100Lifes()
    {
        IAPManager.Instance.BuyConsumable();
    }
    public void reloadSoundStatus(bool sfx, bool music) {




    }
  
}

