using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ChooseController : MonoBehaviour
{
    [SerializeField] GameObject[] painels;

    int language;
    
    [SerializeField] TextMeshProUGUI[] tmpObjects;
    string[] textPtObjects = { "O que eu deveria fazer?", "Empreendedorismo", "Nova Casa", "(Escolha qual caminho seguir)" };
    string[] textEnObjects= { "What should I do?", "Entrepreneurship", "New House", "(Choose a path to follow)" };


    [SerializeField] TextMeshProUGUI dialogueText;
    List<string> choices = new List<string>();

    //0 empreendorismo - 1 Nova Casa --------------- PORTUGUES
    string[] firstTextPt = new string[2] 
    {
        /// 0
        "Quando eu saí daquela casa, comecei a trabalhar em lugares diferentes e juntar dinheiro procurando algo que eu queria fazer. " +
        "Em vários lugares, conheci pessoas que falavam comigo do mesmo jeito que meu antigo Patrão, " +
        "mas encontrei vários que se desculparam também.",  
        
        /// 1 
        "Quando eu fugi daquela casa, eu fiquei apreensiva em continuar trabalhando como empregada doméstica, " +
        "ainda mais por conta da reportagem, então eu arrumei minha renda em outra área, como atendente de locadora."
    };
    string[] secondTextPt = new string[2]
    {
        ///0
        "Durante a ida a um dos meus trabalhos eu vi  uma pequena loja de bolo recentemente aberta, " +
        "quando olhei para ela pude encontrar uma vontade que nunca senti antes, decidi que iria abrir a minha própria lanchonete.",

        ///1
        "Com ele, eu tinha bastante contato com outras pessoas e descobri que as atitudes de algumas, com a minha cor, " +
        "nem sempre eram propositais ou por maldade, afinal minha própria mãe, pensa de forma parecida, " +
        "até eu pensava… É claro que não tolero mais isso, e explico e converso do meu jeitinho, com elas para ver se abrem um pouco a mente."
    };
    string[] thirdTextPt = new string[2]
    {
        ///0
        "Sai dos meus trabalhos e comecei a fazer salgados caseiros e ir vendendo nas ruas, com o tempo, " +
        "comprei uma barraca e trabalhei de forma fixa em uma região na praça da minha cidade. " +
        "Não foi um sucesso logo no início, mas eu estava trabalhando em algo que eu podia dar um sorriso verdadeiro a cada cliente que chegasse, a cada salgado que eu fizesse.",

        ///1
        "Não demorou muito para eu largar esse trabalho de atendente e voltar a cuidar de uma casa, eu gosto desse trabalho. " +
        "Agora sou uma empregada doméstica de uma casa de uma senhora branca, esse tipo de racismo não se aplica a todo mundo, ela é super simpática comigo."
    };
    string[] fourthTextPt = new string[2]
    {
        ///0
        "E agora estou aqui, com a minha lanchonete da cidade, não tem muitas por aqui, então muita gente acaba vindo comprar em minha mão, " +
        "tenho uma vida estabilizada agora, com minha própria casinha. Tenho muito orgulho do que faço, minha mãe também está orgulhosa de mim.",

        ///1
        "Consegui comprar uma casa bem confortável para chamar de meu, e recebo um salário que me paga muito bem. Sempre quando chego em casa, fico tranquila e bem comigo mesma."
    };

    //0 empreendorismo - 1 Nova Casa --------------- ENGLES
    string[] firstTextEn = new string[2]
    {
        /// 0
        "When I left that house, I started working in different places and collecting money looking for something I wanted to do. " +
        "In various places, I met people who spoke to me in the same way as my old boss, " +
        "but I met several who apologized as well.",  
    
        /// 1 
        "When I ran away from that house, I was apprehensive about continuing to work as a domestic worker, " +
        "even more so because of the report, so I earned my income in another area, as an attendant at a video store. "
    };
    string[] secondTextEn = new string[2]
    {
        ///0
        "While going to one of my jobs I saw a small cake store that had recently opened, " +
        "When I looked at it I could find an urge I had never felt before, I decided I would open my own cake shop.",

        ///1
        "With him, I had a lot of contact with other people and discovered that some people's attitudes towards my color, " +
        "weren't always deliberate or out of malice, after all my own mother thinks along similar lines, " +
        "even I thought so... Of course, I don't tolerate it anymore, and I explain and talk to them in my own way to see if they open their minds a little."
    };
    string[] thirdTextEn = new string[2]
    {
        ///0
        "I quit my job and started making homemade snacks and selling them on the street. Over time, " +
        "I bought a stall and worked permanently in an area of my town's square. " +
        "It wasn't a success right from the start, but I was working on something that I could give a real smile to every customer who came in, with every snack I made.",

        ///1
        "It didn't take long for me to give up this job as an attendant and go back to looking after a house, I like this job. " +
        "Now I'm a maid in a white lady's house, that kind of racism doesn't apply to everyone, she's super nice to me."
    };
    string[] fourthTextEn = new string[2]
    {
     ///0
        "And now I'm here, with my eatery in town. There aren't many around here, so a lot of people end up buying from me, " +
        "I have a stable life now, with my own little house. I'm very proud of what I do, and my mother is proud of me too.",

     ///1
        "I managed to buy a very comfortable house to call my own, and I get a salary that pays me very well. Whenever I get home, I feel calm and good about myself."
    };

    void Awake()
    {
        language = PlayerPrefs.GetInt("Language");
        print(PlayerPrefs.GetInt("Language"));
    }

    void Start()
    {
         if (language == 0)
            {
                for (int i = 0; i < tmpObjects.Length; i++)
                {
                    tmpObjects[i].text = textPtObjects[i];
                }
            }
        else
        {
            for (int i = 0; i < tmpObjects.Length; i++)
            {
                tmpObjects[i].text = textEnObjects[i];
            }
        }
    }

    public void ChooseFinal(int index)
    {
        painels[0].SetActive(false);
        painels[1].SetActive(true);

        language = PlayerPrefs.GetInt("Language");

        if (language == 0)
        {
            choices.Add(firstTextPt[index]);
            choices.Add(secondTextPt[index]);
            choices.Add(thirdTextPt[index]);
            choices.Add(fourthTextPt[index]);
        }
        else
        {
            choices.Add(firstTextEn[index]);
            choices.Add(secondTextEn[index]);
            choices.Add(thirdTextEn[index]);
            choices.Add(fourthTextEn[index]);
        }

        StartCoroutine(ShowDialogue(choices));
    }
    IEnumerator ShowDialogue(List<string> texts)
    {
        foreach (string line in texts)
        {
            dialogueText.text = line;
            //yield return new WaitForSeconds(25f);

            float timer = 0f;
            bool avancar = false;

            // Espera até o jogador tocar/clicar ou passar o tempo limite
            while (!avancar)
            {
                timer += Time.deltaTime;

                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    avancar = true;
                }

                if (timer >= 30f)
                {
                    avancar = true;
                }

                yield return null;
            }
        }

        // (Opcional) Limpar o texto no final
        dialogueText.text = "";

        SceneManager.LoadScene(0);
    }
}
