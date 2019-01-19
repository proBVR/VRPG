using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCamera : MonoBehaviour // カメラを制御
{
    [SerializeField]
    private Transform mycamera, leftHand, rightHand;
    private readonly Vector3 head = new Vector3(0, 1.35f, 0);
    private readonly float lengthHandToHand = 1.14f;
    public Vector3 offset = Vector3.zero;
    private float bodyScale = 0.5f;

    private void Start()
    {
        //transform.localScale *= bodyScale;
        transform.localScale /= (float)1.2f;
    }

    private void Update()
    {
        var temp = mycamera.position - mycamera.forward * 0.075f - Player.instance.transform.position;//vector: from player to mycamera
        temp.y = 0;
        Player.instance.transform.position += temp;
        transform.position -= temp;
    }

    public void Reset()
    {
        transform.position += transform.parent.position - mycamera.position + head +transform.parent.forward * 0.05f;
        var length = Vector3.Distance(leftHand.position, rightHand.position);
        transform.localScale *= lengthHandToHand / length;
        Debug.Log("length: "+length);
    }
}
