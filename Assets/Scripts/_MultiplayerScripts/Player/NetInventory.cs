using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Fusion;
using System.Collections;


public class NetInventory : NetworkBehaviour
{
    NetworkObject networkObject;
    int language;
    [SerializeField] private TextMeshProUGUI informationAboutItem;
    [SerializeField] private Sprite[] spriteItem = new Sprite[9]; // Referência ao painel de inventário
    [SerializeField] private Image imagePainelItem; // Referência ao painel de inventário
    [SerializeField] private Transform localDeDrop;
    [Networked] private NetworkObject CarryItem { get; set; }
    [SerializeField] public NetItem itemCarregado;

    public List<NetItem> itens = new List<NetItem>();  // Lista de itens


    // Função para adicionar itens ao inventário
    public void TryColetarItem(NetItem item)
    {
        networkObject = GetComponentInParent<NetworkObject>();

        if (!networkObject.HasInputAuthority) return;

        if (itemCarregado != null)
        {
            StartCoroutine(TimerForShowInformation(language == 0 ? "Você já está segurando um item." : "You're already carrying an item."));
            return;
        }

        NetworkObject netObj = item.GetComponentInParent<NetworkObject>();
        //VisibleController visible = item.GetComponentInParent<VisibleController>();

        itemCarregado = item;
        CarryItem = netObj;

        itens.Add(item);

        if (networkObject.HasStateAuthority)
        {
            Runner.Despawn(CarryItem);
            //visible.IsVisible = false;
            print("INVENTORY PODE STATE:  SIM CHAMANDO NORMAL");
        }
        else if (!networkObject.HasStateAuthority)
        {
            Runner.Despawn(CarryItem);
            //visible.RPC_SetVisibility(false);
            print("INVENTORY PODE STATE: NÃO CHAMANDO RPC? " );
        }
            
        //netObj.gameObject.SetActive(itemCarregado.IsVisible);

        language = PlayerPrefs.GetInt("Language");

        imagePainelItem.sprite = spriteItem[((byte)item.tipoDoItem)]; // Atualiza o sprite do painel de inventário

        Debug.Log("Item coletado: " + item.tipoDoItem);

        if (language == 0)
        {
            StartCoroutine(TimerForShowInformation(language == 0 ?
                                                        $"Item coletado: {item.tipoDoItem.ParaNomeLegivel()}" :
                                                        $"Item collected: {item.tipoDoItem.ParaNomeLegivel()}"));
        }
    }

    public void TrySoltarItem() // funcao que solta item do inventario
    {
        if (!networkObject.HasInputAuthority) return;

        if (itemCarregado == null)
        {
            StartCoroutine(TimerForShowInformation(language == 0 ? "Nenhum item para soltar." : "No item to drop."));
            return;
        }

        language = PlayerPrefs.GetInt("Language");

        NetItem item = itemCarregado.GetComponent<NetItem>();
        //VisibleController visible = itemCarregado.GetComponentInParent<VisibleController>();

        //visible.transform.position = localDeDrop.position;
        //visible.MovePos(localDeDrop);

        //itemCarregado.gameObject.SetActive(true);
        if (HasStateAuthority)
        {
            Runner.Spawn(CarryItem, localDeDrop.position, CarryItem.transform.rotation);
            //visible.IsVisible = true;
            print("INVENTORY PODE STATE:  SIM CHAMANDO NORMAL");
        }
        else if (!HasStateAuthority)
        {
            Runner.Spawn(CarryItem, localDeDrop.position, CarryItem.transform.rotation);
            //visible.RPC_SetVisibility(true);
            print("INVENTORY PODE STATE: NÃO CHAMANDO RPC? ");
        }
            
        if (itemCarregado.TryGetComponent<Rigidbody>(out var rb))
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

    /*[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SetVisibility(bool visible)
    {
        VisibleController visiblec = itemCarregado.GetComponentInParent<VisibleController>();
        Debug.Log($"[RPC] SetVisibility para {visible} -> {visiblec.IsVisible}, por {Runner.LocalPlayer}");
        visiblec.IsVisible = visible;
    }*/

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_ColetarItem(NetItem item)
    {
        if (itemCarregado != null) return;

        TryColetarItem(item);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SoltarItem()
    {
        TrySoltarItem();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_UsarItem()
    {
        UsarItem();
    }

    public void UsarItem() //usa item do inventario
    {
        if (!networkObject.HasInputAuthority) return;

        if (itemCarregado == null) return;

        itens.Remove(itemCarregado);

        Runner.Despawn(CarryItem);

        imagePainelItem.sprite = null; // Limpa o sprite do painel de inventário
        CarryItem = null;
        itemCarregado = null;
    }

    // Verificar se o inventário contém um item específico
    public bool TemItem(TipoDeItem tipo)
    {
        if (!HasInputAuthority) return false;
    
        foreach (var item in itens)
        {
            if (item.tipoDoItem == tipo)
                return true;
        }
        return false;
    }
    
    IEnumerator TimerForShowInformation(string textInformation)
    {
        if (networkObject.HasInputAuthority)
        {
            informationAboutItem.text = textInformation.ToString();

            yield return new WaitForSeconds(2f);

            informationAboutItem.text = "";
        }
    }
}
