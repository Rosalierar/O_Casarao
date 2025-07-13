using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class NetFinalController : MonoBehaviour
{
    NetworkObject networkObject;
    [SerializeField] SongsController song;
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject[] player;
    Animator animCam;

    GameObject objCam;
    Camera camMain;
    [SerializeField] bool isOpenDoor = false;
    [SerializeField] NetDoorMoviment doorMovimentSecond;
    [SerializeField] NetDoorMoviment doorMoviment;
    [SerializeField] private GameObject[] cadeado = new GameObject[3];

    void Start()
    {
        networkObject = GetComponent<NetworkObject>();

        camMain = GameObject.FindWithTag("Cam").GetComponent<Camera>();
        animCam = GameObject.FindWithTag("Cam").GetComponent<Animator>();

        song = FindAnyObjectByType<SongsController>();
        enemy = FindObjectOfType<NetPatraoController>().gameObject;

        camMain.enabled = false;
        animCam.enabled = false;

        print("FINAL CAM PEGOU OS COMPONENTES");
    }

    void Update()
    {
        try
        {
            if (!cadeado[0].activeSelf && !cadeado[1].activeSelf && !cadeado[2].activeSelf && !isOpenDoor)
            {
                for (int i = 0; i < song.audioSorceBackGround.Length; i++)
                {
                    if (song.songsBackGround[i] != null && i != 3)
                    {
                        song.audioSorceBackGround[i].Stop();
                    }
                    else
                    {
                        song.audioSorceBackGround[3].clip = song.songsBackGround[4];
                        song.audioSorceBackGround[3].Play();
                        song.audioSorceBackGround[0].Play();
                    }
                }

                isOpenDoor = true;

                if (animCam == null)
                    print("animCam é null");

                for (int i = 0; i < cadeado.Length; i++)
                {
                    Destroy(cadeado[i]);
                }

                animCam.enabled = true;
                //player[0].SetActive(false);
                DesableCams();

                enemy.SetActive(false);

                print("FINAL CONTROLLER");
            }
        }
        catch
        {
            print("Todos os Cadeados ja foram Destruidos");
        }

        Animator anim = camMain.GetComponent<Animator>();
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("SeeAround") && stateInfo.normalizedTime >= 1f)
        {
            SceneManager.LoadScene(6);
        }
    }

    public void DesableCams()
    {
        GetComponent<BoxCollider>().enabled = true;


        GameObject camObjects = GameObject.FindGameObjectWithTag("MainCamera");

        Camera cam = camObjects.GetComponent<Camera>();

        if (cam != null)
        {
            cam.enabled = false; // desativa a câmera
        }

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        NetworkObject[] playersNetwork = new NetworkObject[players.Length];

        for (int i = 0; i < players.Length; i++)
        {
            playersNetwork[i] = players[i].GetComponentInParent<NetworkObject>();
        }

        camMain.enabled = true;

        GameObject canvaObjects = GameObject.FindGameObjectWithTag("Canva");

        GameObject canva = canvaObjects.GetComponent<GameObject>();

        if (canva != null)
        {
            canva.SetActive(false); // desativa os Canvas
        }

        doorMoviment.enabled = true;
        doorMovimentSecond.enabled = true;

        if (networkObject.HasStateAuthority)
        {
            doorMoviment.TryActiveDoor();
            doorMovimentSecond.TryActiveDoor();
        }
        else if (!networkObject.HasStateAuthority)
        {
            doorMoviment.Rpc_RequestToggleDoor();
            doorMovimentSecond.Rpc_RequestToggleDoor();
        }

        NavMeshAgent agentEnemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<NetPatraoController>().Agent();
        agentEnemy.isStopped = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PortaEntrada"))
        {
            NetworkRunner runner = FindObjectOfType<NetworkRunner>();
            StartGameMultiplayer(runner);
        }
    }
    
    private async void StartGameMultiplayer(NetworkRunner runner)
    {
        await runner.LoadScene(SceneRef.FromIndex(5));
    }
}
