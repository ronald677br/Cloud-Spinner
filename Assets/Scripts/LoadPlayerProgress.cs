using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;

//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
using System.IO;


public class LoadPlayerProgress : MonoBehaviour
{
    public string zoneId = "1123911";
    public SaveLoad saveReference;
    private Scrollbar sensibilityBar;
    public Image[] LockList16_9,LockList3_2, LockList5_3, LockList4_3;
    public Button[] LevelList;
    public Color SetAlpha;
    private int playerProgress;
    private Text scrollText;
    public float sens;
    private string zoneID = "vz4d0924bac4064d648a";
    public MenuManager menuReference;

    
    void Awake()
    {
        sensibilityBar = GameObject.Find("Scrollbar").GetComponent<Scrollbar>();
        menuReference = GameObject.Find("Canvas").GetComponent<MenuManager>();
        saveReference = GameObject.Find("SaveManager").GetComponent<SaveLoad>();
        scrollText = GameObject.Find("Scroll_text").GetComponent<Text>();
        saveReference.LoadPProgress();
        saveReference.LoadPlayerSettings();
        if (File.Exists(Application.persistentDataPath + "/savePProgress.bw"))
        {
            sens = saveReference.sensibility;
            sensibilityBar.value = (sens / 40);

            playerProgress = saveReference.loadedLevel -3;
        }
        else { playerProgress = 0; saveReference.sensibility = 1; sens = saveReference.sensibility; }
    }

    void Start()
    {
    /*   
      PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) => {
          
        });
      */ 
            for (int x = playerProgress +1; x < LevelList.Length; x++)
            {
                LevelList[x].interactable = false;
                
            }

            if (menuReference.l16_9)
            {
                for (int y = (playerProgress); y >= 0; y--)
                {

                    LockList16_9[y].color = SetAlpha;
                }
            }
            else if (menuReference.l3_2)
            {
                for (int y = (playerProgress); y >= 0; y--)
                {

                    LockList3_2[y].color = SetAlpha;
                }
            }
            else if (menuReference.l5_3)
            {
                for (int y = (playerProgress); y >= 0; y--)
                {

                    LockList5_3[y].color = SetAlpha;
                }
            }
            else if (menuReference.l4_3)
            {
                for (int y = (playerProgress); y >= 0; y--)
                {

                    LockList4_3[y].color = SetAlpha;
                }
            }

    }
    void Update()
    {
        
        sens = sensibilityBar.value;
        scrollText.text = saveReference.sensibility.ToString("0.0");
        saveReference.sensibility = sens * 40;
    }

    public void DevLifes()
    {
        saveReference.playerLifes += 100;
        saveReference.SavePlayerLifes();

    }

  /*  public void showUnityAds()
    {
        if (string.IsNullOrEmpty(zoneId)) zoneId = null;
        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;
        if (Advertisement.IsReady(zoneId))
        {
            Advertisement.Show(zoneId, options);
        }
    }
    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("Video completed. User rewarded " + 5 + " lifes.");
                saveReference.playerLifes += 5;
                saveReference.SavePlayerLifes();
                break;
            case ShowResult.Skipped:
                Debug.LogWarning("Video was skipped.");
                break;
            case ShowResult.Failed:
                Debug.LogError("Video failed to show.");
                break;
        }
    }*/   
}



