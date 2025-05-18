using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    /*Trocar no Canva
    [SerializeField] Image image; // Referência ao script DetectionObjects
    [SerializeField] Image painelInventario;*/
    int language;
    [SerializeField] private TextMeshProUGUI informationAboutItem;
    [SerializeField] private Sprite[] spriteItem = new Sprite[9]; // Referência ao painel de inventário
    [SerializeField] private Image imagePainelItem; // Referência ao painel de inventário
    [SerializeField] private Transform localDeDrop;
    public Item itemCarregado;
    public List<Item> itens = new List<Item>();  // Lista de itens

    // Função para adicionar itens ao inventário
    public void ColetarItem(Item item)
    {
        language = PlayerPrefs.GetInt("Language");

        if (itemCarregado != null)
        {
            Debug.Log("Você já está segurando um item.");

            if (language == 0)
            {
                StartCoroutine(TimerForShowInformation("Você já está segurando um item."));
            }
            else
            {
                StartCoroutine(TimerForShowInformation("You're already get an item."));
            }

            return;
        }

        imagePainelItem.sprite = spriteItem[((byte)item.tipoDoItem)]; // Atualiza o sprite do painel de inventário

        itemCarregado = item;
        itens.Add(item);
        item.gameObject.SetActive(false);

        Debug.Log("Item coletado: " + item.tipoDoItem);

        if (language == 0)
        {
            StartCoroutine(TimerForShowInformation("Item coletado: " + itemCarregado.tipoDoItem.ParaNomeLegivel()));
        }
        else
        {
            StartCoroutine(TimerForShowInformation("Item collected: " + itemCarregado.tipoDoItem.ParaNomeLegivel()));
        }
    }

    public void SoltarItem()
    {
        language = PlayerPrefs.GetInt("Language");

        if (itemCarregado == null)
        {
            Debug.Log("Nenhum item para soltar.");

            if (language == 0)
            {
                StartCoroutine(TimerForShowInformation("Nenhum item para soltar."));
            }
            else
            {
                StartCoroutine(TimerForShowInformation("No item for drop."));
            }

            return;
        }

        imagePainelItem.sprite = null; // Limpa o sprite do painel de inventário

        itemCarregado.gameObject.SetActive(true);
        itemCarregado.transform.position = localDeDrop.position;
        itemCarregado.GetComponent<Rigidbody>().useGravity = true;

        itens.Remove(itemCarregado);
        Debug.Log("Item solto: " + itemCarregado.tipoDoItem);

        if (language == 0)
        {
            StartCoroutine(TimerForShowInformation("Item solto: " + itemCarregado.tipoDoItem.ParaNomeLegivel()));
        }
        else
        {
            StartCoroutine(TimerForShowInformation("Item dropped: " + itemCarregado.tipoDoItem.ParaNomeLegivel()));
        }

        itemCarregado = null;
    }

    public void UsarItem()
    {

        itens.Remove(itemCarregado);

        Destroy(itemCarregado.gameObject);

        imagePainelItem.sprite = null; // Limpa o sprite do painel de inventário
        itemCarregado = null;
    }

    // Verificar se o inventário contém um item específico
    public bool TemItem(TipoDeItem tipo)
    {
        foreach (var item in itens)
        {
            if (item.tipoDoItem == tipo)
                return true;
        }
        return false;
    }
    
    IEnumerator TimerForShowInformation(string textInformation)
    {
        informationAboutItem.text = textInformation.ToString();

        yield return new WaitForSeconds(2f);

        informationAboutItem.text = "";
    }

}
