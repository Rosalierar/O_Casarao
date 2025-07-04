using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;

public class ChangeColliderPlayer : MonoBehaviour
{
    [SerializeField] bool isFirstFloor;
  
    void OnTriggerExit(Collider other)
    {
        Debug.Log( isFirstFloor + " Trigger ativado em enr: " + gameObject.name);

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
            if (localPos.z < 0f && !isFirstFloor)  // Saiu da Escada e está no porao
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

                    playerNet.ch.height = 1.56f; //muda a altura do capsule collider para o tamanho do jogador agachado
                    playerNet.ch.radius = 0.57f; //muda o raio do capsule collider para o tamanho do jogador agachado
                    playerNet.ch.center = new Vector3(0.1f, 0.79f, 0.41f); //muda o centro do capsule collider para o meio do jogador
                }

                Debug.Log("enr pela frente (x- >) Saiu da Escada e está no porao");
            }
            else if (localPos.z > 0f && !isFirstFloor) // Entrou na Escada pelo porao 
            {
                if (player)
                {
                    player.playerCollider.height = 2f; //muda a altura do capsule collider para o tamanho do jogador agachado
                    player.playerCollider.radius = 0.35f; //muda o raio do capsule collider para o tamanho do jogador agachado
                    player.playerCollider.center = new Vector3(-0.07f, 1f, 0.129f); //muda o centro do capsule collider para o meio do jogador
                }
                else if (playerNet)
                {
                    playerNet.playerCollider.height = 2f; //muda a altura do capsule collider para o tamanho do jogador agachado
                    playerNet.playerCollider.radius = 0.35f; //muda o raio do capsule collider para o tamanho do jogador agachado
                    playerNet.playerCollider.center = new Vector3(-0.07f, 1f, 0.129f); //muda o centro do capsule collider para o meio do jogador
                    
                    playerNet.ch.height = 2f; //muda a altura do capsule collider para o tamanho do jogador agachado
                    playerNet.ch.radius = 0.35f; //muda o raio do capsule collider para o tamanho do jogador agachado
                    playerNet.ch.center = new Vector3(-0.07f,1f,0.129f); //muda o centro do capsule collider para o meio do jogador
                }

                Debug.Log("enr por trás (x- <)  // Entrou na Escada pelo porao");
            }

            else if (localPos.z > 0f && isFirstFloor)  // Saiu na Escada e está no 1 andar
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

                    playerNet.ch.height = 1.56f; //muda a altura do capsule collider para o tamanho do jogador agachado
                    playerNet.ch.radius = 0.57f; //muda o raio do capsule collider para o tamanho do jogador agachado
                    playerNet.ch.center = new Vector3(0.1f, 0.79f, 0.41f); //muda o centro do capsule collider para o meio do jogador
                }

                Debug.Log("enr por trás (x+ >)  // Saiu na Escada e está no 1 andar"); 
            }
            else if (localPos.z < 0f && isFirstFloor)  // Entrou na Escada pelo 1 andar
            {
                if (player)
                {
                    player.playerCollider.height = 2f; //muda a altura do capsule collider para o tamanho do jogador agachado
                    player.playerCollider.radius = 0.35f; //muda o raio do capsule collider para o tamanho do jogador agachado
                    player.playerCollider.center = new Vector3(-0.07f, 1f, 0.129f); //muda o centro do capsule collider para o meio do jogador
                }
                else if (playerNet)
                {
                    playerNet.playerCollider.height = 2f; //muda a altura do capsule collider para o tamanho do jogador agachado
                    playerNet.playerCollider.radius = 0.35f; //muda o raio do capsule collider para o tamanho do jogador agachado
                    playerNet.playerCollider.center = new Vector3(-0.07f, 1f, 0.129f); //muda o centro do capsule collider para o meio do jogador
                    
                    playerNet.ch.height = 2f; //muda a altura do capsule collider para o tamanho do jogador agachado
                    playerNet.ch.radius = 0.35f; //muda o raio do capsule collider para o tamanho do jogador agachado
                    playerNet.ch.center = new Vector3(-0.07f,1f,0.129f); //muda o centro do capsule collider para o meio do jogador
                }
                
                Debug.Log("enr por trás (x+ <)  // Entrou na Escada pelo 1 andar");  
            }
        }
    }
}
