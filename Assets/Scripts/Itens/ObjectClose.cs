using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClose : MonoBehaviour
{
    DetectionObjects detectionObjects; // Referência ao script DetectionObjects
    BoxCollider boxColliderObject; // Referência ao BoxCollider do objeto

    public bool isCloseTheObj = false; // Variável para verificar se o objeto está próximo

    // Start is called before the first frame update
    void Awake()
    {
        boxColliderObject = GetComponent<BoxCollider>(); // Obtém o BoxCollider do objeto
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) // Verifica se o objeto que colidiu tem a tag "Player"
        {
            isCloseTheObj = true; // Define que o objeto está próximo
            detectionObjects = other.GetComponentInChildren<DetectionObjects>(); // Obtém o script DetectionObjects do objeto colidido
            detectionObjects.enabled = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica se o objeto que saiu da colisão tem a tag "Player"
        {
            isCloseTheObj = false; // Define que o objeto não está mais próximo
            detectionObjects.enabled = false;
        }
    }
}
