using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;
using UnityEngine.Playables;
using Unity.VisualScripting;
using Cinemachine;

public class NetMovePlayer : NetworkBehaviour
{
    [SerializeField] SongsController song;
    [SerializeField] PlayableDirector jumpscareDirector;
    NetworkObject networkObject;
    Camera camPlayer;
    [SerializeField] GameObject canva;

    public static event Action OnLifeLost;
    //GameObjects do player
    NetControllerPlayer controllerPlayer; //Referencia do controller do jogador
    //public MyButton crunchBtn; //Botão de agachar
    //public JoyRoots moveJoy; //Joystick de movimento
   
    private Transform myCamera;  //Referencia da câmera
    [SerializeField]private Transform View; //Referencia da visao do jogador

    //Classe
    public CharacterController ch;
    public Animator anim; //Referencia do animator do jogador
    public CapsuleCollider playerCollider; //Referencia do capsule collider do jogador

    //CONTROLE DO JOGADOR
    //Gravidade
    float gravity = -9.91f;
    Vector3 velocity;
    
    //movimento do jogador
    [SerializeField] private float playerSpeed, moveH, moveV; //velocidade do jogador e input horizontal e vertical
    Vector3 dir; //direção do movimento
    [SerializeField] private float originalSpeed; //velocidade original do jogador
        //Agachamento do jogador
    [SerializeField] private float crounchVelocity; //multiplicador de velocidade para o botão de correr
    bool isCrounching = false, crounchPressed; //verifica se o jogador está agachado ou não
    bool pressedButton = false;

    public override void Spawned()
    {
        networkObject = GetComponentInParent<NetworkObject>();

        if (networkObject.HasInputAuthority)
        {
            anim = GetComponent<Animator>(); //pega o animator do jogador
            ch = GetComponent<CharacterController>();
            playerCollider = GetComponent<CapsuleCollider>(); //pega o capsule collider do jogador
            controllerPlayer = GetComponent<NetControllerPlayer>(); //pega o controller do jogador

            StartController();

            originalSpeed = playerSpeed; //salva a velocidade original do jogador

            controllerPlayer.spawnPoint = transform.localPosition; //salva o ponto de spawn do jogador
            controllerPlayer.PlayerHealth = 3; //salva a vida do jogador

            print("PLAYER SPAWNED");
        }
    }
    private void StartController() // Procurar scripts e ativa-los
    {
        Transform raiz = transform.root;

        // Procura a câmera apenas dentro da prefab
        camPlayer = raiz.GetComponentInChildren<Camera>(true); // true = inclui objetos inativos
        camPlayer.gameObject.SetActive(true);
        camPlayer.enabled = true;
        myCamera = camPlayer.transform;
        print(myCamera);

        SongsController song = raiz.GetComponentInChildren<SongsController>(true);
        song.gameObject.SetActive(true);

        PlayableDirector director = raiz.GetComponentInChildren<PlayableDirector>(true);
        director.gameObject.SetActive(true);


        AudioListener audioListener = camPlayer.GetComponent<AudioListener>();
        audioListener.enabled = true;

        Transform canvas = raiz.Find("Canvas").GetComponentInChildren<Transform>(true);
        canva = canvas.gameObject;
        canva.SetActive(true);
    }

    Transform GetRaiz(Transform t)
    {
        while (t.parent != null)
            t = t.parent;
        return t;
    }

    void DesativarTodosScriptsDoPaiRaiz(GameObject objeto)
    {
        Transform raiz = GetRaiz(objeto.transform);
        MonoBehaviour[] scripts = raiz.GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!networkObject.HasInputAuthority) return;

        print("FIXEDNETWORK");
        velocity.y += gravity * Runner.DeltaTime;

        // Ajusta a rotação do jogador para alinhar com a rotação da câmera
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, myCamera.eulerAngles.y, transform.eulerAngles.z); 
        //Metodo de Movimentação
        MoveHorizontal();
        //Metodo de Agachamento
        Crunch();
        GetPressedButtonCrunch();
    }


    void MoveHorizontal(){
        moveH = controllerPlayer.moveJoy.inputDirection.x;
        moveV = controllerPlayer.moveJoy.inputDirection.y;

        dir = new Vector3(moveH, 0, moveV); 
        dir = transform.TransformDirection(dir); 

        Vector3 DirNormalized = dir.normalized * playerSpeed * Runner.DeltaTime;
        ch.Move(DirNormalized + velocity * Runner.DeltaTime);

        if (DirNormalized != Vector3.zero)
        {
            anim.SetBool("isWalking", true);

            //gameObject.transform.forward = DirNormalized;

            anim.SetFloat("Blend", 1);
        }
        else
        {
            // Se não houver movimento, mantém a velocidade vertical
            anim.SetBool("isWalking", false);
        }
    }

    void Crunch(){
        //Verifica se o botão de agachar foi pressionado e se o jogador não está agachado
        if (crounchPressed && !isCrounching) {
            anim.SetBool("isCrounch", true);

            playerCollider.height = 1.56f; //muda a altura do capsule collider para o tamanho do jogador agachado
            playerCollider.radius = 0.57f; //muda o raio do capsule collider para o tamanho do jogador agachado
            playerCollider.center = new Vector3(0.1f, 0.79f, 0.41f); //muda o centro do capsule collider para o meio do jogador

            ch.height = 1.56f;
            ch.radius = 0.57f; //muda o raio do capsule collider para o tamanho do jogador agachado
            ch.center = new Vector3(0.1f, 0.79f, 0.41f); //muda o centro do capsule collider para o meio do jogador
 
            View.localPosition = new Vector3(0.28f, 1.039f, 0.87f); //muda a posição da câmera para o meio do jogador

            playerSpeed = crounchVelocity;
            
            crounchPressed = false; // Reseta o estado do botão de agachar
            isCrounching = true; 
        }
        //Verifica se o botão de agachar foi pressionado e se o jogador está agachado
        else if (crounchPressed && isCrounching) {
            anim.SetBool("isCrounch", false); //desativa a animação de agachar

            playerCollider.height = 2f; //muda a altura do capsule collider para o tamanho do jogador agachado
            playerCollider.radius = 0.35f; //muda o raio do capsule collider para o tamanho do jogador agachado
            playerCollider.center = new Vector3(-0.07f, 1f, 0.129f);

            ch.height = 2f; //muda a altura do capsule collider para o tamanho do jogador agachado
            ch.radius = 0.35f; //muda o raio do capsule collider para o tamanho do jogador agachado
            ch.center = new Vector3(-0.07f, 1f, 0.129f);

            View.localPosition = new Vector3(-0.05497f,1.352f,0.451f); 

            playerSpeed = originalSpeed;

            crounchPressed = false; // Reseta o estado do botão de agachar
            isCrounching = false; 
        }
    }

    void GetPressedButtonCrunch() //verifica se o botão de agachar foi pressionado
    {
        if (controllerPlayer.crunchBtn.isPressed && !crounchPressed) //verifica se o botão de agachar foi pressionado
        {
            if (!pressedButton) //verifica se o botão não foi pressionado antes
            {
                pressedButton = true; //define que o botão foi pressionado
                crounchPressed = true; //define que o botão de agachar foi pressionado
            }
        }
        else 
        {
            pressedButton = false; //define que o botão não foi pressionado
        }
    }
    void CWACFS()
    {
        StartCoroutine(WaitForSpawn()); 
    }

    IEnumerator WaitForSpawn()
    {
        controllerPlayer.blackPainel.SetActive(true); //ativa o painel preto
        controllerPlayer.PlayerHealth -= 1; //diminui a vida do jogador

        // Disparar o evento para as janelas ligarem a grade
        OnLifeLost?.Invoke();

        ch.enabled = false;
        transform.position = controllerPlayer.spawnPoint;

        print("Player Life: " + controllerPlayer.PlayerHealth); //imprime a vida do jogador
        //transform.localPosition = controllerPlayer.spawnPoint;

        yield return new WaitForSeconds(6f);

        ch.enabled = true;
        controllerPlayer.blackPainel.SetActive(false); //ativa o painel pretos
    }
    private void RPC_AskPatraoToHandleHit(NetworkObject patraoNetObj, PlayerRef playerWhoHit)
    {
        var patraoScript = patraoNetObj.GetComponent<NetPatraoController>();
        if (patraoScript != null)
        {
            patraoScript.RPC_PlayerCaught(playerWhoHit);
        }
    }

    void OnTriggerEnter(Collider collision) //verifica se o jogador colidiu com algo
    {
        if (networkObject.HasInputAuthority)
        {
            if (collision.CompareTag("LocalHide")) //verifica se o objeto colidido tem a tag "Esconderijo"
                {
                    gameObject.layer = LayerMask.NameToLayer("Hide"); //define a layer do jogador como "Hide"

                    for (int i = 0; i < song.audioSorceBackGround.Length; i++)
                    {
                        if (song.songsBackGround[i] != null && i != 3)
                        {
                            song.audioSorceBackGround[i].Stop();
                        }
                        else
                        {
                            song.audioSorceBackGround[i].Play();
                        }
                    }
                }

            if (collision.CompareTag("Enemy") && gameObject.layer != LayerMask.NameToLayer("Hide")) //verifica se o objeto colidido tem a tag "Enemy"
            {
                NetworkObject patrao = collision.GetComponentInParent<NetworkObject>();

                if (patrao != null)
                {
                    // Envia pedido para o patrão tomar uma ação
                    if (patrao.HasStateAuthority)
                    {
                        patrao.GetComponentInChildren<NetPatraoController>().ContinueGame();
                    }
                    else
                    {
                        RPC_AskPatraoToHandleHit(patrao, Runner.LocalPlayer);
                    }
                }
                
                for (int i = 0; i < song.audioSorceBackGround.Length; i++)
                {
                    if (song.songsBackGround[i] != null && i != 3)
                    {
                        song.audioSorceBackGround[i].Stop();
                    }
                    else
                    {
                        song.audioSorceBackGround[i].Play();
                    }
                }

                canva.GetComponentInChildren<JoyRoots>().inputDirection = new Vector3(0,0,0);
                jumpscareDirector.Play();
                print("Touch Enemy");
            }
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (networkObject.HasInputAuthority)
        {
            if (collision.CompareTag("LocalHide")) //verifica se o objeto colidido tem a tag "Esconderijo"
            {
                gameObject.layer = LayerMask.NameToLayer("Player"); //define a layer do jogador como "Player"

                for (int i = 0; i < song.audioSorceBackGround.Length; i++)
                {
                    if (song.songsBackGround[i] != null && i < 2)
                    {
                        song.audioSorceBackGround[i].Play();
                    }
                    else
                    {
                        song.audioSorceBackGround[i].Stop();
                    }
                }
            }
        }
    }
}
