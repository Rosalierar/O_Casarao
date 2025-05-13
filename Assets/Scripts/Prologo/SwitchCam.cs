using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SwitchCam : MonoBehaviour
{
    public CinemachineVirtualCamera cam1;
    public CinemachineVirtualCamera cam2;
    public bool isStartWatch = false;
    public bool isLeftTv = false;

    Animator animCam;

    // Start is called before the first frame update
    void Start()
    {
        animCam = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    void SwitchCamara()
    {
        if (cam1.Priority == 10 && isStartWatch && !isLeftTv)
        {
            cam1.Priority = 0;
            cam2.Priority = 10;
        }
        else if (cam2.Priority == 10 && isStartWatch && !isLeftTv)
        {

        }
    }
}
