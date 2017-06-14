using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;


public class PlayerControl : MonoBehaviour
{
    public ParticleSystem particleReference;
    public MenuManager menuReference;
    public SaveLoad saveReference;
    public Rotation powerupReference;
    public GameObject spawnClone;
    public Transform boost_assign;
    public Transform ultraboost_assign;
    public Transform jump_assign;
    public Canvas canvasAssign;
    public Transform flip_assign;
    public Vector3 boostLocation;
    public Vector3 jumpLocation;
    public ParticleSystem wind_assign;
    private Vector3 grav = new Vector3(0, -9.8f, 0);
    private Vector3 boostZ = new Vector3(0, 0, 3000);
    private Vector3 boostY = new Vector3(0, 300000, 0);
    private Vector3 boostAdd = new Vector3(0, 0, 3500);
    private Vector3 UltraBoostV3 = new Vector3(0, 0, 3000000);
    private Vector3 movement;
    private Vector3 playerPos;
    public Menu FinishUI;
    private int boostlvl = 0;
    public int[] checksPerLevel;
    private int currLevel;
    public int playerCoins = 0;
    private float MoveX;
    private float windTime;
    private float speed = 6000;
    private float time = 0;
    private int checkCount = 0;
    public float ballSpeed;
    private bool boost = false;
    private bool jump = false;
    private bool UltraBoost = false;
    public bool Inversion;
    public bool activeSave = false;
    public bool flip = false;
    public bool returnFlip = false;
    
    public bool callBoost = false;
    public bool cameraForward = false;
    public bool windSpawn = false;



    void Start()
    {
        Time.timeScale = 1;
        if(saveReference)
        currLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        saveReference = GameObject.Find("SaveManager").GetComponent<SaveLoad>();
        menuReference = canvasAssign.GetComponent<MenuManager>();
        powerupReference = GetComponent<Rotation>();
        checkCount = 0;
        InvokeRepeating("decreaseTimeRemaining", 1.0f, 1.0f);
        Physics.gravity = grav;


    }

   

    void FixedUpdate()
    {

        
        playerPos = GetComponent<Rigidbody>().position;
        
        ballSpeed = GetComponent<Rigidbody>().velocity.magnitude;
       

        MoveX = Input.GetAxis("Mouse X");
        
        movement = new Vector3(MoveX, 0.0f, 1);
       

        if (flip)
        {
            GetComponent<Rigidbody>().AddForce(-movement * speed);
        }
       
        else
        {
            GetComponent<Rigidbody>().AddForce(movement * speed);
        }

        if (flip)
        {

            if (boost)
            {



                if (time > 0)
                {
                    GetComponent<Rigidbody>().AddForce(-boostZ);
                    if (boostlvl == 2)
                    {
                        GetComponent<Rigidbody>().AddForce(-boostZ - boostAdd);
                    }
                    if (boostlvl == 3)
                    {
                        GetComponent<Rigidbody>().AddForce(-boostZ - boostAdd * 2);
                    }
                    if (boostlvl >= 4)
                    {
                        GetComponent<Rigidbody>().AddForce(-boostZ - boostAdd * 3);
                    }

                }

                time -= Time.deltaTime;
                if (time <= 0)
                {
                    boostlvl = 0;
                    boost = false;

                }
            }
        }

      

        else
        {
            if (boost)
            {
                if (time > 0)
                {
                    GetComponent<Rigidbody>().AddForce(boostZ);
                    if (boostlvl == 2)
                    {
                        GetComponent<Rigidbody>().AddForce(boostZ + boostAdd);
                    }
                    if (boostlvl == 3)
                    {
                        GetComponent<Rigidbody>().AddForce(boostZ + boostAdd * 2);
                    }
                    if (boostlvl >= 4)
                    {
                        GetComponent<Rigidbody>().AddForce(boostZ + boostAdd * 3);
                    }

                }


                time -= Time.deltaTime;
                if (time <= 0)
                {
                    boostlvl = 0;
                    boost = false;

                }
            }
        }


        if (jump)
        {
            GetComponent<Rigidbody>().AddForce(boostY);
            jump = false;
        }
        if (flip)
        {
            if (UltraBoost)
            {
                GetComponent<Rigidbody>().AddForce(-UltraBoostV3);
                UltraBoost = false;
            }
        }
        else
        {
            if (UltraBoost)
            {
               
                GetComponent<Rigidbody>().AddForce(UltraBoostV3);
                UltraBoost = false;
            }
        }
        if (windSpawn)
        {

            wind_assign.Play(true);
            wind_assign.transform.position = playerPos + new Vector3(0, 0, 0.5f);
            windTime -= Time.deltaTime;
            if (windTime <= 0) { windSpawn = false; }

        }
        else { wind_assign.Stop(true); }
        
    }

    void OnTriggerEnter(Collider colider)
    {
        if (colider.gameObject.tag == "Pickable")
        {
            boostLocation = colider.GetComponent<MeshCollider>().transform.position;

            time = 2;
            boostlvl++;
            boost = true;
            colider.gameObject.SetActive(false);
            spawnBoost();


        }
        if (colider.gameObject.tag == "jumpBox")
        {
            boostLocation = colider.GetComponent<MeshCollider>().transform.position;
            jump = true;
            colider.gameObject.SetActive(false);
            spawnJump();
        }
        if (colider.gameObject.tag == "Fall")
        {

            Physics.gravity = new Vector3(0, -20, 0);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            colider.gameObject.SetActive(false);
        }
        if (colider.gameObject.tag == "Unfreeze")
        {
            Physics.gravity = new Vector3(0, -9.8f, 0);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            colider.gameObject.SetActive(false);
        }
        if (colider.gameObject.tag == "UltraBoost")
        {
            windTime = 3;
            UltraBoost = true;
            windSpawn = true;
            spawnUltraBoost();
            colider.gameObject.SetActive(false);
        }
        if (colider.gameObject.tag == "ChangeCamera")
        {


            colider.gameObject.SetActive(false);


        }
        if (colider.gameObject.tag == "Inversion")
        {
            Physics.gravity = new Vector3(0, 9.8f, 0);
            Inversion = true;
            spawnUltraBoost();
            colider.gameObject.SetActive(false);
        }
        if (colider.gameObject.tag == "InversionReturn")
        {
            Inversion = false;
            spawnUltraBoost();
            Physics.gravity = grav;
            colider.gameObject.SetActive(false);


        }
        if (colider.gameObject.tag == "Flip")
        {
            spawnFlip();
            flip = true;
            returnFlip = false;
            colider.gameObject.SetActive(false);
        }
        if (colider.gameObject.tag == "ReturnFlip")
        {
            spawnFlip();
            flip = false;
            colider.gameObject.SetActive(false);
        }


        if (colider.gameObject.tag == "Check")
         {

             checkCount++; 
             colider.gameObject.SetActive(false);

         }
        if (colider.gameObject.tag == "LastCheck")
        {
            
               
                saveReference.SavePlayerProgress();
                menuReference.ShowMenu(FinishUI);
                checkCount = 0;
            
           
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
            
            colider.gameObject.SetActive(false);

        }
        if (colider.gameObject.tag == "Coin")
        {

            playerCoins += 10;

        }
    }

    void spawnBoost()
    {
        boost_assign.GetComponent<AudioSource>().Play();
        if (flip)
        {
            if (ballSpeed <= 5)
            {
                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, -1), Quaternion.Euler(new Vector3(180, 180, 0))) as GameObject;


            }
            if (5.01f < ballSpeed && ballSpeed < 7)
            {
                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, -2.3f), Quaternion.Euler(new Vector3(180, 180, 0))) as GameObject;


            }

            if (7.01f < ballSpeed && ballSpeed < 10)
            {
                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, -2.6f), Quaternion.Euler(new Vector3(180, 180, 0))) as GameObject;


            }
            if (10.01f < ballSpeed && ballSpeed < 13)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, -3), Quaternion.Euler(new Vector3(180, 180, 0))) as GameObject;

            }
            if (13.01f < ballSpeed && ballSpeed < 16)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, -3.35f), Quaternion.Euler(new Vector3(180, 180, 0))) as GameObject;

            }
            if (16.01f < ballSpeed && ballSpeed < 20)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, -4), Quaternion.Euler(new Vector3(180, 180, 0))) as GameObject;

            }
            if (20.01f < ballSpeed && ballSpeed < 24)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, -4.7f), Quaternion.Euler(new Vector3(180, 180, 0))) as GameObject;

            }
            if (24.01 < ballSpeed && ballSpeed < 28)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, -5.2f), Quaternion.Euler(new Vector3(180, 180, 0))) as GameObject;

            }
            if (28.01f < ballSpeed && ballSpeed < 32)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, -6), Quaternion.Euler(new Vector3(180, 180, 0))) as GameObject;

            }
            if (32.01 < ballSpeed && ballSpeed < 36)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, -7f), Quaternion.Euler(new Vector3(180, 180, 0))) as GameObject;

            }
            if (36.01 < ballSpeed && ballSpeed < 40)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, -8), Quaternion.Euler(new Vector3(180, 180, 0))) as GameObject;

            }
            if (40.01f < ballSpeed && ballSpeed < 45)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, -8.7f), Quaternion.Euler(new Vector3(180, 180, 0))) as GameObject;

            }
            if (45.01f < ballSpeed && ballSpeed < 48)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, -9.3f), Quaternion.Euler(new Vector3(180, 180, 0))) as GameObject;

            }
            if (48.01f < ballSpeed && ballSpeed < 54)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, -10), Quaternion.Euler(new Vector3(180, 180, 0))) as GameObject;

            }
            if (54.01f < ballSpeed && ballSpeed < 60)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, -10.9f), Quaternion.Euler(new Vector3(180, 180, 0))) as GameObject;

            }
            if (60.01f < ballSpeed && ballSpeed < 65)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, -11.5f), Quaternion.Euler(new Vector3(180, 180, 0))) as GameObject;

            }
            if (65.01f < ballSpeed && ballSpeed < 70)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, -12), Quaternion.Euler(new Vector3(180, 180, 0))) as GameObject;

            }
            if (70.01f < ballSpeed && ballSpeed < 75)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, -11.5f), Quaternion.Euler(new Vector3(180, 180, 0))) as GameObject;

            }
            if (65.01f < ballSpeed && ballSpeed < 70)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, -12), Quaternion.Euler(new Vector3(180, 180, 0))) as GameObject;

            }
            if (70.01f < ballSpeed && ballSpeed < 75)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, -12.5f), Quaternion.Euler(new Vector3(180, 180, 0))) as GameObject;

            }
            if (75.01f < ballSpeed && ballSpeed < 80)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, -13), Quaternion.Euler(new Vector3(180, 180, 0))) as GameObject;

            }
            if (80.01f < ballSpeed && ballSpeed < 85)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, -13.5f), Quaternion.Euler(new Vector3(180, 180, 0))) as GameObject;

            }
        }
        else
        {
            if (ballSpeed <= 5)
            {
                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, 1), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;


            }
            if (5.01f < ballSpeed && ballSpeed < 7)
            {
                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, 2.3f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;


            }

            if (7.01f < ballSpeed && ballSpeed < 10)
            {
                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, 2.6f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;


            }
            if (10.01f < ballSpeed && ballSpeed < 13)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, 3), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (13.01f < ballSpeed && ballSpeed < 16)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, 3.35f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (16.01f < ballSpeed && ballSpeed < 20)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, 4), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (20.01f < ballSpeed && ballSpeed < 24)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, 4.7f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (24.01 < ballSpeed && ballSpeed < 28)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, 5.2f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (28.01f < ballSpeed && ballSpeed < 32)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, 6), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (32.01 < ballSpeed && ballSpeed < 36)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, 7f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (36.01 < ballSpeed && ballSpeed < 40)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, 8), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (40.01f < ballSpeed && ballSpeed < 45)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, 8.7f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (45.01f < ballSpeed && ballSpeed < 48)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, 9.3f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (48.01f < ballSpeed && ballSpeed < 54)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, 10), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (54.01f < ballSpeed && ballSpeed < 60)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, 10.9f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (60.01f < ballSpeed && ballSpeed < 65)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, 11.5f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (65.01f < ballSpeed && ballSpeed < 70)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, 12), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (70.01f < ballSpeed && ballSpeed < 75)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, 11.5f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (65.01f < ballSpeed && ballSpeed < 70)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, 12), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (70.01f < ballSpeed && ballSpeed < 75)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, 12.5f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (75.01f < ballSpeed && ballSpeed < 80)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, 13), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (80.01f < ballSpeed && ballSpeed < 85)
            {

                spawnClone = Instantiate(boost_assign, playerPos + new Vector3(0, 0, 13.5f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
        }
    }
    void spawnJump()
    {
        jump_assign.GetComponent<AudioSource>().Play();
        if (ballSpeed <= 5)
        {
            spawnClone = Instantiate(jump_assign, playerPos + new Vector3(0, 0, 1), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;


        }
        if (5.01f < ballSpeed && ballSpeed < 7)
        {
            spawnClone = Instantiate(jump_assign, playerPos + new Vector3(0, 0, 2.3f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;


        }

        if (7.01f < ballSpeed && ballSpeed < 10)
        {
            spawnClone = Instantiate(jump_assign, playerPos + new Vector3(0, 0, 2.6f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;


        }
        if (10.01f < ballSpeed && ballSpeed < 13)
        {

            spawnClone = Instantiate(jump_assign, playerPos + new Vector3(0, 0, 3), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (13.01f < ballSpeed && ballSpeed < 16)
        {

            spawnClone = Instantiate(jump_assign, playerPos + new Vector3(0, 0, 3.35f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (16.01f < ballSpeed && ballSpeed < 20)
        {

            spawnClone = Instantiate(jump_assign, playerPos + new Vector3(0, 0, 4), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (20.01f < ballSpeed && ballSpeed < 24)
        {

            spawnClone = Instantiate(jump_assign, playerPos + new Vector3(0, 0, 4.7f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (24.01 < ballSpeed && ballSpeed < 28)
        {

            spawnClone = Instantiate(jump_assign, playerPos + new Vector3(0, 0, 5.2f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (28.01f < ballSpeed && ballSpeed < 32)
        {

            spawnClone = Instantiate(jump_assign, playerPos + new Vector3(0, 0, 6), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (32.01 < ballSpeed && ballSpeed < 36)
        {

            spawnClone = Instantiate(jump_assign, playerPos + new Vector3(0, 0, 7f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (36.01 < ballSpeed && ballSpeed < 40)
        {

            spawnClone = Instantiate(jump_assign, playerPos + new Vector3(0, 0, 8), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (40.01f < ballSpeed && ballSpeed < 45)
        {

            spawnClone = Instantiate(jump_assign, playerPos + new Vector3(0, 0, 8.7f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (45.01f < ballSpeed && ballSpeed < 48)
        {

            spawnClone = Instantiate(jump_assign, playerPos + new Vector3(0, 0, 9.3f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (48.01f < ballSpeed && ballSpeed < 54)
        {

            spawnClone = Instantiate(jump_assign, playerPos + new Vector3(0, 0, 10), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (54.01f < ballSpeed && ballSpeed < 60)
        {

            spawnClone = Instantiate(jump_assign, playerPos + new Vector3(0, 0, 10.9f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (60.01f < ballSpeed && ballSpeed < 65)
        {

            spawnClone = Instantiate(jump_assign, playerPos + new Vector3(0, 0, 11.5f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (65.01f < ballSpeed && ballSpeed < 70)
        {

            spawnClone = Instantiate(jump_assign, playerPos + new Vector3(0, 0, 12), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (70.01f < ballSpeed && ballSpeed < 75)
        {

            spawnClone = Instantiate(jump_assign, playerPos + new Vector3(0, 0, 11.5f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (65.01f < ballSpeed && ballSpeed < 70)
        {

            spawnClone = Instantiate(jump_assign, playerPos + new Vector3(0, 0, 12), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (70.01f < ballSpeed && ballSpeed < 75)
        {

            spawnClone = Instantiate(jump_assign, playerPos + new Vector3(0, 0, 12.5f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (75.01f < ballSpeed && ballSpeed < 80)
        {

            spawnClone = Instantiate(jump_assign, playerPos + new Vector3(0, 0, 13), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (80.01f < ballSpeed && ballSpeed < 85)
        {

            spawnClone = Instantiate(jump_assign, playerPos + new Vector3(0, 0, 13.5f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
    }
    void spawnUltraBoost()
    {
        
        ultraboost_assign.GetComponent<AudioSource>().Play();
        if (ballSpeed <= 5)
        {
            spawnClone = Instantiate(ultraboost_assign, playerPos + new Vector3(0, 0, 1), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;


        }
        if (5.01f < ballSpeed && ballSpeed < 7)
        {
            spawnClone = Instantiate(ultraboost_assign, playerPos + new Vector3(0, 0, 2.3f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;


        }

        if (7.01f < ballSpeed && ballSpeed < 10)
        {
            spawnClone = Instantiate(ultraboost_assign, playerPos + new Vector3(0, 0, 2.6f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;


        }
        if (10.01f < ballSpeed && ballSpeed < 13)
        {

            spawnClone = Instantiate(ultraboost_assign, playerPos + new Vector3(0, 0, 3), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (13.01f < ballSpeed && ballSpeed < 16)
        {

            spawnClone = Instantiate(ultraboost_assign, playerPos + new Vector3(0, 0, 3.35f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (16.01f < ballSpeed && ballSpeed < 20)
        {

            spawnClone = Instantiate(ultraboost_assign, playerPos + new Vector3(0, 0, 4), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (20.01f < ballSpeed && ballSpeed < 24)
        {

            spawnClone = Instantiate(ultraboost_assign, playerPos + new Vector3(0, 0, 4.7f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (24.01 < ballSpeed && ballSpeed < 28)
        {

            spawnClone = Instantiate(ultraboost_assign, playerPos + new Vector3(0, 0, 5.2f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (28.01f < ballSpeed && ballSpeed < 32)
        {

            spawnClone = Instantiate(ultraboost_assign, playerPos + new Vector3(0, 0, 6), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (32.01 < ballSpeed && ballSpeed < 36)
        {

            spawnClone = Instantiate(ultraboost_assign, playerPos + new Vector3(0, 0, 7f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (36.01 < ballSpeed && ballSpeed < 40)
        {

            spawnClone = Instantiate(ultraboost_assign, playerPos + new Vector3(0, 0, 8), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (40.01f < ballSpeed && ballSpeed < 45)
        {

            spawnClone = Instantiate(ultraboost_assign, playerPos + new Vector3(0, 0, 8.7f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (45.01f < ballSpeed && ballSpeed < 48)
        {

            spawnClone = Instantiate(ultraboost_assign, playerPos + new Vector3(0, 0, 9.3f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (48.01f < ballSpeed && ballSpeed < 54)
        {

            spawnClone = Instantiate(ultraboost_assign, playerPos + new Vector3(0, 0, 10), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (54.01f < ballSpeed && ballSpeed < 60)
        {

            spawnClone = Instantiate(ultraboost_assign, playerPos + new Vector3(0, 0, 10.9f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (60.01f < ballSpeed && ballSpeed < 65)
        {

            spawnClone = Instantiate(ultraboost_assign, playerPos + new Vector3(0, 0, 11.5f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (65.01f < ballSpeed && ballSpeed < 70)
        {

            spawnClone = Instantiate(ultraboost_assign, playerPos + new Vector3(0, 0, 12), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (70.01f < ballSpeed && ballSpeed < 75)
        {

            spawnClone = Instantiate(ultraboost_assign, playerPos + new Vector3(0, 0, 11.5f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (65.01f < ballSpeed && ballSpeed < 70)
        {

            spawnClone = Instantiate(ultraboost_assign, playerPos + new Vector3(0, 0, 12), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (70.01f < ballSpeed && ballSpeed < 75)
        {

            spawnClone = Instantiate(ultraboost_assign, playerPos + new Vector3(0, 0, 12.5f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (75.01f < ballSpeed && ballSpeed < 80)
        {

            spawnClone = Instantiate(ultraboost_assign, playerPos + new Vector3(0, 0, 13), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }
        if (80.01f < ballSpeed && ballSpeed < 85)
        {

            spawnClone = Instantiate(ultraboost_assign, playerPos + new Vector3(0, 0, 13.5f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

        }

    }
    void spawnFlip()
    {
        flip_assign.GetComponent<AudioSource>().Play();
        if (flip)
        {
            if (ballSpeed <= 5)
            {
                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, -1), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;


            }
            if (5.01f < ballSpeed && ballSpeed < 7)
            {
                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, -2.3f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;


            }

            if (7.01f < ballSpeed && ballSpeed < 10)
            {
                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, -2.6f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;


            }
            if (10.01f < ballSpeed && ballSpeed < 13)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0,-3), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (13.01f < ballSpeed && ballSpeed < 16)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, -3.35f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (16.01f < ballSpeed && ballSpeed < 20)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, -4), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (20.01f < ballSpeed && ballSpeed < 24)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, -4.7f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (24.01 < ballSpeed && ballSpeed < 28)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, -5.2f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (28.01f < ballSpeed && ballSpeed < 32)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, -6), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (32.01 < ballSpeed && ballSpeed < 36)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, -7), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (36.01 < ballSpeed && ballSpeed < 40)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, -8), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (40.01f < ballSpeed && ballSpeed < 45)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, -8.7f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (45.01f < ballSpeed && ballSpeed < 48)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0,-9.3f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (48.01f < ballSpeed && ballSpeed < 54)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, -10), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (54.01f < ballSpeed && ballSpeed < 60)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, -10.9f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (60.01f < ballSpeed && ballSpeed < 65)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, -11.5f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (65.01f < ballSpeed && ballSpeed < 70)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, -12), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (70.01f < ballSpeed && ballSpeed < 75)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, -11.5f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (65.01f < ballSpeed && ballSpeed < 70)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, -12), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (70.01f < ballSpeed && ballSpeed < 75)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, -12.5f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (75.01f < ballSpeed && ballSpeed < 80)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, -13), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (80.01f < ballSpeed && ballSpeed < 85)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, -13.5f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
        }
        else {
            if (ballSpeed <= 5)
            {
                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, 1), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;


            }
            if (5.01f < ballSpeed && ballSpeed < 7)
            {
                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, 2.3f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;


            }

            if (7.01f < ballSpeed && ballSpeed < 10)
            {
                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, 2.6f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;


            }
            if (10.01f < ballSpeed && ballSpeed < 13)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, 3), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (13.01f < ballSpeed && ballSpeed < 16)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, 3.35f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (16.01f < ballSpeed && ballSpeed < 20)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, 4), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (20.01f < ballSpeed && ballSpeed < 24)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, 4.7f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (24.01 < ballSpeed && ballSpeed < 28)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, 5.2f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (28.01f < ballSpeed && ballSpeed < 32)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, 6), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (32.01 < ballSpeed && ballSpeed < 36)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, 7f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (36.01 < ballSpeed && ballSpeed < 40)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, 8), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (40.01f < ballSpeed && ballSpeed < 45)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, 8.7f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (45.01f < ballSpeed && ballSpeed < 48)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, 9.3f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (48.01f < ballSpeed && ballSpeed < 54)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, 10), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (54.01f < ballSpeed && ballSpeed < 60)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, 10.9f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (60.01f < ballSpeed && ballSpeed < 65)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, 11.5f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (65.01f < ballSpeed && ballSpeed < 70)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, 12), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (70.01f < ballSpeed && ballSpeed < 75)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, 11.5f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (65.01f < ballSpeed && ballSpeed < 70)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, 12), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (70.01f < ballSpeed && ballSpeed < 75)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, 12.5f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (75.01f < ballSpeed && ballSpeed < 80)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, 13), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
            if (80.01f < ballSpeed && ballSpeed < 85)
            {

                spawnClone = Instantiate(flip_assign, playerPos + new Vector3(0, 0, 13.5f), Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;

            }
        }


    }
}

//BoostZ = 3000
//BoostY = 300000
//BoostAdd = 3500
//UltraBoostV3 = 3000000
//Speed = 6000
//Mass = 500
//Drag = 0.9
//ADrag = 0.05
//Grav = true
//Sensibility = 6 (G3)
//Fixed Timestep: 0,015
//Maximum allowed timestep: 0,3333333
//Time scale: 1
//Camera settings
//Distance Y = -1
//Distance Z = 4
//Texture Green color : 80 155 43
//Texture Red color: 255
//Texture Blue color: 34 91 255
//C:\Users\Ronald\AppData\Local\Android\sdk
//Designed by Freepik (Particle lights)