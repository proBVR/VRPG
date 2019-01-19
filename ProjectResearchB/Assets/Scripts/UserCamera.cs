using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCamera : MonoBehaviour // HMDのカメラを制御
{
    [SerializeField]
    private Transform mycamera, leftHand, rightHand;
    private readonly Vector3 head = new Vector3(0, 1.35f, 0);
    private readonly float lengthHandToHand = 1.14f;
    public Vector3 offset = Vector3.zero;
    private float bodyScale = 0.5f;

    private void Start()
    {
        transform.localScale /= 1.2f;
    }

    private void Update()
    {
        //vector: from player to mycamera
        var temp = mycamera.position - mycamera.forward * 0.075f - Player.instance.transform.position;
        temp.y = 0;
        Player.instance.transform.position += temp;
        transform.position -= temp;
    }

    public void HeightReset()
    {
        var temp = Player.instance.transform.position.y - mycamera.position.y;
        transform.position += Vector3.up * temp;
    }

    public void WidthReset()
    {
        var length = Vector3.Distance(leftHand.position, rightHand.position);
        if (length > 0)
            transform.localScale *= lengthHandToHand / length;
        Debug.Log("length: " + length);
    }
}
