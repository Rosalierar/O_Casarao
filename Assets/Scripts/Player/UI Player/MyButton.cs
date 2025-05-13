using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyButton : MonoBehaviour
{
    public bool isPressed;
    
    // Start is called before the first frame update
    void Start()
    {
        SetUpButton();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetUpButton(){
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>(); //criando um evento trigger

        //nova entrada para o pointerdown
        var pointerDown = new EventTrigger.Entry(); //criando um evento de clique
        pointerDown.eventID = EventTriggerType.PointerDown; //definindo o evento como clique
        pointerDown.callback.AddListener((evenData) => onClickDown()); //adicionando o evento de clique

        //nova entrada para o pointerup
        var pointerUp = new EventTrigger.Entry(); //criando um evento de clique
        pointerUp.eventID = EventTriggerType.PointerUp; //definindo o evento como clique
        pointerUp.callback.AddListener((evenData) => onClickUp()); //adicionando o evento de clique

        //empurrar as mudanças
        trigger.triggers.Add(pointerDown); //adicionando o evento de clique
        trigger.triggers.Add(pointerUp); //adicionando o evento de clique
    }

    public void onClickDown()
    {
        isPressed = true;
        Debug.Log("Button Pressed");
    }

    public void onClickUp()
    {
        isPressed = false;
        Debug.Log("Button Released");
    }
}
