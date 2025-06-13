using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerController : NetworkBehaviour
{
    Camera camPlayer;

    Rigidbody rb;
    [SerializeField] private float playerSpeed = 2f;

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            camPlayer = GetComponentInChildren<Camera>();
            camPlayer.enabled = true;
        }
    }

    public override void FixedUpdateNetwork()
    {
        print("FIXED UPDATE NETWORK");
    }
}
