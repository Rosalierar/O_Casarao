using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextForSignalController : MonoBehaviour
{
    GameObject player2;
    [SerializeField] int playerCount = 1;

    int language;
    [SerializeField] private TextMeshProUGUI textForSignal;

    [TextArea]
    [SerializeField] private string[] textForSignalStringPt;
    [TextArea]
    [SerializeField] private string[] textForSignalStringEn;

    [TextArea]
    [SerializeField] private string[] textForSignalConversationPt;
    [TextArea]
    [SerializeField] private string[] textForSignalConversationEn;

    public void ZeroText()
    {
        textForSignal.text = "...";
    }

    public void SeeText(int index)
    {
        switch (language)
        {
            case 0:
                textForSignal.text = textForSignalStringPt[index].ToString();
                break;
            case 1:
                textForSignal.text = textForSignalStringEn[index].ToString();
                break;
            default:
                textForSignal.text = textForSignalStringPt[index].ToString();
                break;
        }
    }

    public void SeeTextConversation(int index)
    {
        switch (language)
        {
            case 0:
                textForSignal.text = textForSignalConversationPt[index].ToString();
                break;
            case 1:
                textForSignal.text = textForSignalConversationEn[index].ToString();
                break;
            default:
                textForSignal.text = textForSignalConversationPt[index].ToString();
                break;
        }
    }

    public void IsMultiPLayer()
    {
        if (playerCount == 2)
            player2.SetActive(true);
    }

    public void LoadScene()
    {
        if (playerCount == 1)
        {
            SceneManager.LoadScene(2); //index da cena Single player
        }
        else if (playerCount == 2)
        {
            SceneManager.LoadScene(5); //index da cena Multi player
        }
    }
}
