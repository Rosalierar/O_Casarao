using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Fusion;
using System.Collections;


public class NetInventory : NetworkBehaviour
{
    /*Trocar no Canva
    [SerializeField] Image image; // Referência ao script DetectionObjects
    [SerializeField] Image painelInventario;*/
    int language;
    [SerializeField] private TextMeshProUGUI informationAboutItem;
    [SerializeField] private Sprite[] spriteItem = new Sprite[9]; // Referência ao painel de inventário
    [SerializeField] private Image imagePainelItem; // Referência ao painel de inventário
    [SerializeField] private Transform localDeDrop;
    [Networked] private NetworkObject ItemCarregado { get; set; }
    public List<Item> itens = new List<Item>();  // Lista de itens

     public void TryColetarItem(Item item)
    {
        if (!HasInputAuthority) return;

        if (ItemCarregado != null)
        {
            StartCoroutine(TimerForShowInformation(language == 0 ? "Você já está segurando um item." : "You're already carrying an item."));
            return;
        }

        NetworkObject netObj = item.GetComponent<NetworkObject>();
        RPC_ColetarItem(netObj);
    }

    public void TrySoltarItem()
    {
        if (!HasInputAuthority) return;

        if (ItemCarregado == null)
        {
            StartCoroutine(TimerForShowInformation(language == 0 ? "Nenhum item para soltar." : "No item to drop."));
            return;
        }

        RPC_SoltarItem();
    }
    // Função para adicionar itens ao inventário
    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_ColetarItem(NetworkObject itemNet)
    {
        if (ItemCarregado != null) return;

        Item item = itemNet.GetComponent<Item>();

        ItemCarregado = itemNet;
        itemNet.RequestStateAuthority();
        itens.Add(item);
        itemNet.gameObject.SetActive(false);

        language = PlayerPrefs.GetInt("Language");

        imagePainelItem.sprite = spriteItem[((byte)item.tipoDoItem)]; // Atualiza o sprite do painel de inventário

        Debug.Log("Item coletado: " + item.tipoDoItem);

        if (language == 0)
        {
            StartCoroutine(TimerForShowInformation(language == 0 ?
                                                        $"Item coletado: {item.tipoDoItem.ParaNomeLegivel()}" :
                                                        $"Item collected: {item.tipoDoItem.ParaNomeLegivel()}"));
        }
        /*else
        {
            StartCoroutine(TimerForShowInformation("Item collected: " + ItemCarregado.tipoDoItem.ParaNomeLegivel()));
        }*/
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_SoltarItem()
    {
        if (!HasInputAuthority) return;

        language = PlayerPrefs.GetInt("Language");

        Item item = ItemCarregado.GetComponent<Item>();

        ItemCarregado.transform.position = localDeDrop.position;
        //ItemCarregado.transform.SetParent(null);
        ItemCarregado.gameObject.SetActive(true);

        if (ItemCarregado.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.useGravity = true;
            rb.velocity = Vector3.zero;
        }

        if (imagePainelItem != null)
            imagePainelItem.sprite = null;

        imagePainelItem.sprite = null; // Limpa o sprite do painel de inventário

        itens.Remove(item);
        StartCoroutine(TimerForShowInformation(language == 0 ?
                                               $"Item solto: {item.tipoDoItem.ParaNomeLegivel()}" :
                                               $"Item dropped: {item.tipoDoItem.ParaNomeLegivel()}"));

        ItemCarregado = null;
    }

    public void UsarItem()
    {
        if (!HasInputAuthority || ItemCarregado == null ) return;

        Item item = ItemCarregado.GetComponent<Item>();
        itens.Remove(item);

        Runner.Despawn(ItemCarregado);

        imagePainelItem.sprite = null; // Limpa o sprite do painel de inventário
        ItemCarregado = null;
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
