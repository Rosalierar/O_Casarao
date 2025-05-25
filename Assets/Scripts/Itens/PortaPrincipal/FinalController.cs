using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class FinalController : MonoBehaviour
{
    [SerializeField] GameObject player;
    Animator animCam;
    Camera camMain;
    bool isOpenDoor = false;
    [SerializeField] DoorMoviment doorMovimentSecond;
    [SerializeField] DoorMoviment doorMoviment;
    [SerializeField] bool[] conclusion = new bool[3];
    [SerializeField] private GameObject[] cadeado = new GameObject[3];

    void Awake()
    {
        camMain = GameObject.FindWithTag("Cam").GetComponent<Camera>();
        camMain.enabled = false;

        animCam = camMain.GetComponent<Animator>();
        animCam.enabled = false;

        //this.enabled = false;
    }

    void Update()
    {
        try
        {
            if (!cadeado[0].activeSelf && !cadeado[1].activeSelf && !cadeado[2].activeSelf && !isOpenDoor)
            {
                isOpenDoor = true;

                DesableCams();

                for (int i = 0; i < cadeado.Length; i++)
                {
                    Destroy(cadeado[i]);
                }
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

        NavMeshAgent agentEnemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<PatraoController>().Agent();
        agentEnemy.isStopped = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (animCam == null)
            print("animCam é null");

        animCam.enabled = true;
        player.SetActive(false);
    }
}
