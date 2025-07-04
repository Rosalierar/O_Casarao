using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeColliderPlayer : MonoBehaviour
{
    [SerializeField] bool isFirstFloor;

    void OnTriggerEnter(Collider other)
    {
        MovePlayer player = null;
        NetMovePlayer playerNet = null;

        try
        {
            player = other.GetComponent<MovePlayer>();
        }
        catch
        {
            Debug.Log("Player não está no multiplayer");
        }

        try
        {
            playerNet = other.GetComponent<NetMovePlayer>();
        }
        catch
        {
            Debug.Log("Player está no multiplayer");
        }
        if (other.CompareTag("Player") && (player != null || playerNet != null))
        {
            // Posição do outro objeto no sistema local deste objeto
            Vector3 localPos = transform.InverseTransformPoint(other.transform.position);

            // Se saiu pela frente (Z positivo local)
            if (localPos.z > 0f && !isFirstFloor)  // Entrou na Escada pelo porao
            {
                if (player)
                {
                    player.playerCollider 
                }
                else if (playerNet)
                {

                }

                Debug.Log("enrtou por trás (x- <)  // Entrou na Escada pelo porao");
            }
            else if (localPos.z < 0f && !isFirstFloor) // Saiu da Escada e está no porao
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
