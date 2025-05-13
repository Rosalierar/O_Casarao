using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ControllerPlayer : MonoBehaviour
{
    public Vector3 spawnPoint;
    public GameObject blackPainel;
    public TextMeshProUGUI tmpSpeaks;
    public MyButton crunchBtn;
    public MyButton interectBtn;
    public JoyRoots moveJoy;

    int playerHealth;
    public int PlayerHealth
    {
        get { return playerHealth; }
        set
        {
            playerHealth = value;

            languageText = PlayerPrefs.GetInt("Language");
            
            if (languageText == 0)
            {
                if (playerHealth >= 0 && playerHealth < 3)
                    tmpSpeaks.text = textLostLifePt[playerHealth];
            }
            else if (languageText == 1)
            {
                if (playerHealth >= 0 && playerHealth < 3)
                    tmpSpeaks.text = textLostLifePt[playerHealth];
            }

            if (playerHealth < 0)
            {
                if (languageText == 0)
                {
                    tmpSpeaks.text = textPtLost;
                }
                else if (languageText == 1)
                {
                    tmpSpeaks.text = textEnLost;
                }

                Invoke("GameOverScene", 6f);
            }
        }
    }

    private void GameOverScene()
    {
        SceneManager.LoadScene(1); // Load Scene Game Over
    }

    int languageText;
    [SerializeField] private string[] textLostLifePt = 
    {
        "Eu não sou o problema aqui. Isso vai além de mim... Eu preciso sair daqui antes que me apaguem por completo.",
        "Por que tudo sempre recai sobre mim? Não importa o quanto eu me esforce, eu sou sempre a suspeita.",
        "Talvez... se eu fizer tudo certo, ele pare de me tratar assim. Eu só preciso provar que sou confiável.",
    };

    [SerializeField] private string[] textLostLifeEn =
    {
        "Maybe... if I do everything right, he'll stop treating me like this. I just need to prove that I can be trusted.",
        "Why does everything always fall on me? No matter how hard I try, I'm always the suspect.",
        "I'm not the problem here. This is beyond me... I need to get out of here before they erase me completely."
    };
    
    [SerializeField] private string textPtLost = "Não acho que aquela reportagem se aplique a mim… então não devo me preocupar.";
    [SerializeField] private string textEnLost = "I don't think that report applies to me... so I shouldn't worry.";

    [SerializeField] private string textPtWin = "No fim… aquela reportagem me libertou. Da casa… e de tudo que eu pensava sobre mim. Eu mereço mais. E aqui… nunca teve.";
    [SerializeField] private string textEnWin = "In the end... that report freed me. From the house... and from everything I thought about myself. I deserve more. And here... there never was.";
}
