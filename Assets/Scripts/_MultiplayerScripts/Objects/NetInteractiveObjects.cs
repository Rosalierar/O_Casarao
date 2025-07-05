using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;

public class NetInteractiveObjects : MonoBehaviour
{

     [Header("Sons dos Itens")]
    [SerializeField] AudioSource AS;
    
    [Header("UI Sobre Itens")]
    [SerializeField] private TextMeshProUGUI informationAboutItem;
    int language;

    [Header("Porta Principal")]
    [SerializeField] private GameObject[] cadeado = new GameObject[3];
    AudioSource audioSource;

    Animator animFinal;

    [Header("Geladeira")]
    [SerializeField] GameObject passwordPainel;

    private NetParentObjectReference parent;

    [Header("Item Controller")]
    bool[] progressionGame = new bool[3];
    public TipoDeItem itemNecessario;  // Tipo de item necessário para interagir com o objeto
    public TipoDeItem tipoDeObjeto;
    public bool unlocked; // Variável para verificar se o objeto está bloqueado
   
     [Header("Controller Open/Close")]
    /// <Controle dos Objetos, Abrir, Fechar, Quebrar>
    [SerializeField] NetDoorMoviment doorMoviment;
    [SerializeField] NetDrawerMoviment drawerMoviment;

    public void TentarInteragir()
    {
        informationAboutItem = GameObject.Find("ItemTextController").GetComponent<TextMeshProUGUI>();

        language = PlayerPrefs.GetInt("Language");

        Debug.Log("Usando o Objeto Resultado:");

        if (parent.inventory.TemItem(itemNecessario) && !unlocked) // Verifica se o inventário tem o item necessário        
        {
            if (itemNecessario == TipoDeItem.ChaveCircular) /////////////////////////////////////////// CHAVE CIRCULAR
            {
                parent.inventory.UsarItem(); // Chama o método de usar item do inventário}
                drawerMoviment.enabled = true;
                drawerMoviment.TryActiveDrawer();

                /*parent.grabTheObject.enabled = true; // Habilita o script de pegar
                parent.useTheObject.enabled = false; // Desabilita o script de usar
                parent.dropTheObject.enabled = false; // Desabilita o script de soltar*/

                Debug.Log("Porta Aberta!");
            }

            if (itemNecessario == TipoDeItem.Alicate)
            {
                parent.inventory.UsarItem(); // Chama o método de usar item do inventário}  

                gameObject.SetActive(false); // Desativa o objeto do mundo

                Debug.Log("Corrente Quebrada!");
            }

            else if (itemNecessario == TipoDeItem.PeDeCabra) /////////////////////////////////////////// PE DE CABRA
            {
                parent.inventory.UsarItem(); // Chama o método de usar item do inventário}
                doorMoviment.enabled = true;
                doorMoviment.TryActiveDoor();

                //gameObject.SetActive(false); // Desativa o objeto do mundo

                Debug.Log("Porta Aberta!");
            }
            else if (itemNecessario == TipoDeItem.Crucifixo)
            {
                parent.inventory.UsarItem(); // Chama o método de usar item do inventário}
                doorMoviment.enabled = true;
                doorMoviment.TryActiveDoor();

                //gameObject.SetActive(false); // Desativa o objeto do mundo

                Debug.Log("Porta Aberta!");
            }

            else if (itemNecessario == TipoDeItem.ChaveQuadrada) /////////////////////////////////////////// CHAVE QUADRADA
            {
                parent.inventory.UsarItem(); // Chama o método de usar item do inventário}
                drawerMoviment.enabled = true;
                drawerMoviment.TryActiveDrawer();

                //doorMoviment.enabled = true;
                //doorMoviment.TryActiveDoor();
                //gameObject.SetActive(false); // Desativa o objeto do mundo

                Debug.Log("Gaveta Aberta!");
            }

            else if (itemNecessario == TipoDeItem.ChaveVermelha) /////////////////////////////////////////// CHAVE VERMELHA
            {
                audioSource = GetComponent<AudioSource>();
                audioSource.Play();

                parent.inventory.UsarItem(); // Chama o método de usar item do inventário}
                progressionGame[0] = true;

                StartCoroutine(ToDisableDelayed(1, 0.5f));
            }

            else if (itemNecessario == TipoDeItem.ChaveAmarela) /////////////////////////////////////////// CHAVE AMARELA
            {
                audioSource = GetComponent<AudioSource>();
                audioSource.Play();

                parent.inventory.UsarItem(); // Chama o método de usar item do inventário}
                progressionGame[1] = true;

                StartCoroutine(ToDisableDelayed(0, 0.5f));
            }

            else if (itemNecessario == TipoDeItem.ChaveVerde) /////////////////////////////////////////// CHAVE VERDE
            {
                audioSource = GetComponent<AudioSource>();
                audioSource.Play();

                parent.inventory.UsarItem(); // Chama o método de usar item do inventário}
                progressionGame[2] = true;

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

            if (itemNecessario != TipoDeItem.Desinfetante)
                unlocked = true; // Define que o objeto foi desbloqueado
        }
        
        else if (tipoDeObjeto == TipoDeItem.Senha && !unlocked) /////////////////////////////////////////// GELADEIRA
        {
            passwordPainel = GameObject.Find("PanelGeladeira");
            passwordPainel.SetActive(true);
        }

        else if (unlocked) // Verifica se o objeto já foi desbloqueado
        {
            switch (tipoDeObjeto)
            {
                case TipoDeItem.Gaveta:
                    drawerMoviment.enabled = true; // Habilita o script de movimentação da gaveta
                    drawerMoviment.TryActiveDrawer();
                    AS.clip = parent.AC[13];
                    AS.Play();
                    break;

                case TipoDeItem.Porta:
                    doorMoviment.enabled = true; // Habilita o script de movimentação da porta
                    doorMoviment.TryActiveDoor();
                    AS.clip = parent.AC[12];
                    AS.Play();
                    break;

                case TipoDeItem.Senha:
                    doorMoviment.enabled = true; // Habilita o script de movimentação da porta
                    doorMoviment.TryActiveDoor();
                    AS.clip = parent.AC[11];
                    
                    AS.Play();
                    break;
                case TipoDeItem.Desinfetante:
                    doorMoviment.enabled = true; // Habilita o script de movimentação da porta
                    doorMoviment.TryActiveDoor();
                    AS.clip = parent.AC[7];
                    AS.Play();
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

    IEnumerator ToDisableDelayed(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        cadeado[index].SetActive(false);
    }
    public void SetParentReference(NetParentObjectReference parent)
    {
        informationAboutItem = GameObject.Find("ItemTextController").GetComponent<TextMeshProUGUI>();

        this.parent = parent;
    }

    IEnumerator TimerForShowInformation(string textInformation)
    {
        informationAboutItem.text = textInformation.ToString();

        yield return new WaitForSeconds(2f);

        informationAboutItem.text = "";
    }
}
