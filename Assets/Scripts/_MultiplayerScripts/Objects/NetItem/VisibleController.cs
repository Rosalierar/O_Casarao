using Fusion;
using UnityEngine;

public class VisibleController : NetworkBehaviour
{
    NetItem item;
    [Networked] public bool IsVisible { get; set; } = true;

    public override void Spawned()
    {
        item = GetComponentInChildren<NetItem>();
    }

    public override void Render()
    {
        ApplyVisibility();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SetVisibility(bool visible)
    {
        Debug.Log($"[RPC] SetVisibility para {visible} -> {IsVisible}, por {Runner.LocalPlayer}");
        IsVisible = visible;
    }


    public void ApplyVisibility()
    {
        print(gameObject.name + "Is Visible: " + IsVisible + "Possui State: " + HasStateAuthority);
        item.gameObject.SetActive(IsVisible);
    }

    public void MovePos(Transform position)
    {

        
    }
}
