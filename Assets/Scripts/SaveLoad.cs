using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
public class SaveLoad : MonoBehaviour {
    public int playerLevel, currLevel, loadedLevel, playerLifes;
    public float sensibility;
    public bool soundMusicStatus = true;
    public bool soundSFXStatus = true;    
    public bool tutorialCompleted;
    public MenuManager menuReference;
    public DateTime timeToGetLifes;
    public int getLevel;
    
    
    
    public void Start()
    {
        getLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        menuReference = GameObject.Find("Canvas").GetComponent<MenuManager>();
        
    }
    public void SavePlayerLifes()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream savedLifes = File.Create(Application.persistentDataPath + "/savePLifes.bw");
        SaveManager saver = new SaveManager();
        //all stuff to save
        saver.playerLifes = playerLifes;
        saver.timeToGetLifes = timeToGetLifes;

        //
        bf.Serialize(savedLifes, saver);
        savedLifes.Close();


    }
    public void LoadPlayerLifes()
    {
        if (File.Exists(Application.persistentDataPath + "/savePLifes.bw"))
        {           
            BinaryFormatter bf = new BinaryFormatter();
            FileStream savedLifes = File.Open(Application.persistentDataPath + "/savePLifes.bw", FileMode.Open);
            SaveManager saver = (SaveManager)bf.Deserialize(savedLifes);
            savedLifes.Close();
            //all progress to load
            playerLifes = saver.playerLifes;
            timeToGetLifes = saver.timeToGetLifes;
        }
        
      
    }
    public void SavePlayerProgress() {

        BinaryFormatter bf = new BinaryFormatter();
        FileStream savedGame = File.Create(Application.persistentDataPath + "/savePProgress.bw");
        SaveManager saver = new SaveManager();
        //all stuff to save


       
        saver.Progress_levelToSave = currLevel;
        
        
        
        
        //
        bf.Serialize(savedGame, saver);
        savedGame.Close();
    }
    public void SavePlayerSettings()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream savedSettings = File.Create(Application.persistentDataPath + "/savePSettings.bw");
        SaveManager saver = new SaveManager();
        //all settings to save
        saver.music = soundMusicStatus;
        saver.sfx = soundSFXStatus;
        saver.Settings_sensibility = sensibility;
        saver.playerLifes = playerLifes;
        saver.tutorialCompleted = tutorialCompleted;
        
        //
        bf.Serialize(savedSettings, saver);
        savedSettings.Close();



    }
    public void LoadPProgress()
    {
        if (File.Exists(Application.persistentDataPath + "/savePProgress.bw"))
        {
           
            
            BinaryFormatter bf = new BinaryFormatter();
            FileStream savedGame = File.Open(Application.persistentDataPath + "/savePProgress.bw", FileMode.Open);
            SaveManager saver = (SaveManager)bf.Deserialize(savedGame);
            savedGame.Close();
            //all progress to load
            loadedLevel = saver.Progress_levelToSave;
           
        }
        else { }
       
        
    }
    public void LoadPlayerSettings(){


        if (File.Exists(Application.persistentDataPath + "/savePSettings.bw"))
        {
            
            BinaryFormatter bf = new BinaryFormatter();
            FileStream savedSettings = File.Open(Application.persistentDataPath + "/savePSettings.bw", FileMode.Open);
            SaveManager saver = (SaveManager)bf.Deserialize(savedSettings);
            savedSettings.Close();
            //all settings to load
            tutorialCompleted = saver.tutorialCompleted;
            sensibility = saver.Settings_sensibility;
            soundMusicStatus = saver.music;
            soundSFXStatus = saver.sfx;
        }
        else {  }
}
    public void Delete()
    {

        if (File.Exists(Application.persistentDataPath + "/savedGame"))
        {

            File.Delete(Application.persistentDataPath + "/savedGame");

        }

    }
}
