using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTheObject : MonoBehaviour
{
    ParentObjectReference parent;
    //DetectionObjects detectionObjects;

    [SerializeField] private MyButton interectBtn; // Bot�o de intera��o
    bool pressedButtonGrab = false; // Vari�vel para verificar se o bot�o foi pressionado
    public bool holdPressed, isHolding;

    private void Awake()
    {
        parent = GetComponent<ParentObjectReference>();
        //detectionObjects = GetComponent<DetectionObjects>();
    }
    // Update is called once per frame
    void Update()
    {
        GetPressedButtonHold();
        GrabObject();
    }

    void GrabObject()
    {
        if (holdPressed && !isHolding && parent.detectionObjects.isCollidingItem) // Verifica se o bot�o foi pressionado e n�o est� segurando o obj
        {
            // Pegar
            parent.inventory.ColetarItem(parent.detectionObjects.item);

            parent.dropTheObject.enabled = true;

            parent.detectionObjects.isCollidingItem = false;
            isHolding = true; // Define que o objeto est� sendo segurado

            Debug.Log("Pegou Objeto");
            Debug.Log( "Grab:"+ parent.grabTheObject.enabled + "Use:"+ parent.useTheObject.enabled + "Drop:" + parent.dropTheObject.enabled);
        }
        else if (holdPressed && isHolding && parent.detectionObjects.isCollidingItem){
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
