using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsController : MonoBehaviour
{
    //[SerializeField] private Slider sliderSensibility;

    [SerializeField] private GameObject painelOptions;

    public TextMeshProUGUI[] tmpConfig;
    string[] textEnConfig = {"Sensibility", "Language", "Portuguese", "English", "Volume" };
    string[] textPtConfig = {"Sensibilidade", "Idioma", "Português", "Inglês", "Volume"};

    //[SerializeField] private CameraTouchController cameraTouchController;

    bool isOptionsOpen = false;

    public void ChangeSensibility()
    {
        //cameraTouchController.cameraSensitivity = sliderSensibility.value;
    }

    public void OptionController()
    {
        if (isOptionsOpen)
        {
            painelOptions.SetActive(false);
            isOptionsOpen = false;
        }
        else 
        {
            painelOptions.SetActive(true);
            isOptionsOpen = true;    
        }

    }
}
