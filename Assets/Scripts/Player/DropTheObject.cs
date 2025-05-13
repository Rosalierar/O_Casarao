using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTheObject : MonoBehaviour
{   

    ParentObjectReference parent;
    DetectionObjects detectionObjects;

    [SerializeField] private MyButton dropBtn; // Refer�ncia ao bot�o de drop

    bool pressedButtonDrop = false; // Vari�vel para verificar se o bot�o foi pressionado
    public bool dropPressed, isDroping;

    private void Awake()
    {
        parent = GetComponent<ParentObjectReference>();
        detectionObjects = GetComponent<DetectionObjects>();
    }

    // Update is called once per frame
    void Update()
    {
        GetPressedButtonDrop();
        DropObjectt();
    }

    void DropObjectt()
    {
        if (dropPressed) // Verifica se o invent�rio tem itens
        {
            if (parent.inventory.itemCarregado != null) // Se nao tiver itens, define como nulo
            {
                parent.inventory.SoltarItem();

                parent.grabTheObject.isHolding = false;

                parent.grabTheObject.enabled = true;
                parent.useTheObject.enabled = false;
                parent.dropTheObject.enabled = false;

                Debug.Log("Item solto com sucesso.");
                Debug.Log("Grab:" + parent.grabTheObject.enabled + "Use:" + parent.useTheObject.enabled + "Drop:" + parent.dropTheObject.enabled);
            }
        
            else // Se não Tiver Itens
            {
                detectionObjects.item = null; // Se nao tiver itens, define como nulo
                Debug.Log("Seu Inventario esta vazio"); // Exibe no console que o itemé nulo

                Debug.Log("Grab:" + parent.grabTheObject.enabled + "Use:" + parent.useTheObject.enabled + "Drop:" + parent.dropTheObject.enabled);
            }
        }

        dropPressed = false;
    }

    void GetPressedButtonDrop() //verifica se o bot�o de agachar foi pressionado
    {
        if (dropBtn.isPressed && !dropPressed) //verifica se o bot�o de largar foi pressionado
        {
            if (!pressedButtonDrop) //verifica se o bot�o n�o foi pressionado antes
            {
                pressedButtonDrop = true; //define que o bot�o foi pressionado
                dropPressed = true; //define que o bot�o de agachar foi pressionado
            }
        }
        else
        {
            pressedButtonDrop = false; //define que o bot�o n�o foi pressionado
        }
    }
}
