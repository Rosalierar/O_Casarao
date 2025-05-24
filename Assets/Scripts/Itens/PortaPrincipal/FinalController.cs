using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalController : MonoBehaviour
{
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

        //this.enabled = false;
    }

    void Update()
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
    }

    void OnTriggerEnter(Collider other)
    {
        
    }
}
