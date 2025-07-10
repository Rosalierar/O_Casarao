using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class NetDoorMoviment : NetworkBehaviour
{
    [Networked] private Vector3 boxSize { get; set; }
    [Networked] private Vector3 boxCenter { get; set; }

    NetworkObject networkObject;
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
        
        if (networkObject.HasStateAuthority)
        {
            if (boxCollider != null && gameObject.tag == "Machine")
            {
                boxSize = new Vector3(1.807f,1.85f,1.438f);
                boxCenter = new Vector3(0.046f, 0.425f, -0.11f);
            }
        }
    }

    public override void FixedUpdateNetwork()
    {
        print("Size: " + boxSize + "Center: " + boxCenter);
        if (boxCollider != null && gameObject.tag == "Machine")
        {
            ApplyColliderChanges();
        }
    }

    private void ApplyColliderChanges()
    {
        boxCollider.size = boxSize;
        boxCollider.center = boxCenter;
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
        if (boxCollider != null)
        {
            if (networkObject.HasStateAuthority)
            {
                print("PORTA DA MAQUINA TEM STATE, MUDANDO COLLIDER");
                ChangeSizeCollider();
            }
            else
            {
                print("PORTA DA MAQUINA NÃO TEM STATE, PEDINDO PARA MUDAR O COLLIDER");
                RPC_SetChangeSizeCollider();
            }
        }
        
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

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SetChangeSizeCollider()
    {
        ChangeSizeCollider();
    }

    public void ChangeSizeCollider()
    {
        boxSize = new Vector3(0.87f, 0.80f, 2.70f);
        boxCenter = new Vector3(0.06f, -0.10f, -0.70f);
        pants.SetActive(false);
    }
}
