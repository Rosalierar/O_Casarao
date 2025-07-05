using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Cinemachine.Editor;
using Fusion;
using UnityEngine;

public class OffOnVirtualCamera : MonoBehaviour
{
    NetworkObject networkObject;
    CinemachineVirtualCamera cinemachine;
    // Start is called before the first frame update
    void Start()
    {
        networkObject = GetComponentInParent<NetworkObject>();

        if (networkObject.HasInputAuthority)
        {
            Invoke("OnVirtual", 0.1f);
        }
        else if (!networkObject.HasInputAuthority)
        {
            Invoke("OffVirtual", 0.1f);
        }
    }

    public void OnVirtual()
    {
        gameObject.SetActive(true);
    }

    public void OffVirtual()
    {
        gameObject.SetActive(false);
    }
}
