using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMoviment : MonoBehaviour
{
    public Transform doorTransform;
    public float openAngle = -90f;
    public float openSpeed = 2f;

    private bool isOpen = false;
    private bool isMoving = false;
    private bool playerNear = false;

    private Quaternion closedRot;
    private Quaternion openRot;

    void Awake()
    {
        closedRot = doorTransform.rotation;
        openRot = Quaternion.Euler(doorTransform.eulerAngles + Vector3.up * openAngle);
    }

    void Update()
    {
        StartCoroutine(ToggleDoor());
    }

    private System.Collections.IEnumerator ToggleDoor()
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
