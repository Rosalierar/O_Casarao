using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class NetFinalController : MonoBehaviour
{
    [SerializeField] SongsController song;
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject player;
    Animator animCam;
    Camera camMain;
    [SerializeField] bool isOpenDoor = false;
    [SerializeField] NetDoorMoviment doorMovimentSecond;
    [SerializeField] NetDoorMoviment doorMoviment;
    [SerializeField] private GameObject[] cadeado = new GameObject[3];

    void Start()
    {
        camMain = GameObject.FindWithTag("Cam").GetComponent<Camera>();
        animCam = GameObject.FindWithTag("Cam").GetComponent<Animator>();
        song = FindObjectOfType<SongsController>();
        enemy = FindObjectOfType<NetPatraoController>().gameObject;

        camMain.enabled = false;
        animCam.enabled = false;
    }

    void Update()
    {
        try
        {
            if (!cadeado[0].activeSelf && !cadeado[1].activeSelf && !cadeado[2].activeSelf && !isOpenDoor)
            {
                camMain = GameObject.FindWithTag("Cam").GetComponent<Camera>();
                animCam = GameObject.FindWithTag("Cam").GetComponent<Animator>();
                song = FindObjectOfType<SongsController>();
                enemy = FindObjectOfType<NetPatraoController>().gameObject;

                camMain.enabled = false;
                animCam.enabled = false;

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
                player.SetActive(false);
                enemy.SetActive(false);

                DesableCams();

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

        GameObject[] camObjects = GameObject.FindGameObjectsWithTag("MainCamera");

        foreach (GameObject obj in camObjects)
        {
            Camera cam = obj.GetComponent<Camera>();
            if (cam != null)
            {
                cam.enabled = false; // desativa a câmera
            }
        }

        camMain.enabled = true;

        GameObject[] canvaObjects = GameObject.FindGameObjectsWithTag("Canva");

        foreach (GameObject obj in canvaObjects)
        {
            GameObject canva = obj.GetComponent<GameObject>();
            if (canva != null)
            {
                canva.SetActive(false); // desativa os Canvas
            }
        }

        doorMoviment.enabled = true;
        doorMoviment.TryActiveDoor();
        doorMovimentSecond.enabled = true;
        doorMovimentSecond.TryActiveDoor();

        NavMeshAgent agentEnemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<NetPatraoController>().Agent();
        agentEnemy.isStopped = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PortaEntrada"))
        {
            SceneManager.LoadScene(6);
        }
    }
}
