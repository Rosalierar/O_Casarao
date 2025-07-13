using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;
using System;

public class OptionsController : MonoBehaviour
{
    public static event Action OnLightRange;
    LoobyManager lobby;
    [SerializeField] private SongsController audioMusic;
    [SerializeField] public Slider[] sliderVolume;

    [SerializeField] private GameObject tutorial;

    [SerializeField] private Slider sliderSensibility;
    public Slider sliderIluminação;

    [SerializeField] private GameObject painelOptions;

    public TextMeshProUGUI[] tmpConfig;
    string[] textEnConfig = { "Sensibility", "Language", "Portuguese", "English", "Tutorial", "Volume Rain" };
    string[] textPtConfig = { "Sensibilidade", "Idioma", "Português", "Inglês", "Tutorial", "Volume da Chuva" };

    [SerializeField] private CameraTouchController cameraTouchController;
    [SerializeField] private NetCamersTouchController netCameraTouchController;

    bool isOptionsOpen = false;

    void Start()
    {
        if (!PlayerPrefs.HasKey("Volume" + 0))
        {
            PlayerPrefs.SetFloat("Volume0", 0.015f);
            print("Saved=" + PlayerPrefs.GetFloat("Volume" + 0));
        }
        if (!PlayerPrefs.HasKey("Volume" + 1))
        {
            PlayerPrefs.SetFloat("Volume1", 0.006f);
            print("Saved=" + PlayerPrefs.GetFloat("Volume" + 1));
        }

        audioMusic.audioSorceBackGround[0].volume = PlayerPrefs.GetFloat("Volume0");
        audioMusic.audioSorceBackGround[1].volume = PlayerPrefs.GetFloat("Volume1");
        sliderVolume[0].value = PlayerPrefs.GetFloat("Volume0");
        sliderVolume[1].value = PlayerPrefs.GetFloat("Volume1");
        print("Slider=" + sliderVolume[0].value + " | Saved=" + PlayerPrefs.GetFloat("Volume" + 0));
        print("Slider=" + sliderVolume[1].value + " | Saved=" + PlayerPrefs.GetFloat("Volume" + 1));

        
    }

    public void ChangeVolume(int index)
    {
        audioMusic.audioSorceBackGround[index].volume = sliderVolume[index].value;

        PlayerPrefs.SetFloat("Volume" + index, audioMusic.audioSorceBackGround[index].volume);
        print("Volume" + index + ": Slider=" + sliderVolume[index].value + " | Saved=" + PlayerPrefs.GetFloat("Volume" + index));
        PlayerPrefs.Save();
    }

    public void ChangeSensibility()
    {
        if (cameraTouchController != null)
        {
            cameraTouchController.cameraSensitivity = sliderSensibility.value;
        }
        else if (netCameraTouchController != null)
        {
            netCameraTouchController.cameraSensitivity = sliderSensibility.value;
        }
    }

    public void ChangeLightRange()
    {
        OnLightRange?.Invoke();
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
    public void Menu(int index)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
    }


    public void GoToKoobby(int index)
    {
        NetworkRunner runner = FindObjectOfType<NetworkRunner>();

        print("LEAVE LOBBY CLICKED");

        if (runner != null)
        {
            runner.Shutdown();
            UnityEngine.SceneManagement.SceneManager.LoadScene(index);
        }
 
    }
}
