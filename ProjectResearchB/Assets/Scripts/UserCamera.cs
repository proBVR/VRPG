using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCamera : MonoBehaviour // カメラを制御
{ 
    [SerializeField]
    private readonly float rate = 5;
    private float ud_deg=0;
    private Vector3 offset = new Vector3(0, 1.2f, 0);
	
    /*
	void Update ()
    {
        float dx = Input.GetAxis("Camera_x");
        float dy = Input.GetAxis("Camera_y");

        transform.position = Player.instance.transform.position + offset;
        ud_deg += dy * rate;
        if (ud_deg > 45) ud_deg = 45;
        else if (ud_deg < -45) ud_deg = -45;
        else transform.eulerAngles += new Vector3(dy * rate, dx*rate, 0);

        if (Input.GetButtonDown("Rbutton"))
        {
            transform.eulerAngles = new Vector3(10, 0, 0);
            ud_deg = 0;
        }
        //Debug.Log("rot x, y, ud: " + dx + ", " + dy + ", " + ud_deg);
    } 
    */
}
