using System.Collections;
using Fusion;
using UnityEngine;

public class NetWashingMachineController : NetworkBehaviour
{
    NetworkObject net;
    [SerializeField] AudioClip[] audios;
    AudioSource audioSource;

    public void StartTime()
    {
        NetDesableController netDesable = GetComponentInParent<NetDesableController>();
        net = GetComponentInParent<NetworkObject>();

        if (net.HasStateAuthority)
            netDesable.IsActive = false;
        else
            netDesable.RPC_SetVisibilityObj(false);
        StartCoroutine(TimerWashingClothes());
    }

    IEnumerator TimerWashingClothes()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audios[0];
        audioSource.Play();

        yield return new WaitForSeconds(8f);

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audios[1];
        audioSource.Play();

        NetInteractiveObjects interactive = GetComponent<NetInteractiveObjects>();

        if (net.HasStateAuthority)
        {
            interactive.UnlockedController(true); // Define que o objeto foi desbloqueado
        }
        else if (!net.HasStateAuthority)
        {
            interactive.RPC_SetUnlockedController(true);
        }
    }
}
