using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
[System.Serializable]
public class SaveManager  {
    public float Settings_sensibility;
    public int Progress_levelToSave;
    public int playerLifes;
    public bool music, sfx, tutorialCompleted;
    public DateTime timeToGetLifes;
}
