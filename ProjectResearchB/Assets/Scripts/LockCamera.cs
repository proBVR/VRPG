using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCamera : MonoBehaviour
{
    [SerializeField]
    private Transform mycamera;
    private readonly Vector3 pos = new Vector3(0, 2.5f, 0);

    private void Update()
    {
        transform.position += pos - mycamera.position;
    }
}
