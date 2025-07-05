using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetParentObjectReference : MonoBehaviour
{
    NetworkObject networkObject;

    public NetDetectionObjects detectionObjects; // Refer�ncia ao script Detection
    public NetDropTheObject dropTheObject; // Refer�ncia ao script DropObject
    public NetGrabTheObject grabTheObject;
    public NetUseTheObject useTheObject; // Ref�rencia ao Script UseTheObject
    public NetInventory inventory;
    public AudioClip[] AC;

    // Start is called before the first frame update
    void Start()
    {
        networkObject = GetComponentInParent<NetworkObject>();

        if (networkObject.HasInputAuthority)
        {
            detectionObjects = GetComponent<NetDetectionObjects>();
            dropTheObject = GetComponent<NetDropTheObject>();
            grabTheObject = GetComponent<NetGrabTheObject>();
            useTheObject = GetComponent<NetUseTheObject>();
            inventory = GetComponent<NetInventory>();

            grabTheObject.enabled = true;
            dropTheObject.enabled = false;
            useTheObject.enabled = false;
        }
    }
}
