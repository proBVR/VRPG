using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainContoroller : MonoBehaviour {
    public UDPReceiver updReceiver;
    public Transform phoneTf;
    public Quaternion phoneRot;

    // Use this for initialization
    void Start()
    {
        UDPReceiver.AccelCallBack += AccelAction;
        UDPReceiver.GyroCallBack += GyroAction;
        updReceiver.UDPStart();
    }

    public void AccelAction(float xx, float yy, float zz)
    {
        var vector = new Vector3(xx,yy,zz);
        Debug.Log(vector);
    }

    public void GyroAction(float xx, float yy, float zz, float ww)
    {
        var newQut = new Quaternion(-xx, -zz, -yy, ww);
        var newRot = newQut * Quaternion.Euler(90f, 0, 0);
        phoneRot = newRot;
    }

    // Update is called once per frame
    void Update()
    {
        phoneTf.localRotation = phoneRot;
    }
}
