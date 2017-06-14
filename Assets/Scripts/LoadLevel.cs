using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour {
    public PlayerControl reference;
    

    public void Start()
    {
        reference = GetComponent<PlayerControl>();


    }
	
    public void Retry()
    {

        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex, UnityEngine.SceneManagement.LoadSceneMode.Single);

        
    }
    public void LevelLoad(int num)
    {
        LoadingScreenManager.LoadScene(num);

    }
    
}
