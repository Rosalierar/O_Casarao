using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentObjectReference : MonoBehaviour
{
    public DetectionObjects detectionObjects; // Refer�ncia ao script Detection
    public DropTheObject dropTheObject; // Refer�ncia ao script DropObject
    public GrabTheObject grabTheObject;
    public UseTheObject useTheObject; // Ref�rencia ao Script UseTheObject
    public Inventory inventory;

    //public GameObject itemCarregado;

    private void Awake()
    {
        detectionObjects = GetComponent<DetectionObjects>();
        dropTheObject = GetComponent<DropTheObject>();
        grabTheObject = GetComponent<GrabTheObject>();
        useTheObject = GetComponent<UseTheObject>();
        inventory = GetComponent<Inventory>();
    }
    // Start is called before the first frame update
    void Start()
    {
        grabTheObject.enabled = true;
        dropTheObject.enabled = false;
        useTheObject.enabled = false;
    }
}
