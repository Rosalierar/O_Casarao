using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class PatraoController : MonoBehaviour
{
    NavMeshAgent agent; // Referência ao agente NavMesh

    /// <summary>
    /// Perseguição
    /// </summary>
    
    [SerializeField] private bool seePlayer = false; // Verifica se o patrão viu o jogador
    [SerializeField] float timerToStopPersecution;

    [SerializeField] private Transform player; // Referência ao jogador
    /// <summary>
    /// Rotacao de Patrulha
    /// </summary>
    
    public float rotationPatraoAngle = 90f;
    public float openSpeed = 5f;

    [SerializeField] Transform patraoTransform;
    Transform playerTransform;

    //[SerializeField] private bool isFinishRot = false;
    [SerializeField] private bool isRotate = false, isWalking;

    private Quaternion startRot, openRotRight,openRotLeft;

    /// <summary>
    /// Patrulha
    /// </summary>

    [SerializeField] bool isPatrol = true; // Verifica se o patrão está patrulhando
    public Transform[] patrolPointsObjects;
    [SerializeField] Vector3[] patrolPoints; // Array de pontos de patrulha
    int currentPatrolIndex = 0; // Índice do ponto de patrulha atual
    int randomPatrolIndex = 0; // Índice aleatório para patrulha

    /// Cozinha 1: 42, 0, 58
    /// Cozinha 2: 35, 0, 56.5
    /// Sala de Jantar1: 35, 0, 49 
    /// Sala de Jantar2: 39.5, 0, 54
    /// Entrada Principal: 32, 0, 46
    /// Sala: 
    /// QuartoP: 
    /// QuartoF: 
    /// QuartoJ: 
    /// QuartoV: 
    /// Banheiro:
    /// Sala de Estar:
    /// Sala de Convivio 2: 
    /// Escritorio: 52.5, 0, 55.5
    /// Porao1: 
    /// Porao2:
    /// Porao3:

    /// <summary>
    /// RayCast do Patrao
    /// </summary>

    [SerializeField] private LayerMask layerPlayer;
    [SerializeField] private Transform visionPos;
    Ray ray;
    RaycastHit patraoHit;
    [SerializeField] float distanceRayPatrao;

    // Start is called before the first frame update
    void Awake()
    {
        patrolPoints = new Vector3[patrolPointsObjects.Length];

        for (int i = 0; i < patrolPointsObjects.Length; i++)
        {
            patrolPoints[i] = patrolPointsObjects[i].position;
        }

        // Se quiser, pode até destruir os GameObjects agora
        foreach (Transform t in patrolPointsObjects)
        {
            Destroy(t.gameObject);
        }
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Obtém o componente NavMeshAgent do objeto

        StartCoroutine(TimerPatrol()); // Inicia a patrulha
    }

    // Update is called once per frame
    void Update()
    {
        PatraoVision();
        Patrolling();

        if (FindObjectOfType<ControllerPlayer>().blackPainel.activeSelf)
        {
            agent.isStopped = true; // Para o agente NavMesh
            isPatrol = false; // Define que o patrão não está patrulhando
        }
        /*else if (!FindObjectOfType<ControllerPlayer>().blackPainel.activeSelf)
        {
            agent.isStopped = false; // Ativa o agente NavMesh
            isPatrol = true; // Define que o patrão está patrulhando
        }*/
    }
    void OnDrawGizmos()
    {
        if (patrolPoints == null) return;

        Gizmos.color = Color.green;
        foreach (Vector3 point in patrolPoints)
        {
            Gizmos.DrawSphere(point, 0.2f);
        }
    }
    #region RayCast
    void PatraoVision()
    {
        ray = new Ray(visionPos.position, transform.forward); // Cria um raio a partir da posição do objeto em direção à frente

        Physics.Raycast(ray, out patraoHit, distanceRayPatrao, layerPlayer);
        Debug.DrawRay(visionPos.position, transform.forward * distanceRayPatrao, Color.red); // Desenha o raio na cena para visualização

        if (Physics.Raycast(ray, out patraoHit, distanceRayPatrao, layerPlayer))
        {
            if (patraoHit.collider.CompareTag("Player"))
            { // Verifica se o raio atingiu algo
                playerTransform = patraoHit.transform; // Atribui o objeto atingido à variável playerTransform

                agent.speed = 2.5f; // Define a velocidade do agente NavMesh
                isPatrol = false; // Define que o patrão não está patrulhando
                isRotate = false; // Define que o patrão não está rotacionando
                seePlayer = true; // Define que o patrão viu o jogador

                PersecutionPlayer(); // Chama o método de perseguição

                Debug.Log("Patrão viu o jogador!"); // Exibe mensagem de que o patrão viu o jogador

                if (patraoHit.distance <= 0.2)
                {
                    print("Patrão pegou o jogador!"); // Exibe mensagem de que o patrão pegou o jogador

                    StartCoroutine(ContinueGame()); // Inicia a contagem para continuar o jogo
                     // Para o agente NavMesh
                    //isPatrol = true; // Define que o patrão está patrulhando

                    //StartCoroutine(TimerPatrol()); // Inicia a patrulha

                    //MovePlayer movePlayer = patraoHit.collider.GetComponent<MovePlayer>(); // Obtém o componente MovePlayer do jogador
                    //////////  Retirar vida do jogador aqui ////////////
                    //////////  Iniciar a posicao do jogador do jogador aqui ////////////
                    //////////  Chamar script de controle de jogo para coisar as janelas aqui ////////////
                }
            }
            else if (seePlayer && !patraoHit.collider.CompareTag("Player")){
                  
                seePlayer = false; // Define que o patrão não viu o jogador
                StartCoroutine(TimerStopPersecution()); // Inicia a contagem para parar a perseguição
            }
            else if (!seePlayer && !agent.isStopped && !isPatrol)
            {
                PersecutionPlayer();
            }
        }
        else 
        {
            Debug.Log("Patrão não viu o jogador!"); // Exibe mensagem de que o patrão não viu o jogador
        }
    }

    IEnumerator ContinueGame()
    {
        agent.isStopped = true;

        yield return new WaitForSeconds(5f); // Aguarda 2 segundos antes de continuar o jogo

        agent.isStopped = false; // Ativa o agente NavMesh
        isPatrol = true; // Define que o patrão está patrulhando
        StartCoroutine(TimerPatrol()); // Inicia a patrulha
    }
    #endregion RayCast

    #region Perseguicao
    void PersecutionPlayer()
    {
        if (!isPatrol)
        {
            agent.transform.LookAt(playerTransform.position); // Faz o patrão olhar para o jogador
            agent.SetDestination(player.position); // Define a posição de destino do agente como a posição do jogador

            //isPatrol = false; // Define que o patrão não está patrulhando

            print("Peserguindo");
        }
    }

    IEnumerator TimerStopPersecution()
    {
        print("Contagem iniciada para parar a perseguição pois parou de ver o player");

        if (seePlayer) yield break; // Se o patrão viu o jogador, sai do método

        yield return new WaitForSeconds(timerToStopPersecution); // Aguarda 2 segundos antes de parar a perseguição

        agent.isStopped = true; // Para o agente NavMesh
        isPatrol = true; // Define que o patrão está patrulhando

        StartCoroutine(TimerPatrol()); // Inicia a patrulha
    }
    #endregion Perseguicao

    #region Patrulha
    IEnumerator TimerPatrol()
    {
        print("Aguandando um pouco para comecar a patrulhar");

        yield return new WaitForSeconds(2f); // Aguarda 2 segundos antes de iniciar a patrulha

        agent.transform.LookAt(patrolPoints[currentPatrolIndex]); // Faz o patrão olhar para o ponto de patrulha atual
        isWalking = true; // Define que o patrão está andando
        agent.isStopped = false; // Volta a ativa o agente NavMesh
        agent.speed = 1.5f; // Define a velocidade do agente NavMesh
        GoToPatrol(); // Chama o método para ir para o ponto de patrulha
    }

    void Patrolling()
    {
        if (isPatrol)
        {
            print("Indo par Novo ponto");

            agent.SetDestination(patrolPoints[currentPatrolIndex]); // Define o ponto de patrulha atual como destino do agent   

            if (Vector3.Distance(patraoTransform.position, patrolPoints[currentPatrolIndex]) <= 1f) // Verifica se o patrão chegou ao ponto de patrulha
            {
                print("Cheguei no Ponto");
                //currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length; // Atualiza o índice do ponto de patrulha atual
                StopAndRotate();
            }
        }
    }

    void GoToPatrol()
    {
        if (isPatrol) // Verifica se o patrão está patrulhando
        {
            print("Sorteando Posicao");

            // Escolhe um waypoint aleatório
            randomPatrolIndex = Random.Range(0, patrolPoints.Length);

            while (randomPatrolIndex == currentPatrolIndex && patrolPoints.Length > 1)
            {
                // Gera um índice aleatório diferente do índice atual
                randomPatrolIndex = Random.Range(0, patrolPoints.Length);
            }

            currentPatrolIndex = randomPatrolIndex;

            Patrolling(); // Inicia a patrulha
        }
    }
    #endregion Patrulha

    #region Rotacao da Patrulha
    void StopAndRotate()
    {
        if (seePlayer) return; // Se o patrão viu o jogador, não faz nada
        
        if (isPatrol && !isRotate && isWalking) // Verifica se o patrão está patrulhando
        {
            print("Parou Para Olhar para os lADOS");
            agent.isStopped = true; // Para o agente NavMesh
            isWalking = false; // Define que o patrão não está andando

            Rotacionar();
        }
    }

    void Rotacionar()
    {
        if (isRotate && !isWalking) return; // Se já estiver rotacionando, não faz nada

        startRot = patraoTransform.rotation;
        openRotRight = Quaternion.Euler(patraoTransform.eulerAngles + Vector3.up * rotationPatraoAngle);
        openRotLeft = Quaternion.Euler(patraoTransform.eulerAngles + Vector3.up * -rotationPatraoAngle);

        if (Vector3.Distance(patraoTransform.position, patrolPoints[currentPatrolIndex]) <= 1f)
            StartCoroutine(ToggleDoor());
    }

    private System.Collections.IEnumerator ToggleDoor()
    {
        print("rOTACIONANDO");

        isRotate = true;
        
        print("Rotacionando para a direita");
        float elapsed = 0f;
        // Rotaciona para a direita
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * openSpeed;
            patraoTransform.rotation = Quaternion.Slerp(startRot, openRotRight, elapsed);
            yield return null;
        }
        
        elapsed = 0f;
        print("Rotacionando para a Frente");
        // Rotaciona para a Frente
        elapsed = 0f;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * openSpeed;
            patraoTransform.rotation = Quaternion.Slerp(openRotRight, startRot, elapsed);
            yield return null;
        }

        print("Rotacionando para a Esquerda");
        elapsed = 0f;
        // Rotaciona para a Esquerda
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * openSpeed;
            patraoTransform.rotation = Quaternion.Slerp(startRot, openRotLeft, elapsed);
            yield return null;
        }

        print("Rotacionando para a Frente");
        elapsed = 0f;
        // Rotaciona para a Frente
        while (elapsed < 1f)
        {            
            elapsed += Time.deltaTime * openSpeed;
            patraoTransform.rotation = Quaternion.Slerp(openRotLeft, startRot, elapsed);
            yield return null;
        }


        agent.isStopped = false; // Volta a ativa o agente NavMesh
        isRotate = false;
        print("Rotaciao Finalizada");
        StartCoroutine(TimerPatrol()); // Inicia a patrulha
    }

    #endregion Rotacao da Patrulha

}

