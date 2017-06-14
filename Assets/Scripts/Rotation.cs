using UnityEngine;
using System.Collections;

public class Rotation : MonoBehaviour {

    public float x, y, z;
	
	void Update () {
      
        transform.Rotate(x * Time.deltaTime, y * Time.deltaTime, z * Time.deltaTime);

        RenderSettings.skybox.SetFloat("_Rotation", Time.time);
	}
}
