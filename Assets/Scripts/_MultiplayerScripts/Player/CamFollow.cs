using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] Transform view;
    NetworkObject networkObject;
    // Start is called before the first frame update
    void Start()
    {
        networkObject = GetComponentInParent<NetworkObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!networkObject.HasInputAuthority) return;

        transform.position = Vector3.Lerp(transform.position, view.position, 90f * Time.deltaTime);
    }
}
