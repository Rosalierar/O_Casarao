using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class NetDrawerMoviment : NetworkBehaviour
{
     [SerializeField]int indexVector;
    public Transform drawerTransform;
    public float openDistance = 0.3f;
    public float openSpeed = 2f;

    private bool isOpen = false;
    private bool isMoving = false;
    //private bool playerNear = false;

    private Vector3 closedPos;
    private Vector3 openPos;

    void Start()
    {
        closedPos = drawerTransform.position;
        openPos = closedPos + drawerTransform.up * openDistance;
    }

    public void TryActiveDrawer()
    {
        if (!isMoving)
            StartCoroutine(ToggleDoor());
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void Rpc_RequestToggleDrawer()
    {
        TryActiveDrawer(); // Apenas quem tem StateAuthority executará de fato
    }

    private System.Collections.IEnumerator ToggleDoor()
    {
        isMoving = true;
        Vector3 targetPos = isOpen ? closedPos : openPos;
        Vector3 startPos = drawerTransform.position;

        float elapsed = 0f;
        while (elapsed < 1f)
        {
            elapsed += Runner.DeltaTime * openSpeed;
            drawerTransform.position = Vector3.Lerp(startPos, targetPos, elapsed);
            yield return null;
        }

        drawerTransform.position = targetPos;
        isOpen = !isOpen;
        isMoving = false;
        this.enabled = false; // Disable the script after opening/closing the door
    }
}
