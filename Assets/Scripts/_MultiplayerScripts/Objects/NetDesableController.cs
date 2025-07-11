using Fusion;
using UnityEngine;

public class NetDesableController : NetworkBehaviour
{
    public GameObject object4Disable;
    public GameObject other4Disable;
    [Networked] public bool IsActivePdLock0 { get; set; } = true;
    [Networked] public bool IsActivePdLock1 { get; set; } = true;
    [Networked] public bool IsActivePdLock2 { get; set; } = true;

    public GameObject[] padLock;
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

        if (object4Disable != null)
        {
            object4Disable.gameObject.SetActive(IsActive);
        }

        if (other4Disable != null)
        {
            other4Disable.gameObject.SetActive(IsActive);
        }


        bool[] canDisableArray = { IsActivePdLock0, IsActivePdLock1, IsActivePdLock2 };

        for (int i = 0; i < padLock.Length; i++)
        {
            if (padLock[i] != null && canDisableArray[i] == true)
            {
                print(canDisableArray[i] + " Cadeado");
                padLock[i].gameObject.SetActive(canDisableArray[i]);
            }
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SetCanDisablePadLock(bool visible, byte index)
    {
        Debug.Log($"[RPC] SetVisibility Pad lock para {visible} -> {index}, por {Runner.LocalPlayer}");

        CanDisablePadLock(visible, index);
    }

    public void CanDisablePadLock(bool visible, byte index)
    {
        switch (index)
        {
            case 0: IsActivePdLock0 = visible; print("Can 0: " + visible); break;
            case 1: IsActivePdLock1 = visible; print("Can 1: " + visible); break;
            case 2: IsActivePdLock2 = visible; print("Can 2: " + visible); break;
        }
    }
}
