using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fusion;
using System.Threading.Tasks;

public class TextForSignalController : MonoBehaviour
{
    private bool isMultiplayer = false;
    private NetworkRunner runner;
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

    void Awake()
    {
        try
        {
            runner = FindObjectOfType<NetworkRunner>();
        }
        catch
        {
            print("Não está no Multiplayer");
        }

        if (runner != null)
        {
            isMultiplayer = true;
            print("Multiplayer");
        }
        else
        {
            print("Não Multiplayer");
        }
    }

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
        if (!isMultiplayer)
        {
            SceneManager.LoadScene(2); //index da cena Single player
        }
        else if (isMultiplayer)
        {
            NetworkRunner runner = FindObjectOfType<NetworkRunner>();

            StartGameMultiplayer(runner); //index da cena Multi player
        }
    }

    private async void StartGameMultiplayer(NetworkRunner runner)
    {
        await runner.LoadScene(SceneRef.FromIndex(5));
    }
}
