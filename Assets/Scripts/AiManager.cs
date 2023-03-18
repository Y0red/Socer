using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{
    public List<StateController> controller;

    StateController ai1, ai2, ai3;

    private void Start()
    {
        ai1 = controller[0];
        ai2 = controller[1];
        //ai3 = controller[2];
        foreach(StateController ai in controller)
        {
            //ai.enabled = false;
        }
    }
}
