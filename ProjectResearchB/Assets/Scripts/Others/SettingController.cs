using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingController : MonoBehaviour {
    [SerializeField]
    public Transform  leftHand, rightHand;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            var firstlength = Vector3.Distance(leftHand.position, rightHand.position);
            if (firstlength > 0)
            {
                UserCamera.length = firstlength;
               SceneManager.LoadScene("GameScene");
            }

            ////debug用
            //UserCamera.length = 1.14f;
            //SceneManager.LoadScene("GameScene");
        }
    }
}
