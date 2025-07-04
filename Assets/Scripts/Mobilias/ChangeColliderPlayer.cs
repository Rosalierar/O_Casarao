using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeColliderPlayer : MonoBehaviour
{
    [SerializeField] bool isFirstFloor;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Posição do outro objeto no sistema local deste objeto
            Vector3 localPos = transform.InverseTransformPoint(other.transform.position);
            print("enrtou localPos: " + localPos);

            // Se saiu pela frente (Z positivo local)
            if (localPos.z > 0f && !isFirstFloor)  // Entrou na Escada pelo porao
            {
                Debug.Log("enrtou por trás (x- <)  // Entrou na Escada pelo porao");
            }
            else if (localPos.z < 0f  && !isFirstFloor) // Saiu da Escada e está no porao
            {
                Debug.Log("enrtou pela frente (x- >) Saiu da Escada e está no porao");
            }

            if (isFirstFloor) // Em Cima
            {
                print("enrtou no collider do 1 andar");
            }
        }
    }
}
