using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Globalization;
using System.IO;
using UnityEngine.UI;


public class LifeControl : MonoBehaviour
{
    public DateTime dateTime;
    public DateTime dateTimeToGetBalls;
    public SaveLoad saveReference;
    private bool connectionFailed;
    private int compareTime;
    private float timeFromStartup;
    private float timeToCheckConnection;
    private int timesNotConnected = 0;
    public Image WirelessOn, WirelessOff;
    public Color on, off;

    public DateTime GetInternetTime()
    {
        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.microsoft.com");
        myHttpWebRequest.Timeout = 5000;
        myHttpWebRequest.ReadWriteTimeout = 5000;

        try
        {
            var response = myHttpWebRequest.GetResponse();
            string todaysDates = response.Headers["date"];
            dateTime = DateTime.ParseExact(todaysDates, "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal);
            connectionFailed = false;
            timesNotConnected = 0;
        }
        catch (WebException)
        {
            connectionFailed = true;
           
            timesNotConnected++;
        }

        return dateTime;
    }
    void Start()
    {


        saveReference = GameObject.Find("SaveManager").GetComponent<SaveLoad>();
        GetInternetTime();
        if (timesNotConnected < 2 && connectionFailed)
        {
            dateTime = DateTime.Now;


        }
        if (connectionFailed)
        {
            WirelessOff.color = on;
            WirelessOn.color = off;

        }
        if(connectionFailed == false)
        {
            WirelessOff.color = off;
            WirelessOn.color = on;

        }
        saveReference.LoadPlayerLifes();
        if (File.Exists(Application.persistentDataPath + "/savePLifes.bw"))
        {                                
            dateTimeToGetBalls = saveReference.timeToGetLifes;
            CheckTime(dateTime, dateTimeToGetBalls);
        }
       if(!File.Exists(Application.persistentDataPath + "/savePLifes.bw"))
        {            
            saveReference.playerLifes = 10;
            dateTimeToGetBalls = dateTime.AddHours(2);
            saveReference.timeToGetLifes = dateTimeToGetBalls;
            saveReference.SavePlayerLifes();
        }
    }
    void Update()
    {

        if (connectionFailed)
        {
            timeFromStartup = Time.timeSinceLevelLoad;
            timeToCheckConnection = timeFromStartup + 300;

            if(timeFromStartup >= timeToCheckConnection)
            {
                GetInternetTime();

            }

        }

    }
    private void CheckTime(DateTime old, DateTime limit)
    {
        compareTime = DateTime.Compare(old, limit);
        if(compareTime >=0 )
        {
            if (saveReference.playerLifes < 10) { saveReference.playerLifes = 10;}
            GetInternetTime();
            dateTimeToGetBalls = dateTime.AddHours(2);
            saveReference.timeToGetLifes = dateTimeToGetBalls;
            saveReference.SavePlayerLifes();
        }
    }

   
}


