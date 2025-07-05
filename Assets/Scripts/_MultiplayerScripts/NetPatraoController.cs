using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NetPatraoController : NetworkBehaviour
{
    NetworkObject networkObject;
    byte dificulty;
    [SerializeField] SongsController songs;
    bool isPlaySongPersecution= false;
    Animator animPatrao;
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

    private Quaternion startRot, openRotRight, openRotLeft;

    /// <summary>
    /// Patrulha
    /// </summary>

    [SerializeField] bool isPatrol = true; // Verifica se o patrão está patrulhando
    public Transform[] patrolPointsObjects;
    [SerializeField] Vector3[] patrolPoints; // Array de pontos de patrulha
    int currentPatrolIndex = 0; // Índice do ponto de patrulha atual
    int randomPatrolIndex = 0; // Índice aleatório para patrulha
    [Networked] public int PatrolIndex { get; set; }
    
    /// <summary>
    /// RayCast do Patrao
    /// </summary>

    [SerializeField] private LayerMask layerPlayer;
    [SerializeField] private Transform visionPos;
    Ray ray;
    RaycastHit patraoHit;
    [SerializeField] float[] rayAngles = { 0f, 25f, -25f, 45f, -45f, 90f, -90f, 180f, -180f };// Ângulos em graus para disparar os raios
    [SerializeField] float distanceRayPatrao;

    void Start()
    {
        networkObject = GetComponentInParent<NetworkObject>();

        print("PATRAO PODE TER HAS STATE: " + networkObject.HasStateAuthority);

        if (networkObject.HasStateAuthority)
        {
            dificulty = (byte)PlayerPrefs.GetInt("Dificulty");

            patrolPoints = new Vector3[patrolPointsObjects.Length - ValuePointPatrolsDelete()];

            print("Fase: " + (byte)PlayerPrefs.GetInt("Dificulty") + "Total Pontos de Patrulha Pontos: " + patrolPoints.Length);

            for (int i = 0; i < patrolPoints.Length; i++)
            {
                patrolPoints[i] = patrolPointsObjects[i].position;
            }

            // Destruir os GameObjects 
            foreach (Transform t in patrolPointsObjects)
            {
                Destroy(t.gameObject);
            }

            animPatrao = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>(); // Obtém o componente NavMeshAgent do objeto
            StartCoroutine(TimerPatrol()); // Inicia a patrulha
        }
    }

    public NavMeshAgent Agent()
    {
        return agent;
    }

    public override void FixedUpdateNetwork()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (!networkObject.HasStateAuthority) return;

        print("RODANDO NO NETFIXED");

        PatraoVision();
        Patrolling();

        try
        {
            if (FindObjectOfType<ControllerPlayer>().blackPainel.activeSelf)
            {
                animPatrao.SetBool("isWalking", false);
                animPatrao.SetBool("isRunning", false);

                agent.isStopped = true; // Para o agente NavMesh
                isPatrol = false; // Define que o patrão não está patrulhando
            }
        }
        catch
        {
            print("ERRO AO PEGAR BLACKPAINEL");
        }
    }
    void OnDrawGizmos()
    {
        if (!networkObject.HasStateAuthority) return;

        if (patrolPoints == null) return;

        Gizmos.color = Color.green;
        foreach (Vector3 point in patrolPoints)
        {
            Gizmos.DrawSphere(point, 0.2f);
        }
    }

    private float ValueSpeedPatrol()
    {
        float speedPl;

        if (dificulty == 3)
        {
            speedPl = 1.7f;
        }
        else if (dificulty == 2)
        {
            speedPl = 1.5f;
        }
        else
        {
            speedPl = 1.1f;
        }
        return speedPl;
    }

    private float ValueSpeedPersecution()
    {
        float speedPn;

        if (dificulty == 3)
        {
            speedPn = 2.4f;
        }
        else if (dificulty == 2)
        {
            speedPn = 2.1f;
        }
        else
        {
            speedPn = 1.8f;
        }
        return speedPn;
    }
    private int ValueTimePn()
    {
        int time;

        if (dificulty == 3)
        {
            time = 30;
        }
        else if (dificulty == 2)
        {
            time = 20;
        }
        else
        {
            time = 10;
        }
        return time;
    }
    private byte ValuePointPatrolsDelete()
    {
        byte pointPatrolsForDelete;

        if (dificulty == 3)
        {
            pointPatrolsForDelete = 0;
        }
        else if (dificulty == 2)
        {
            pointPatrolsForDelete = 8;
        }
        else
        {
            pointPatrolsForDelete = 12;
        }

        return pointPatrolsForDelete;
    }


    #region RayCast
    public void ContinueGame()
    {
        agent.speed = ValueSpeedPatrol();
        animPatrao.SetBool("isWalking", false);
        animPatrao.SetBool("isRunning", false);

        agent.isStopped = true;

        print("Continue Game foi chamado" + agent.isStopped);
        isPatrol = true; // Define que o patrão está patrulhando
        isRotate = false;
        isWalking = false;

        agent.Warp(new Vector3(-11.42f, 7.13f, -7.51f));

        StartCoroutine(TimerStopPersecution()); // Inicia a contagem para parar a perseguição
        //StartCoroutine(TimerPatrol()); // Inicia a patrulha
    }

    void PatraoVision()
    {
        RaycastHit patraoHit;
        bool SawPlayer = false;

        foreach (float angle in rayAngles)
        {
            Vector3 origin = visionPos.position + Vector3.up + transform.forward;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;

            Ray ray = new Ray(origin, direction);
            Debug.DrawRay(ray.origin, ray.direction * distanceRayPatrao, Color.magenta);

            if (Physics.Raycast(ray, out patraoHit, distanceRayPatrao, layerPlayer))
            {
                string tag = patraoHit.collider.tag;

                if (tag == "Player")
                {
                    SawPlayer = true;

                    playerTransform = patraoHit.transform;

                    if (!isPlaySongPersecution)
                    {
                        for (int i = 0; i < songs.audioSorceBackGround.Length; i++)
                        {
                            if (songs.songsBackGround[i] != null && i != 2)
                                songs.audioSorceBackGround[i].Stop();
                            else
                                songs.audioSorceBackGround[i].Play();
                        }

                        isPlaySongPersecution = true;
                    }

                    animPatrao.SetBool("isRunning", true);
                    animPatrao.SetBool("isWalking", false);

                    agent.speed = ValueSpeedPersecution();
                    isPatrol = false;
                    isRotate = false;
                    seePlayer = true;

                    PersecutionPlayer();

                    Debug.Log("Patrão viu o jogador!");

                    if (patraoHit.distance <= 0.2f)
                    {
                        Debug.Log("Patrão pegou o jogador!");
                    }

                    return; // já viu o jogador
                }
                else if (tag == "Porta" && patraoHit.distance <= 0.8f)
                {
                    DoorMoviment doorMoviment = patraoHit.collider.GetComponentInParent<DoorMoviment>();
                    InteractiveObject interactivedoor = doorMoviment.GetComponentInChildren<InteractiveObject>();

                    if (!doorMoviment.isOpen && interactivedoor.unlocked)
                        doorMoviment.TryActiveDoor();
                }
            }
        }

        if (!SawPlayer)
        {
            // Se chegou aqui, não viu o jogador com nenhum raio
            if (seePlayer)
            {
                seePlayer = false;
                StartCoroutine(TimerStopPersecution());
            }
            else if (!agent.isStopped && !isPatrol && playerTransform != null)
            {
                PersecutionPlayer();
            }

            Debug.Log("Patrão não viu o jogador!");
        }
    }
    #endregion RayCast

    #region Perseguicao
    void PersecutionPlayer()
    {
        if (!isPatrol)
        {
            print("player: " + playerTransform.position);

            agent.transform.LookAt(playerTransform.position); // Faz o patrão olhar para o jogador
            agent.SetDestination(player.position); // Define a posição de destino do agente como a posição do jogador

            //isPatrol = false; // Define que o patrão não está patrulhando

            print("Peserguindo");
        }
    }

    IEnumerator TimerStopPersecution()
    {
        //StopCoroutine(ContinueGame());

        print("Contagem iniciada para parar a perseguição pois parou de ver o player");

        if (seePlayer) yield break; // Se o patrão viu o jogador, sai do método

        yield return new WaitForSeconds(ValueTimePn()); // Aguarda 2 segundos antes de parar a perseguição

        animPatrao.SetBool("isWalking", false);
        animPatrao.SetBool("isRunning", false);
        
        agent.isStopped = true; // Para o agente NavMesh
        isPatrol = true; // Define que o patrão está patrulhando

        if (isPlaySongPersecution)
        {
            for (int i = 0; i < songs.audioSorceBackGround.Length; i++)
            {
                if (songs.songsBackGround[i] != null && i < 2)
                {
                    songs.audioSorceBackGround[i].Play();
                }
                else
                {
                    songs.audioSorceBackGround[i].Stop();
                }
            }
            isPlaySongPersecution = false;
        }

        StartCoroutine(TimerPatrol()); // Inicia a patrulha
    }
    #endregion Perseguicao

    #region Patrulha
    IEnumerator TimerPatrol()
    {
        print("Aguandando um pouco para comecar a patrulhar");

        yield return new WaitForSeconds(2f); // Aguarda 2 segundos antes de iniciar a patrulha

        agent.transform.LookAt(patrolPoints[currentPatrolIndex]); // Faz o patrão olhar para o ponto de patrulha atual

        animPatrao.SetBool("isWalking", true);
        animPatrao.SetBool("isRunning", false);

        isWalking = true; // Define que o patrão está andando
        agent.isStopped = false; // Volta a ativa o agente NavMesh
        agent.speed = ValueSpeedPatrol(); // Define a velocidade do agente NavMesh
        GoToPatrol(); // Chama o método para ir para o ponto de patrulha
    }

    void GoToPatrol()
    {
        if (isPatrol) // Verifica se o patrão está patrulhando
        {
            print("Sorteando Posicao");

            // Escolhe um waypoint aleatório
            randomPatrolIndex = Random.Range(0, patrolPoints.Length);
            PatrolIndex = randomPatrolIndex

            while (randomPatrolIndex == currentPatrolIndex && patrolPoints.Length > 1)
            {
                // Gera um índice aleatório diferente do índice atual
                randomPatrolIndex = Random.Range(0, patrolPoints.Length);
            }

            currentPatrolIndex = randomPatrolIndex;

            Patrolling(); // Inicia a patrulha
        }
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
    #endregion Patrulha

    #region Rotacao da Patrulha
    void StopAndRotate()
    {
        if (seePlayer) return; // Se o patrão viu o jogador, não faz nada

        if (isPatrol && !isRotate && isWalking) // Verifica se o patrão está patrulhando
        {
            print("Parou Para Olhar para os lADOS");

            animPatrao.SetBool("isWalking", false);
            animPatrao.SetBool("isRunning", false);

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
        //print("rOTACIONANDO");

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

        animPatrao.SetBool("isWalking", true);
        animPatrao.SetBool("isRunning", false);

        agent.isStopped = false; // Volta a ativa o agente NavMesh
        isRotate = false;
        print("Rotaciao Finalizada");
        StartCoroutine(TimerPatrol()); // Inicia a patrulha
    }

    #endregion Rotacao da Patrulha

}
