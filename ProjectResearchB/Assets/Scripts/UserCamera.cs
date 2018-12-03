﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCamera : MonoBehaviour // カメラを制御
{
    [SerializeField]
    private Transform mycamera;
    private readonly Vector3 head = new Vector3(0, 1.35f, 0);
    public Vector3 offset = Vector3.zero;
    private float bodyScale = 0.5f;

    private void Start()
    {
        //transform.localScale *= bodyScale;
        transform.localScale /= (float)1.2f;
    }

    public void Reset()
    {
        //offset = mycamera.position - Player.instance.transform.position - head;
        transform.position += transform.parent.position - mycamera.position + head +transform.parent.forward * 0.05f;
    }
}
