using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class PasswordController : MonoBehaviour
{
    AudioSource audioSource;
    //AudioClip songUnlocked;
    [SerializeField] TextMeshProUGUI textPassword;
    [SerializeField] int correctPassword;

    public void number(int num)
    {
        if (textPassword.text.Length < 4)
        {
            textPassword.text += num;
        }

        IsPasswordCorrect();
    }

    void IsPasswordCorrect()
    {
        if (textPassword.text == correctPassword.ToString())
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.Play();

            GameObject.Find("Geladeira").GetComponentInChildren<InteractiveObject>().unlocked = true;

            Invoke("ClosePainel", 0.7f);

            //Ciolocar soom
        }
    }
    public void Reset()
    {
        textPassword.text = "";
    }
    public void ClosePainel()
    {
        this.gameObject.SetActive(false);
    }
}
