using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrig : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            //other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
