using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampadaController : MonoBehaviour
{
    [SerializeField] Light light;

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
        light.range = FindObjectOfType<OptionsController>().sliderIluminação.value;
    }
}
