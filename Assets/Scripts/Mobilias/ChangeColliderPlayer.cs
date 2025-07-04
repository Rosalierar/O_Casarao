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
                    player.playerCollider.height = 1.56f; //muda a altura do capsule collider para o tamanho do jogador agachado
                    player.playerCollider.radius = 0.57f; //muda o raio do capsule collider para o tamanho do jogador agachado
                    player.playerCollider.center = new Vector3(0.1f, 0.79f, 0.41f); //muda o centro do capsule collider para o meio do jogador
                }
                else if (playerNet)
                {
                    playerNet.playerCollider.height = 1.56f; //muda a altura do capsule collider para o tamanho do jogador agachado
                    playerNet.playerCollider.radius = 0.57f; //muda o raio do capsule collider para o tamanho do jogador agachado
                    playerNet.playerCollider.center = new Vector3(0.1f, 0.79f, 0.41f); //muda o centro do capsule collider para o meio do jogador
                }

                Debug.Log("enrtou por trás (x- <)  // Entrou na Escada pelo porao");
            }
            else if (localPos.z < 0f && !isFirstFloor) // Saiu da Escada e está no porao
            {
                if (player)
                {
                    player.playerCollider.height = 2f; //muda a altura do capsule collider para o tamanho do jogador agachado
                    player.playerCollider.radius = 0.35f; //muda o raio do capsule collider para o tamanho do jogador agachado
                    player.playerCollider.radius = 0.35f; //muda o raio do capsule collider para o tamanho do jogador agachado
                }
                else if (playerNet)
                {
                    playerNet.playerCollider.height = 2f; //muda a altura do capsule collider para o tamanho do jogador agachado
                    playerNet.playerCollider.radius = 0.35f; //muda o raio do capsule collider para o tamanho do jogador agachado
                    playerNet.playerCollider.radius = 0.35f; //muda o raio do capsule collider para o tamanho do jogador agachado
                }

                Debug.Log("enrtou pela frente (x- >) Saiu da Escada e está no porao");
            }

            if (isFirstFloor) // Em Cima
            {
                if (player)
                {
                    player.playerCollider.height = 2f; //muda a altura do capsule collider para o tamanho do jogador agachado
                    player.playerCollider.radius = 0.35f; //muda o raio do capsule collider para o tamanho do jogador agachado
                    player.playerCollider.radius = 0.35f; //muda o raio do capsule collider para o tamanho do jogador agachado
                }
                else if (playerNet)
                {
                    playerNet.playerCollider.height = 2f; //muda a altura do capsule collider para o tamanho do jogador agachado
                    playerNet.playerCollider.radius = 0.35f; //muda o raio do capsule collider para o tamanho do jogador agachado
                    playerNet.playerCollider.radius = 0.35f; //muda o raio do capsule collider para o tamanho do jogador agachado
                }

                print("enrtou no collider do 1 andar");
            }
        }
    }
}
