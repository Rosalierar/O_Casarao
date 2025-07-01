using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsController : MonoBehaviour
{
    [SerializeField] private SongsController audioMusic;
    [SerializeField] private Slider[] sliderVolume;

    [SerializeField] private GameObject tutorial;

    [SerializeField] private Slider sliderSensibility;

    [SerializeField] private GameObject painelOptions;

    public TextMeshProUGUI[] tmpConfig;
    string[] textEnConfig = { "Sensibility", "Language", "Portuguese", "English", "Tutorial", "Volume 1"};
    string[] textPtConfig = { "Sensibilidade", "Idioma", "Português", "Inglês", "Tutorial", "Volume 1" };

    [SerializeField] private CameraTouchController cameraTouchController;

    bool isOptionsOpen = false;

    void Start()
    {
        if (tutorial.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            
            Time.timeScale = 1f;
        }
    }

    public void ChangeVolume(int index)
    {
        if (!PlayerPrefs.HasKey("Volume"+index))
        {
            PlayerPrefs.SetInt("Volume"+index, 1);
        }
        
        audioMusic.audioSorceBackGround[index].volume = sliderVolume[index].value;

        PlayerPrefs.SetInt("Volume"+index, (int)audioMusic.audioSorceBackGround[index].volume);
    }

    public void ChangeSensibility()
    {
        cameraTouchController.cameraSensitivity = sliderSensibility.value;
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

    public void ChangeLanguage(int language)
    {
        switch (language)
        {
            case 0:
                Debug.Log("Language: Portuguese");
                PlayerPrefs.SetInt("Language", 0);

                for (int i = 0; i < tmpConfig.Length; i++)
                {
                    tmpConfig[i].text = textPtConfig[i];
                }
                break;
            case 1:
                Debug.Log("Language: English");
                PlayerPrefs.SetInt("Language", 1);

                for (int i = 0; i < tmpConfig.Length; i++)
                {
                    tmpConfig[i].text = textEnConfig[i];
                }

                break;
            default:
                Debug.Log("Language: Portuguese");

                PlayerPrefs.SetInt("Language", 0);

                for (int i = 0; i < tmpConfig.Length; i++)
                {
                    tmpConfig[i].text = textPtConfig[i];
                }
                break;
        }
    }

    public void OpenTutorial()
    {
        Time.timeScale = 0f;
        tutorial.SetActive(true);
    }

    public void CloseTutorial()
    {
        Time.timeScale = 1f;
        tutorial.SetActive(false);
    }
}
