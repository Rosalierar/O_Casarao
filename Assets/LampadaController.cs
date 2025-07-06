using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampadaController : MonoBehaviour
{

    private void OnEnable()
    {
        OptionsController.OnLightRange += ChangeRangeLight;
    }

    private void OnDisable()
    {
        OptionsController.OnLightRange -= ChangeRangeLight;
    }

    void ChangeRangeLight()
    {
       
    }
}
