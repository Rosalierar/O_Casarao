using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    /*Trocar no Canva
    [SerializeField] Image image; // Referência ao script DetectionObjects
    [SerializeField] Image painelInventario;*/
    [SerializeField] private Sprite[] spriteItem = new Sprite[9]; // Referência ao painel de inventário
    [SerializeField] private Image imagePainelItem; // Referência ao painel de inventário
    [SerializeField] private Transform localDeDrop;
    public Item itemCarregado;
    public List<Item> itens = new List<Item>();  // Lista de itens

    // Função para adicionar itens ao inventário
    public void ColetarItem(Item item)
    {
        if (itemCarregado != null)
        {
            Debug.Log("Você já está segurando um item.");
            return;
        }

        imagePainelItem.sprite = spriteItem[((byte)item.tipoDoItem)]; // Atualiza o sprite do painel de inventário

        itemCarregado = item;
        itens.Add(item);
        item.gameObject.SetActive(false);

        Debug.Log("Item coletado: " + item.tipoDoItem);
    }

    public void SoltarItem()
    {
        if (itemCarregado == null)
        {
            Debug.Log("Nenhum item para soltar.");
            return;
        }

        imagePainelItem.sprite = null; // Limpa o sprite do painel de inventário
        
        itemCarregado.gameObject.SetActive(true);
        itemCarregado.transform.position = localDeDrop.position;
        itemCarregado.GetComponent<Rigidbody>().useGravity = true;

        itens.Remove(itemCarregado);
        Debug.Log("Item solto: " + itemCarregado.tipoDoItem);

        itemCarregado = null;
    }

    public void UsarItem() {

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
}
