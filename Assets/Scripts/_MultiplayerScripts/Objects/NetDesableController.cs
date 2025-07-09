using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class NetDesableController : NetworkBehaviour
{
    public GameObject object4Disable;
    [Networked] public bool IsActive { get; set; } = true;

    public override void Render()
    {
        ApplyVisibility();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SetVisibilityObj(bool visible)
    {
        Debug.Log($"[RPC] SetVisibility para {visible} -> {IsActive}, por {Runner.LocalPlayer}");
        IsActive = visible;
    }

    public void ApplyVisibility()
    {
        print(gameObject.name + "Is Visible: " + IsActive + "Possui State: " + HasStateAuthority);
        object4Disable.gameObject.SetActive(IsActive);
    }
}
