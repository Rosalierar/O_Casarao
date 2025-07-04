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
    string[] textEnConfig = { "Sensibility", "Language", "Portuguese", "English", "Tutorial", "Volume Rain"};
    string[] textPtConfig = { "Sensibilidade", "Idioma", "Português", "Inglês", "Tutorial", "Volume da Chuva" };

    [SerializeField] private CameraTouchController cameraTouchController;

    bool isOptionsOpen = false;

    void Start()
    {
        if (!PlayerPrefs.HasKey("Volume"+0))
        {
            PlayerPrefs.SetFloat("Volume0", 0.015f);
            print("Saved=" + PlayerPrefs.GetFloat("Volume"+0));
        }
        if (!PlayerPrefs.HasKey("Volume"+1))
        {
            PlayerPrefs.SetFloat("Volume1", 0.006f);
            print("Saved=" + PlayerPrefs.GetFloat("Volume"+1));
        }

        audioMusic.audioSorceBackGround[0].volume = PlayerPrefs.GetFloat("Volume0");
        audioMusic.audioSorceBackGround[1].volume = PlayerPrefs.GetFloat("Volume1");
        sliderVolume[0].value = PlayerPrefs.GetFloat("Volume0");
        sliderVolume[1].value = PlayerPrefs.GetFloat("Volume1");
        print("Slider=" + sliderVolume[0].value + " | Saved=" + PlayerPrefs.GetFloat("Volume" + 0));
        print("Slider=" + sliderVolume[1].value + " | Saved=" + PlayerPrefs.GetFloat("Volume" + 1));

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
        audioMusic.audioSorceBackGround[index].volume = sliderVolume[index].value;

        PlayerPrefs.SetFloat("Volume" + index, audioMusic.audioSorceBackGround[index].volume);
        print("Volume" + index + ": Slider=" + sliderVolume[index].value + " | Saved=" + PlayerPrefs.GetFloat("Volume" + index));
        PlayerPrefs.Save();
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
    public void Menu(int index)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
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
