using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeColliderPlayer : MonoBehaviour
{
    [SerializeField] bool isDownStairs;

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Posição do outro objeto no sistema local deste objeto
            Vector3 localPos = transform.InverseTransformPoint(other.transform.position);

            // Se saiu pela frente (Z positivo local)
            if (localPos.z < 0f && !isDownStairs) // Saiu da Escada e está no porao
            {
                Debug.Log("enrtou pela frente (x- >) Saiu da Escada e está no porao");
            }
            else if (localPos.z > 0f && !isDownStairs)  // Entrou na Escada pelo porao?
            {
                Debug.Log("enrtou por trás (x- <)  // Entrou na Escada pelo porao?"); 
            }
            else if (localPos.z < 0f && isDownStairs)
            {
                Debug.Log("enrtou por trás (x+ <)  // Entrou na Escada pelo 1 andar");  // Entrou na Escada pelo 1 andar?
            }
            else if (localPos.z > 0f && isDownStairs)
            {
                Debug.Log("enrtou por trás (x+ >)  // Saiu na Escada e está no 1 andar"); // ntrou na Escada pelo porao?
            }
        }
    }
}
