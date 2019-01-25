using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Arm
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Damager")
        {
            user.GetStatus().guardList.Add(other.GetComponent<IDamage>());
     Debug.Log("guard");   }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Damager")
        {
            user.GetStatus().guardList.Remove(other.GetComponent<IDamage>());
        }
    }
}
