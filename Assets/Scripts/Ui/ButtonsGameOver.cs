using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsGameOver : MonoBehaviour
{
    [SerializeField] private byte LoadSceneMenu;


    public void VoltaMenu()
    {
        SceneManager.LoadScene(LoadSceneMenu);
        Debug.Log("tentando voltar");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
