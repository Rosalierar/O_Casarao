using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsController : MonoBehaviour
{
    public TextMeshProUGUI[] tmpButtons;
    public TextMeshProUGUI[] tmpConfig;
    public TextMeshProUGUI[] tmpCredits;
    string[] textEnMenu = {"SinglePlayer", "MultiPlayer", "Options", "Credits", "Quit" };
    string[] textEnConfig = {"Language", "Portuguese", "English", "Volume" };
    string[] textEnCredits = {"Credits", "Developed by", "Lucas Oliveira", "Lucas Silva", "Gabriel Almeida", "Gabriel Ferreira", "Matheus Oliveira", "Matheus Silva" };
    
    string[] textPtMenu = {"Modo Solo", "Modo Multiplayer", "Opções", "Créditos", "Sair" };
    string[] textPtConfig = {"Idioma", "Português", "Inglês", "Volume"};
    string[] textPtCredits = {"Créditos", "Desenvolvido por", "Lucas Oliveira", "Lucas Silva", "Gabriel Almeida", "Gabriel Ferreira", "Matheus Oliveira", "Matheus Silva" };
    
    [SerializeField] private byte LoadSceneSinglePlayer;
    [SerializeField] private byte LoadSceneMultiPlayer;

    [SerializeField] private GameObject painelMenu;
    [SerializeField] private GameObject painelOptions;
    [SerializeField] private GameObject painelCredits;

    void Start()
    {
        if(!PlayerPrefs.HasKey("Language"))
        {
            PlayerPrefs.SetInt("Language", 0);
        }
        
        ChangeLanguageMenu();
        
    }

    public void StartGameSingle()
    {
        SceneManager.LoadScene(LoadSceneSinglePlayer);
    }
    public void StartGameMulti()
    {
        SceneManager.LoadScene(LoadSceneMultiPlayer);
    }

    public void OpenCredits()
    {
        painelMenu.SetActive(false);
        painelCredits.SetActive(true);

        int language = PlayerPrefs.GetInt("Language");
            if (language == 0)
            {
                    for (int i = 0; i < tmpCredits.Length; i++)
                    {
                            tmpCredits[i].text = textPtCredits[i];
                    }
            }
            else
            {
                    for (int i = 0; i < tmpCredits.Length; i++)
                    {
                            tmpCredits[i].text = textEnCredits[i];
                    }
            }
    }
    public void CloseCredits()
    {
        painelCredits.SetActive(false);
        painelMenu.SetActive(true);

        ChangeLanguageMenu();
    }

        
    void ChangeLanguageMenu()
    {
         int language = PlayerPrefs.GetInt("Language");
         
            if (language == 0)
            {
                    for (int i = 0; i < tmpButtons.Length; i++)
                    {
                        tmpButtons[i].text = textPtMenu[i];
                    }
            }
            else
            {
                    for (int i = 0; i < tmpButtons.Length; i++)
                    {
                        tmpButtons[i].text = textEnMenu[i];
                    }
            }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    /// <summary>
    /// //////////////////////////////////////////////////////////////////// OPTIONS
    /// </summary>
    public void OpenOptions()
    {
        painelMenu.SetActive(false);
        painelOptions.SetActive(true);

        int language = PlayerPrefs.GetInt("Language");
            if (language == 0)
            {
                    for (int i = 0; i < tmpConfig.Length; i++)
                    {
                            tmpConfig[i].text = textPtConfig[i];
                    }
            }
            else
            {
                    for (int i = 0; i < tmpConfig.Length; i++)
                    {
                            tmpConfig[i].text = textEnConfig[i];
                    }
            }
    }
    public void CloseOptions()
    {
        painelOptions.SetActive(false);
        painelMenu.SetActive(true);

        ChangeLanguageMenu();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void ChangeLanguage(int language)
    {
        switch (language)
        {
            case 0:
                Debug.Log("Language: Portuguese");
                PlayerPrefs.SetInt("Language", 0);
                break;
            case 1:
                Debug.Log("Language: English");
                PlayerPrefs.SetInt("Language", 1);
                break;
            default:
                Debug.Log("Language: Portuguese");
                PlayerPrefs.SetInt("Language", 0);
                break;
        }
    }
}
