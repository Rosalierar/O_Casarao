using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogoReportagem : MonoBehaviour
{
    int languageDialogue;
    
    [SerializeField] private string[] dialoguePtTV =
    {
        "Repórter: Região sofre aumento na taxa de agressão contra trabalhadores domésticos que estão sofrendo agressões continuamente na região, entenda o caso.",
        "Repórter: Pesquisas mostram que a taxa de violência verbal e não verbal aumentaram cerca de 70%, para os trabalhadores domésticos, sendo 98% dos casos tendo como vítimas pessoas negras.",
        "Repórter: Ocorreram entrevistas com as pessoas que sofreram essas agressões e em sua maioria foram violentadas pelos próprios patrões.",
        "Repórter: A defesa desses donos da propriedade foram alegados que esses empregados estavam roubando da sua casa. A polícia investiga essas situações.",
        "Repórter: Já as vítimas alegam não ter cometido esses crimes, mas sim, que seus chefes têm preconceito com sua cor, e antes das agressões físicas foram informadas que haviam agressões verbais para os mesmos.",
    };
    [SerializeField] private string[] dialogueEnTV = 
    {
        "Reporter: Region suffers an increase in the rate of aggression against domestic workers who are continuously suffering aggression in the region, understand the case.",
        "Reporter: Research shows that the rate of verbal and non-verbal violence has increased by around 70%, for domestic workers, with 98% of the cases having black people as victims.",
        "Reporter: Interviews with the people who suffered these aggressions took place and most of them were raped by their own bosses.",
        "Reporter: The defense of these property owners was alleged that these employees were stealing from their home. The police are investigating these situations.",
        "Reporter: The victims, on the other hand, claim that they didn't commit these crimes, but that their bosses are prejudiced against their color, and before the physical attacks, they were informed that there were verbal attacks against them.", 
    };

    [SerializeField] private string[] dialoguePtCharacter = 
    {
        "Isso significa que o que eu tenho… o que eu vivo… não é “normal”?…",
        "Meu Patrão não me trata tão bem quanto os outros, eu achava que só era o jeito dele.",
        "mas… sempre que desaparece algo, ele sempre me culpa, só que eu trabalho aqui… eu deveria prestar atenção e tomar conta dos bens dele, para que essas coisas não aconteçam.",
        "Ele pensa assim de mim, só por causa da minha cor?",
        "Minha mãe disse que se um branco se dispusesse a me dar um emprego eu tinha que agradecer… e meu próprio Patrão, concorda com isso.",
        "Mas se aquela reportagem for verdade, eu preciso merecer algo melhor do que isso, e se algo de ruim acontecer por aqui… comigo?",
        "A forma que ele fala comigo, não é agressiva, mas eu sinto que é diferente.",
        "Eu… eu conseguiria um outro trabalho?",
        "Tenho que tentar, não posso deixar que meu Patrão descubra, não sei se ele aceitaria tranquilamente, preciso encontrar um jeito.",
    };
    [SerializeField] private string[] dialogueEnCharacter = 
    {
        "Does that mean that what I have... what I live... isn't “normal”?...",
        "My boss doesn't treat me as well as the others, I thought it was just his way.",
        "But... whenever something goes missing, he always blames me, but I work here... I should pay attention and take care of his property, so that these things don't happen. ",
        "Does he think that about me, just because of my color?",
        "My mother said that if a white man was willing to give me a job, I'd have to say thank you... and my own boss agrees with that.",
        "But if that report is true, I need to deserve better than that, what if something bad happens here... to me? ",
        "The way he talks to me isn't aggressive, but I feel it's different.",
        "Could I get another job?",
        "I have to try, I can't let my boss find out, I don't know if he'd accept it quietly, I have to find a way.",
    };
    // Start is called before the first frame update
    void Start()
    {
        languageDialogue = PlayerPrefs.GetInt("Language");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
