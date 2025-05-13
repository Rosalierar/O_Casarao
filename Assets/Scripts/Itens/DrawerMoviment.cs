using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerMoviment : MonoBehaviour
{
    public Transform drawerTransform;
    public float openDistance = 0.5f;
    public float openSpeed = 2f;

    private bool isOpen = false;
    private bool isMoving = false;
    private bool playerNear = false;

    private Vector3 closedPos;
    private Vector3 openPos;

    void Awake()
    {
        closedPos = drawerTransform.position;
        openPos = closedPos + -drawerTransform.forward * openDistance;
    }

    void Update()
    {
        
        StartCoroutine(ToggleDoor());
    }

    private System.Collections.IEnumerator ToggleDoor()
    {
        isMoving = true;
        Vector3 targetPos = isOpen ? closedPos : openPos;
        Vector3 startPos = drawerTransform.position;

        float elapsed = 0f;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * openSpeed;
            drawerTransform.position = Vector3.Lerp(startPos, targetPos, elapsed);
            yield return null;
        }

        drawerTransform.position = targetPos;
        isOpen = !isOpen;
        isMoving = false;
        this.enabled = false; // Disable the script after opening/closing the door
    }
}
