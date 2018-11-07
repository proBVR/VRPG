using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCamera : MonoBehaviour // カメラを制御
{
    [SerializeField]
    private Transform mycamera;
    [SerializeField]
    private readonly float rate = 5;
    private readonly Vector3 head = new Vector3(0, 1.35f, 0.05f);
    public Vector3 offset = new Vector3(0, 1.2f, 0);

    public void Reset()
    {
        offset = mycamera.position - Player.instance.transform.position - head;
    }
}
