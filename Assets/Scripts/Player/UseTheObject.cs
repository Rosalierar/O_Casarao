using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseTheObject : MonoBehaviour
{
    ParentObjectReference parent;
    DetectionObjects detectionObjects;

    [SerializeField] private MyButton useBtn; // Bot�o de intera��o
    bool pressedButtonUse = false; // Vari�vel para verificar se o bot�o foi pressionado
    bool usePressed;

    void Awake()
    {
        parent = GetComponent<ParentObjectReference>();
        //detectionObjects = GetComponent<DetectionObjects>();
    }

    // Update is called once per frame
    void Update()
    {
        if (parent.detectionObjects.isCollidingInteractiveObj)
        {
            GetPressedButtonUse();
            UseObject();
        }
    }
    void UseObject()
    {
        if (usePressed /*&& parent.grabTheObject.isHolding*/ && parent.detectionObjects.isCollidingInteractiveObj) // Verifica se o bot�o foi pressionaso foi pressionado e est� segurando o obj
        {
            // Usar
            parent.detectionObjects.interactiveObject.TentarInteragir();

            Debug.Log("Interagindo com o objeto: " + parent.detectionObjects.interactiveObject.gameObject.name); // Exibe o nome do objeto interagido no console
        }
        if (usePressed && parent.detectionObjects.interactiveObject.tipoDeObjeto == TipoDeItem.Porta)
        {
            print("Inragindo com porta");
            parent.detectionObjects.interactiveObject.TentarInteragir();
        }

        usePressed = false; // Define que o bot�o de agachar n�o foi pressionado
    }
    void GetPressedButtonUse() //verifica se o bot�o de agachar foi pressionado
    {
        if (useBtn.isPressed && !usePressed) //verifica se o bot�o de agachar foi pressionado
        {
            if (!pressedButtonUse) //verifica se o bot�o n�o foi pressionado antes
            {
                pressedButtonUse = true; //define que o bot�o foi pressionado
                usePressed = true; //define que o bot�o de agachar foi pressionado
            }
        }
        else
        {
            pressedButtonUse = false; //define que o bot�o n�o foi pressionado
        }
    }
}
