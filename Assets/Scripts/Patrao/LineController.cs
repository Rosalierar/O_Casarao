using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LineController : MonoBehaviour
{
    [Header("Controller")]
    BoxCollider colMainDoor;
    private int language;

    [Header("GameObject for the text")]
    [SerializeField] private float timeForShow;
    [SerializeField] private bool isPatrão;
    [SerializeField] private TextMeshProUGUI[] textForShow;


    [Header("Write the Lines")]
    [TextArea]
    [SerializeField] private string[] totalLinePt;
    [TextArea]
    [SerializeField] private string[] totalLineEn;

    // Start is called before the first frame update
    void Start()
    {
        if (isPatrão)
            InvokeRepeating("RandomLine", 200, timeForShow);
    }

    void RandomLine()
    {
        byte i = (byte)Random.Range(0, totalLinePt.Length);

        StopCoroutine(ClearText(i));

        ShowTheLine(i);
    }

    void ShowTheLine(byte index)
    {
        language = PlayerPrefs.GetInt("Language");

        switch (language)
        {
            case 0:
                foreach (TextMeshProUGUI text4AllPlayerSee in textForShow)
                {
                    text4AllPlayerSee.text = totalLinePt[index];
                }
                break;

            case 1:
                foreach (TextMeshProUGUI text4AllPlayerSee in textForShow)
                {
                    text4AllPlayerSee.text = totalLineEn[index];
                }
                break;

            default:
                foreach (TextMeshProUGUI text4AllPlayerSee in textForShow)
                {
                    text4AllPlayerSee.text = totalLinePt[index];
                }
                break;
        }

        StartCoroutine(ClearText(index));
    }

    IEnumerator ClearText(int i)
    {
        yield return new WaitForSeconds(5f);

        foreach (TextMeshProUGUI text4AllPlayerSee in textForShow)
        {
            text4AllPlayerSee.text = "";
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isPatrão)
        {
            if (other.CompareTag("Player"))
            {
                // Posição do outro objeto no sistema local deste objeto
                Vector3 localPos = transform.InverseTransformPoint(other.transform.position);

                // Se saiu pela frente (Z positivo local)
                if (localPos.z > 0f)
                {
                    Debug.Log("enrtou pela frente (Z+)");
                }
                else
                {
                    Invoke("RandomLine", 5);
                    Debug.Log("enrtou por trás (Z-)");
                }
            }
        }
    }
}
