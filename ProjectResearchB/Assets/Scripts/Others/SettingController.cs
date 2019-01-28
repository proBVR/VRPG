using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

public class SettingController : MonoBehaviour
{
    [SerializeField]
    private Transform leftHand, rightHand;

    // Update is called once per frame
    void Update()
    {
        if (SteamVR_Input._default.inActions.Teleport.GetStateDown(SteamVR_Input_Sources.RightHand))
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
