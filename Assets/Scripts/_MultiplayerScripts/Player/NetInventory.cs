using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Fusion;
using System.Collections;


public class NetInventory : NetworkBehaviour
{
    int language;
    [SerializeField] private TextMeshProUGUI informationAboutItem;
    [SerializeField] private Sprite[] spriteItem = new Sprite[9]; // Referência ao painel de inventário
    [SerializeField] private Image imagePainelItem; // Referência ao painel de inventário
    [SerializeField] private Transform localDeDrop;
    [Networked] private NetworkObject CarryItem { get; set; }
    [SerializeField] public NetItem itemCarregado;

    public List<NetItem> itens = new List<NetItem>();  // Lista de itens

     public void TryColetarItem(NetItem item)
    {
        if (!HasInputAuthority) return;

        if (CarryItem != null)
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

        if (CarryItem == null)
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
        if (CarryItem != null) return;

        NetItem item = itemNet.GetComponent<NetItem>();

        itemCarregado = item;
        CarryItem = itemNet;
        
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

        NetItem item = CarryItem.GetComponent<NetItem>();

        CarryItem.transform.position = localDeDrop.position;

        CarryItem.gameObject.SetActive(true);

        if (CarryItem.TryGetComponent<Rigidbody>(out var rb))
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

        CarryItem = null;
        itemCarregado = null;
    }

    public void UsarItem()
    {
        if (!HasInputAuthority || CarryItem == null ) return;

        NetItem item = CarryItem.GetComponent<NetItem>();
        itens.Remove(item);

        Runner.Despawn(CarryItem);

        imagePainelItem.sprite = null; // Limpa o sprite do painel de inventário
        CarryItem = null;
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
