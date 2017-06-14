using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TutorialScript : MonoBehaviour
{
    public float sens;
    private Scrollbar sensScroll;
    private Text sensT;
    public SaveLoad saveReference;
    void Start()
    {
        saveReference = GameObject.Find("SaveManager").GetComponent<SaveLoad>();
        sensT = GameObject.Find("Sens").GetComponent<Text>();
    }
    void Update()
    {
        sensScroll = GameObject.Find("Scrollbar").GetComponent<Scrollbar>();
        sens = sensScroll.value;
        sensT.text = saveReference.sensibility.ToString("0.0");
        saveReference.sensibility = sens * 40;
    }


}