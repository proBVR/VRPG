using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainContoroller : MonoBehaviour {
    public UDPReceiver updReceiver;
    public UDPMove udpMove;
    public UDPDirection udpDirection;
    public Transform phoneTf;
    public Quaternion phoneRot;
    public Vector3 phonePosi;
    public Quaternion phoneDir;
    bool move;

    [SerializeField]
    private float moveSpeed;


    // Use this for initialization
    void Start()
    {
        UDPReceiver.GyroCallBack += GyroAction;
        UDPMove.AccelCallBack += AccelAction;
        UDPDirection.GyroCallBack += GyroAction2;
        updReceiver.UDPStart();
        udpMove.UDPStart();
        udpDirection.UDPStart();
        move = false;
    }

    public void AccelAction(float xx, float yy, float zz)
    {
        //var vector = new Vector3(xx,yy,zz);
        //Debug.Log(vector);
        if (xx > 2)
        {
            move = !move;
            Debug.Log(xx);
        }
    }

    public void GyroAction(float xx, float yy, float zz, float ww)
    {
        //var newQut = new Quaternion(-xx, -zz, -yy, ww);
        var newQut = new Quaternion(0, -zz, 0, ww);
        //var newRot = newQut * Quaternion.Euler(90f, 0, 0);
        var newRot = newQut * Quaternion.Euler(0, 90f, 0);
        phoneRot = newRot;
    }

    public void GyroAction2(float xx, float yy, float zz, float ww)
    {
        var newQut2 = new Quaternion(0, -zz, 0, ww);
        var newRot2 = newQut2 * Quaternion.Euler(0, 90f, 0);
        //var newQut = new Quaternion(-xx, -zz, -yy, ww);
        //var newRot = newQut * Quaternion.Euler(90f, 0, 0);
        phoneDir = newRot2;
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            //phonePosi = phoneDir.eulerAngles * movespeed;
            //Debug.Log(phoneDir.eulerAngles);
            phonePosi = phoneTf.forward * moveSpeed;
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