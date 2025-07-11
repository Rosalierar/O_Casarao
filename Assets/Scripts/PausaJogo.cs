using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausaJogo : MonoBehaviour
{
    bool InConf = false;
    void Update()
    {
        if (InConf)
        {
            Time.timeScale = 0;
        }
    }
}
