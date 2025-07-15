using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class NetDoorMoviment : NetworkBehaviour
{
    public NetworkObject networkObject;
    public GameObject pants;
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] private byte direction;
    public Transform doorTransform;
    [SerializeField] byte posForOpenDoor;
    Vector3[] posOpen = new Vector3[3] { Vector3.up, Vector3.down, Vector3.forward };
    [SerializeField] private float openAngle = -90f;
    [SerializeField] float openSpeed = 2f;

    [Networked] public bool isOpen { get; set; }
    [Networked] private bool isMoving { get; set; }
    //private bool playerNear = false;

    [SerializeField] private Quaternion closedRot;
    [SerializeField] private Quaternion openRot;

    bool IsMachine = false;

    void Start()
    {
        networkObject = GetComponentInParent<NetworkObject>();

        print("PORTA PODE TER HAS STATE: " + networkObject.HasStateAuthority);

        closedRot = doorTransform.rotation;

        if (gameObject.tag != "Machine")
        {
            openRot = Quaternion.Euler(doorTransform.eulerAngles + posOpen[posForOpenDoor] * openAngle);
        }
        else
        {
            Vector3 rotationAxis = (direction == 0) ? Vector3.right : -Vector3.right;

            openRot = Quaternion.Euler(doorTransform.eulerAngles + rotationAxis * openAngle);
        }
    }

    public void TryActiveDoor()
    {
        if (!isMoving)
        {
            if (gameObject.tag != "Machine")
                StartCoroutine(ToggleDoor());
            else
            {
                if (!IsMachine)
                    StartCoroutine(ToggleMachine());
            }
        }
    }

    private IEnumerator ToggleMachine()
    {        
        isMoving = true;
        Quaternion startRot = doorTransform.rotation;
        Quaternion targetRot = isOpen ? closedRot : openRot;

        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * openSpeed;
            doorTransform.rotation = Quaternion.Slerp(startRot, targetRot, elapsed);
            yield return null;
        }

        doorTransform.rotation = targetRot;
        isOpen = !isOpen;
        isMoving = false;
        this.enabled = false;

        IsMachine = true;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void Rpc_RequestToggleDoor()
    {
        TryActiveDoor(); // Apenas quem tem StateAuthority executará de fato
    }

    private IEnumerator ToggleDoor()
    {
        isMoving = true;
        Quaternion targetRot = isOpen ? closedRot : openRot;
        Quaternion startRot = doorTransform.rotation;

        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * openSpeed;
            doorTransform.rotation = Quaternion.Slerp(startRot, targetRot, elapsed);
            yield return null;
        }

        doorTransform.rotation = targetRot;
        isOpen = !isOpen;
        isMoving = false;
        this.enabled = false; // Disable the script after opening/closing the door
    }
}
