using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextForSignalController : MonoBehaviour
{
    int language;
    [SerializeField] private TextMeshProUGUI textForSignal;

    [TextArea]
    [SerializeField] private string[] textForSignalStringPt;
    [TextArea]
    [SerializeField] private string[] textForSignalStringEn;


    public void SeeText(int index)
    {
        switch (language)
        {
            case 0:
                textForSignal.text = textForSignalStringPt[index].ToString();
                break;
            case 1:
                textForSignal.text =  textForSignalStringEn[index].ToString();
                break;
            default:
                textForSignal.text =  textForSignalStringPt[index].ToString();
                break;
        }
    }
}
