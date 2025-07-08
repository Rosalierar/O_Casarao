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

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SetLocalToDropItem(Vector3 positionForDrop, Quaternion rotationForDrop)
    {
         Debug.Log($"[RPC] Posicionando item em {positionForDrop}");

        // Move o objeto que tem o NetworkObject (geralmente o pai)
        MovePos(positionForDrop,rotationForDrop);
    }


    public void ApplyVisibility()
    {
        print(gameObject.name + "Is Visible: " + IsVisible + "Possui State: " + HasStateAuthority);
        item.gameObject.SetActive(IsVisible);
    }

    public void MovePos(Vector3 positionForDrop, Quaternion rotationForDrop)
    {
        transform.position = positionForDrop;
        transform.rotation = rotationForDrop;
    }
}
