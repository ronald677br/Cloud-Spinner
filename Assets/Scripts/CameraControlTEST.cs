using UnityEngine;
using System.Collections;
using System;
public class CameraControlTEST : MonoBehaviour
{

    public Transform PlayerLocation;
    private Vector3 cameraPos;
    private Vector3 cameraInvertPos;
    private Vector3 distance = new Vector3(0,-1,4);
    private Vector3 playerPos;
    public PlayerControlAndroid reference;
    private GameObject player;
    
    void LateUpdate()
    {
        player = GameObject.Find("Player");
        reference = player.GetComponent<PlayerControlAndroid>();

        if (reference.Inversion == true)
        {
            playerPos = PlayerLocation.GetComponent<Rigidbody>().position;
            cameraInvertPos = playerPos - new Vector3(0, 0.3f, 4);

            if (cameraPos.y != cameraInvertPos.y) {
                cameraInvertPos.y += 0.2f;
                transform.Translate(cameraInvertPos);

            }
            transform.position = cameraInvertPos;
        }

        else if (reference.flip) {
            playerPos = PlayerLocation.GetComponent<Rigidbody>().position;
            cameraPos = playerPos - new Vector3(0, -1, -4);
            transform.rotation = new Quaternion(0, -180, 0, 0);
            transform.position = cameraPos;
        }


        else
        {

            playerPos = PlayerLocation.GetComponent<Rigidbody>().position;
            cameraPos = playerPos - distance;
            transform.position = cameraPos;
            transform.rotation = new Quaternion(0, 0, 0, 0);
           
        } 
       
       
    }
}
