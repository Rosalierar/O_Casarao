using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class InteractiveObject : MonoBehaviour
{
    [Header("UI Sobre Itens")]
    [SerializeField] private TextMeshProUGUI informationAboutItem;
    int language;

    [Header("Porta Principal")]
    [SerializeField] private GameObject[] cadeado = new GameObject[3];
    AudioSource audioSource;

    Animator animFinal;

    [Header("Geladeira")]
    [SerializeField] GameObject passwordPainel;

    private ParentObjectReference parent;

    [Header("Item Controller")]
    bool[] progressionGame = new bool[3];
    public TipoDeItem itemNecessario;  // Tipo de item necessário para interagir com o objeto
    public TipoDeItem tipoDeObjeto;
    /// <Controle dos Objetos, Abrir, Fechar, Quebrar>
    [SerializeField] DoorMoviment doorMoviment;
    [SerializeField] DrawerMoviment drawerMoviment;
    public bool unlocked; // Variável para verificar se o objeto está bloqueado

    //private Inventory inventory;  // Referência ao inventário do jogador

    public void TentarInteragir()
    {
        language = PlayerPrefs.GetInt("Language");

        Debug.Log("Usando o Objeto Resultado:");

        if (parent.inventory.TemItem(itemNecessario) && !unlocked) // Verifica se o inventário tem o item necessário        
        {
            if (itemNecessario == TipoDeItem.ChaveCircular) /////////////////////////////////////////// CHAVE CIRCULAR
            {
                parent.inventory.UsarItem(); // Chama o método de usar item do inventário}
                doorMoviment.enabled = true;
                doorMoviment.TryActiveDoor();

                /*parent.grabTheObject.enabled = true; // Habilita o script de pegar
                parent.useTheObject.enabled = false; // Desabilita o script de usar
                parent.dropTheObject.enabled = false; // Desabilita o script de soltar*/

                Debug.Log("Porta Aberta!");
            }

            else if (itemNecessario == TipoDeItem.PeDeCabra) /////////////////////////////////////////// PE DE CABRA
            {
                parent.inventory.UsarItem(); // Chama o método de usar item do inventário}  

                gameObject.SetActive(false); // Desativa o objeto do mundo

                /*parent.grabTheObject.enabled = true; // Habilita o script de pegar
                parent.useTheObject.enabled = false; // Desabilita o script de usar
                parent.dropTheObject.enabled = false; // Desabilita o script de soltar*/

                Debug.Log("Corrente Quebrada!");
            }

            else if (itemNecessario == TipoDeItem.ChaveQuadrada) /////////////////////////////////////////// CHAVE QUADRADA
            {
                parent.inventory.UsarItem(); // Chama o método de usar item do inventário}
                drawerMoviment.enabled = true;
                drawerMoviment.TryActiveDrawer();

                //gameObject.SetActive(false); // Desativa o objeto do mundo

                Debug.Log("Gaveta Aberta!");
            }

            else if (itemNecessario == TipoDeItem.ChaveVermelha) /////////////////////////////////////////// CHAVE VERMELHA
            {
                audioSource = GetComponent<AudioSource>();
                audioSource.Play();

                parent.inventory.UsarItem(); // Chama o método de usar item do inventário}
                progressionGame[0] = true;

                if (progressionGame[0] && progressionGame[1] && progressionGame[2])
                {
                  //  GoToWin();
                }

                StartCoroutine(ToDisableDelayed(1, 0.5f));
            }

            else if (itemNecessario == TipoDeItem.ChaveAmarela) /////////////////////////////////////////// CHAVE AMARELA
            {
                audioSource = GetComponent<AudioSource>();
                audioSource.Play();

                parent.inventory.UsarItem(); // Chama o método de usar item do inventário}
                progressionGame[1] = true;

                if (progressionGame[0] && progressionGame[1] && progressionGame[2])
                {
                    //GoToWin();
                }

                StartCoroutine(ToDisableDelayed(0, 0.5f));
            }

            else if (itemNecessario == TipoDeItem.ChaveVerde) /////////////////////////////////////////// CHAVE VERDE
            {
                audioSource = GetComponent<AudioSource>();
                audioSource.Play();

                parent.inventory.UsarItem(); // Chama o método de usar item do inventário}
                progressionGame[2] = true;

                if (progressionGame[0] && progressionGame[1] && progressionGame[2])
                {
                   // GoToWin();
                }

                StartCoroutine(ToDisableDelayed(2, 0.5f));
            }

            else if (itemNecessario == TipoDeItem.Carne) /////////////////////////////////////////// CACHORRO
            {
                GetComponent<Animation>().Play("GotOutKey");

                parent.inventory.UsarItem(); // Chama o método de usar item do inventário}
            }

            else if (itemNecessario == TipoDeItem.Desinfetante) /////////////////////////////////////////// MAQUINA DE LAVAR
            {
                GetComponent<WashingMachineController>().StartTime();

                parent.inventory.UsarItem(); // Chama o método de usar item do inventário}
            }

            parent.grabTheObject.enabled = true; // Habilita o script de pegar
            parent.useTheObject.enabled = false; // Desabilita o script de usar
            parent.dropTheObject.enabled = false; // Desabilita o script de solt
            parent.grabTheObject.isHolding = false; // Define que o objeto n�o est� mais sendo segurado
            unlocked = true; // Define que o objeto foi desbloqueado
        }
        
        else if (tipoDeObjeto == TipoDeItem.Senha && !unlocked) /////////////////////////////////////////// GELADEIRA
        {
            passwordPainel.SetActive(true);
        }

        else if (unlocked) // Verifica se o objeto já foi desbloqueado
        {
            switch (tipoDeObjeto)
            {
                case TipoDeItem.Gaveta:
                    drawerMoviment.enabled = true; // Habilita o script de movimentação da gaveta
                    drawerMoviment.TryActiveDrawer();
                    break;

                case TipoDeItem.ChaveQuadrada:
                    drawerMoviment.enabled = true; // Habilita o script de movimentação da gaveta
                    drawerMoviment.TryActiveDrawer();
                    break;

                case TipoDeItem.Porta:
                    doorMoviment.enabled = true; // Habilita o script de movimentação da porta
                    doorMoviment.TryActiveDoor();
                    break;

                case TipoDeItem.Crucifixo:
                    doorMoviment.enabled = true; // Habilita o script de movimentação da porta
                    doorMoviment.TryActiveDoor();
                    break;
                case TipoDeItem.Senha:
                    doorMoviment.enabled = true; // Habilita o script de movimentação da porta
                    doorMoviment.TryActiveDoor();
                    break;
                case TipoDeItem.Desinfetante:
                    doorMoviment.enabled = true; // Habilita o script de movimentação da porta
                    doorMoviment.TryActiveDoor();
                    break;
                default:
                    break;
            }
            parent.grabTheObject.enabled = true; // Habilita o script de pegar
            parent.useTheObject.enabled = false; // Desabilita o script de usar
        }
        else if (!parent.inventory.TemItem(itemNecessario)) // Verifica se o inventário não tem o item necessário
        {
            Debug.Log("Você precisa de " + itemNecessario.ToString() + " para interagir com este objeto.");

            if (language == 0)
            {
                StartCoroutine(TimerForShowInformation("Você precisa de " + itemNecessario.ParaNomeLegivel() + " para interagir com este objeto."));
            }
            else
            {
                StartCoroutine(TimerForShowInformation("You need a: " + itemNecessario.ParaNomeLegivel() + " to interact with this object"));
            }
        }
    }

   /* void GoToWin()
    {
        GameObject portaEntrada = GameObject.FindWithTag("PortaEntrada");
                    
        BoxCollider collider = portaEntrada.GetComponent<BoxCollider>();
        FinalController finalController = portaEntrada.GetComponent<FinalController>();

        collider.enabled = true;
        finalController.enabled = true;

        finalController.DesableCams();
    }*/

    IEnumerator ToDisableDelayed(int index, float delay)
{
    yield return new WaitForSeconds(delay);
    cadeado[index].SetActive(false);
}
    public void SetParentReference(ParentObjectReference parent)
    {
        this.parent = parent;
    }

    IEnumerator TimerForShowInformation(string textInformation)
    {
        informationAboutItem.text = textInformation.ToString();

        yield return new WaitForSeconds(2f);

        informationAboutItem.text = "";
    }

    /*void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            parent = other.GetComponentInChildren<ParentObjectReference>(); // Obtém a referência do ParentObjectReference do jogador
            
            //if (parent.grabTheObject.isHolding) // Verifica se o objeto que colidiu tem a tag "Player"
    
            parent.useTheObject.enabled = true; // Desabilita o script grabTheObject se o objeto estiver sendo segurado para usar apenas o script de interação
            parent.grabTheObject.enabled = false; // Desabilita o script grabTheObject se o objeto estiver sendo segurado para usar apenas o script de interação
           
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica se o objeto que saiu da colisão tem a tag "Player"
        {
            parent.grabTheObject.enabled = false; // Desabilita o script grabTheObject se o objeto estiver sendo segurado para usar apenas o script de interação
            parent.useTheObject.enabled = true; // Habilita o script de usar
            parent.detectionObjects.isCollidingInteractiveObj = false; // Desabilita a detecção de colisão
         
        }
    }*/
}
