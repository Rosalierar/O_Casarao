using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausaJogo : MonoBehaviour
{
    [SerializeField]GameObject Conf, Tuto;
    void Update()
    {
        if (Conf.activeSelf || Tuto.activeSelf)
        {
            Time.timeScale = 0f;
        }else {
            Time.timeScale = 1f;
        }
    }
}
