using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCamera : MonoBehaviour // カメラを制御
{ 
    [SerializeField]
    private readonly float rate = 5;
    private float ud_deg=0;
    private Vector3 offset = new Vector3(0, 1.2f, 0);
	
    
	void Update ()
    {
      //if(Input.get)  
    } 
}
