using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMoviment : MonoBehaviour
{
    public Transform doorTransform;
    [SerializeField] private float openAngle = -90f;
    [SerializeField] float openSpeed = 2f;

    [SerializeField] private bool isOpen = false;
    [SerializeField] private bool isMoving = false;
    //private bool playerNear = false;

    [SerializeField] private Quaternion closedRot;
    [SerializeField] private Quaternion openRot;

    void Awake()
    {
        closedRot = doorTransform.rotation;
        openRot = Quaternion.Euler(doorTransform.eulerAngles + Vector3.down * openAngle);
    }

    public void TryActiveDoor()
    {
        if (!isMoving)
            StartCoroutine(ToggleDoor());
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
