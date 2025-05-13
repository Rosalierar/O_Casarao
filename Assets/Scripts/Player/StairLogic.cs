using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StairLogic : MonoBehaviour
{
    MovePlayer movePlayer; //Classe
    bool isGrounded = true;
    [SerializeField] LayerMask groundLayer; //Camada apenas da escada
    [SerializeField] float maxStepHight; //Altura máxima do degrau

    void Awake()
    {
        movePlayer = GetComponent<MovePlayer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {
        CheckStairs();
    }
    void CheckStairs(){
        if (isGrounded)
        {
           // Calcular a origem dos raios 0.5 unidades à frente do jogador
            Vector3 forwardOffset = transform.forward * 0.5f;
            Vector3 origin1 = transform.position + forwardOffset + Vector3.up * (maxStepHight + 0.2f);
        
            
            // Aplica rotação nas origens para os raios laterais (com ângulo de 35 graus)
            Vector3 origin2 = Quaternion.Euler(0, 35, 0) * (origin1 - transform.position) + transform.position;
            Vector3 origin3 = Quaternion.Euler(0, -35, 0) * (origin1 - transform.position) + transform.position;

            RaycastHit hit1, hit2, hit3;

            // Lançar raios nas direções certas a partir das novas origens calculadas
            Physics.Raycast(origin1, Vector3.down, out hit1, Mathf.Infinity, groundLayer);
            Physics.Raycast(origin2, Vector3.down, out hit2, Mathf.Infinity, groundLayer);
            Physics.Raycast(origin3, Vector3.down, out hit3, Mathf.Infinity, groundLayer);

            // Desenhar os raios para depuração
            Debug.DrawRay(origin1, Vector3.down * (maxStepHight + 0.2f), Color.red);
            Debug.DrawRay(origin2, Vector3.down * (maxStepHight + 0.2f), Color.red);
            Debug.DrawRay(origin3, Vector3.down * (maxStepHight + 0.2f), Color.red); 
            
            if (hit2.point.y > hit1.point.y)
                hit1 = hit2;
            if (hit3.point.y > hit1.point.y)
                hit1 = hit3;
            
            if(hit1.normal == Vector3.up && (hit1.point.y - transform.position.y) > .35f) //esse numero deve ser mudado para o mesmo tamanho colocado na altura de cada degrau
            {
                // Calcular a diferença de altura
                float heightDifference = hit1.point.y - transform.position.y - .1f;

                //movePlayer.rb.velocity = new Vector3(movePlayer.rb.velocity.x, heightDifference, movePlayer.rb.velocity.z);

                transform.position += Vector3.up * (hit1.point.y - transform.position.y + .1f);
                print("Subindo escada: " + (hit1.point.y - transform.position.y - .1f));
            }

            /*else if (hit1.normal == Vector3.up && (transform.position.y - hit1.point.y) > .35f)
            {
                // Calcular a diferença de altura negativa (descer)
                float heightDifference = transform.position.y - hit1.point.y - .1f;
                                           
                //movePlayer.rb.velocity = new Vector3(movePlayer.rb.velocity.x, -heightDifference, movePlayer.rb.velocity.z);

                transform.position += Vector3.down * (transform.position.y - hit1.point.y - .1f);
                print("Descendo escada: " + (transform.position.y - hit1.point.y - .1f));
            }**/
        } 
    }
}
