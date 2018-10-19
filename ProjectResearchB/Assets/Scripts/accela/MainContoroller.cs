using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainContoroller : MonoBehaviour {
    public UDPReceiver updReceiver;
    public UDPMove udpMove;
    public Transform phoneTf;
    public Quaternion phoneRot;
    public Vector3 phonePosi;
    bool move = false;
    float movespeed = 0.1f;


    // Use this for initialization
    void Start()
    {
        //UDPReceiver.AccelCallBack += AccelAction;
        UDPReceiver.GyroCallBack += GyroAction;
        UDPMove.AccelCallBack += AccelAction;
        updReceiver.UDPStart();
        udpMove.UDPStart();

    }

    public void AccelAction(float xx, float yy, float zz)
    {
        //var vector = new Vector3(xx,yy,zz);
        //Debug.Log(vector);
        //if (movetest == true)
        //{
        if (xx > 2)
        {
            move = !move;
            Debug.Log(xx);
        }
        //}
    }

    public void GyroAction(float xx, float yy, float zz, float ww)
    {
        //var newQut = new Quaternion(-xx, -zz, -yy, ww);
        var newQut = new Quaternion(0, -zz, 0, ww);
        //var newRot = newQut * Quaternion.Euler(90f, 0, 0);
        var newRot = newQut * Quaternion.Euler(0, 90f, 0);
        phoneRot = newRot;
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            phonePosi = phoneTf.forward * movespeed;
            Debug.Log(phonePosi);
        }
        else
        {
            phonePosi = Vector3.zero;
        }
        phoneTf.localRotation = phoneRot;
        phoneTf.position += phonePosi;
    }
}
