using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PasswordController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textPassword;
    [SerializeField] int correctPassword;

    public void number(int num)
    {
        if (textPassword.text.Length < 4)
        {
            textPassword.text += num;
        }
        else
        {
            IsPasswordCorrect();
        }
    }

    void IsPasswordCorrect()
    {
        if (textPassword.text == correctPassword.ToString())
        {

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
