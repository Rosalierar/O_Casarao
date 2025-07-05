using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class NetGrabTheObject : MonoBehaviour
{
    NetworkObject networkObject;
    NetParentObjectReference parent;

    [SerializeField] private MyButton interectBtn; // Bot�o de intera��o
    bool pressedButtonGrab = false; // Vari�vel para verificar se o bot�o foi pressionado
    public bool holdPressed, isHolding;

    private void Start()
    {
        networkObject = GetComponentInParent<NetworkObject>();

        if (networkObject.HasInputAuthority)
        {
            parent = GetComponent<NetParentObjectReference>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!networkObject.HasInputAuthority) return;
        
        GetPressedButtonHold();
        GrabObject();
    }

    void GrabObject()
    {
        print(parent);
        if (holdPressed && !isHolding && parent.detectionObjects.isCollidingItem) // Verifica se o bot�o foi pressionado e n�o est� segurando o obj
        {
            // Pegar
            parent.inventory.TryColetarItem(parent.detectionObjects.item);

            parent.dropTheObject.enabled = true;

            parent.detectionObjects.isCollidingItem = false;
            isHolding = true; // Define que o objeto est� sendo segurado

            Debug.Log("Pegou Objeto");
            Debug.Log("Grab:" + parent.grabTheObject.enabled + "Use:" + parent.useTheObject.enabled + "Drop:" + parent.dropTheObject.enabled);
        }
        else if (holdPressed && isHolding && parent.detectionObjects.isCollidingItem)
        {
            Debug.Log("Você já está segurando um item.");
        }

        holdPressed = false;
    }
    void GetPressedButtonHold() //verifica se o bot�o de agachar foi pressionado
    {
        if (interectBtn.isPressed && !holdPressed) //verifica se o bot�o de agachar foi pressionado
        {
            if (!pressedButtonGrab) //verifica se o bot�o n�o foi pressionado antes
            {
                pressedButtonGrab = true; //define que o bot�o foi pressionado
                holdPressed = true; //define que o bot�o de agachar foi pressionado
            }
        }
        else
        {
            pressedButtonGrab = false; //define que o bot�o n�o foi pressionado
        }
    }
}
