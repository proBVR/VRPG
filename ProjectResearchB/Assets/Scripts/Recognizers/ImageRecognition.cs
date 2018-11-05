using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageRecognition : MonoBehaviour {
    public static ImageRecognition instance;
    private float x, y;

    private void Start()
    {
        instance = this;
    }

    void Update () {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
	}

    public Vector3 GetDirection()
    {
        return new Vector3(x, 0, y);
    }
}
