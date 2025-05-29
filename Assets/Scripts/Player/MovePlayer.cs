using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovePlayer : MonoBehaviour
{
    
    public static event Action OnLifeLost;
    //GameObjects do player
    ControllerPlayer controllerPlayer; //Referencia do controller do jogador
    //public MyButton crunchBtn; //Botão de agachar
    //public JoyRoots moveJoy; //Joystick de movimento
   
    private Transform myCamera;  //Referencia da câmera
    [SerializeField]private Transform View; //Referencia da visao do jogador

    //Classe
    public Rigidbody rb;
    public Animator anim; //Referencia do animator do jogador
    CapsuleCollider playerCollider; //Referencia do capsule collider do jogador

    //CONTROLE DO JOGADOR
        //movimento do jogador
    [SerializeField] private float velocity, moveH, moveV; //velocidade do jogador e input horizontal e vertical
    Vector3 dir; //direção do movimento
    [SerializeField] private float originalSpeed; //velocidade original do jogador
        //Agachamento do jogador
    [SerializeField] private float crounchVelocity; //multiplicador de velocidade para o botão de correr
    bool isCrounching = false, crounchPressed; //verifica se o jogador está agachado ou não
    bool pressedButton = false;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>(); //pega o animator do jogador
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>(); //pega o capsule collider do jogador
        controllerPlayer = GetComponent<ControllerPlayer>(); //pega o controller do jogador

        myCamera = Camera.main.transform;
    }

    void Start()
    {
        originalSpeed = velocity; //salva a velocidade original do jogador

        controllerPlayer.spawnPoint = transform.localPosition; //salva o ponto de spawn do jogador
        controllerPlayer.PlayerHealth = 3; //salva a vida do jogador
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
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
       
        if (dir != Vector3.zero)
        {
            anim.SetBool("isWalking", true);
            dir = transform.TransformDirection(dir); 
            rb.velocity = new Vector3(dir.x * velocity * Time.deltaTime, rb.velocity.y, dir.z * velocity * Time.deltaTime);
            //transform.LookAt(transform.position + dir);
            anim.SetFloat("Blend", 1);
        }
        else
        {
            // Se não houver movimento, mantém a velocidade vertical
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            anim.SetBool("isWalking", false);
        }
    }

    void Crunch(){
        //Verifica se o botão de agachar foi pressionado e se o jogador não está agachado
        if (crounchPressed && !isCrounching) {
            anim.SetBool("isCrounch", true);

            playerCollider.height = 1.39f; //muda a altura do capsule collider para o tamanho do jogador agachado
            playerCollider.radius = 0.45f; //muda o raio do capsule collider para o tamanho do jogador agachado
            playerCollider.center = new Vector3(0, 0.695f, 0); //muda o centro do capsule collider para o meio do jogador
 
            View.localPosition = new Vector3(0, 0.957f, 0.594f); //muda a posição da câmera para o meio do jogador

            velocity = crounchVelocity;
            
            crounchPressed = false; // Reseta o estado do botão de agachar
            isCrounching = true; 
        }
        //Verifica se o botão de agachar foi pressionado e se o jogador está agachado
        else if (crounchPressed && isCrounching) {
            anim.SetBool("isCrounch", false); //desativa a animação de agachar

            playerCollider.height = 2f; //muda a altura do capsule collider para o tamanho do jogador agachado
            playerCollider.radius = 0.2f; //muda o raio do capsule collider para o tamanho do jogador agachado
            playerCollider.center = new Vector3(0, 1f, 0f);

            View.localPosition = new Vector3(0, 1.431f, 0.35f);

            velocity = originalSpeed;

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

    IEnumerator WaitForSpawn()
    {
        controllerPlayer.blackPainel.SetActive(true); //ativa o painel preto
        controllerPlayer.PlayerHealth -= 1; //diminui a vida do jogador

        // Disparar o evento para as janelas ligarem a grade
        OnLifeLost?.Invoke();

        print("Player Life: " + controllerPlayer.PlayerHealth); //imprime a vida do jogador
        transform.localPosition = controllerPlayer.spawnPoint;

        yield return new WaitForSeconds(6f);

        controllerPlayer.blackPainel.SetActive(false); //ativa o painel pretos
    }

    void OnTriggerEnter(Collider collision) //verifica se o jogador colidiu com algo
    {
        if (collision.CompareTag("LocalHide")) //verifica se o objeto colidido tem a tag "Esconderijo"
        {
            gameObject.layer = LayerMask.NameToLayer("Hide"); //define a layer do jogador como "Hide"
        }

        if (collision.CompareTag("Enemy")) //verifica se o objeto colidido tem a tag "Enemy"
        {
            FindAnyObjectByType<PatraoController>().ContinueGame();
            
            StartCoroutine(WaitForSpawn()); //chama a coroutine para esperar 3 segundos
            print("Touch Enemy");
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("LocalHide")) //verifica se o objeto colidido tem a tag "Esconderijo"
        {
            gameObject.layer = LayerMask.NameToLayer("Player"); //define a layer do jogador como "Player"
        }
    }
}
