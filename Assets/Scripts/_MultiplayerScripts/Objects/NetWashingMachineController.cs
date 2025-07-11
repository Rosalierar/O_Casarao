using System.Collections;
using Fusion;
using UnityEngine;

public class NetWashingMachineController : NetworkBehaviour
{
    //public GameObject pants;
   // [SerializeField] BoxCollider boxCollider;
    //[Networked] private Vector3 boxSize { get; set; }
    //[Networked] private Vector3 boxCenter { get; set; }

    NetworkObject net;
    [SerializeField] AudioClip[] audios;
    AudioSource audioSource;

    void Start()
    {
        /*if (net.HasStateAuthority)
        {
            if (boxCollider != null && gameObject.tag == "Machine")
            {
                boxSize = new Vector3(1.807f,1.85f,1.438f);
                boxCenter = new Vector3(0.046f, 0.425f, -0.11f);
            }
        }*/
    }

    public override void FixedUpdateNetwork()
    {
        /*print("Size: " + boxSize + "Center: " + boxCenter);
        if (boxCollider != null && gameObject.tag == "Machine")
        {
            ApplyColliderChanges();
        }*/
    }
    
    private void ApplyColliderChanges()
    {
        //boxCollider.size = boxSize;
        //boxCollider.center = boxCenter;
    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SetChangeSizeCollider()
    {
        ChangeSizeCollider();
    }

    public void ChangeSizeCollider()
    {
        //boxSize = new Vector3(0.87f, 0.80f, 2.70f);
        //boxCenter = new Vector3(0.06f, -0.10f, -0.70f);
        //pants.SetActive(false);
    }

    public void StartTime()
    {
        StartCoroutine(TimerWashingClothes());

        /*NetDesableController netDesable = GetComponentInParent<NetDesableController>();
        net = GetComponentInParent<NetworkObject>();

        if (net.HasStateAuthority)
            netDesable.IsActive = false;
        else
            netDesable.RPC_SetVisibilityObj(false);

        if (boxCollider != null)
        {
            if (net.HasStateAuthority)
            {
                print("PORTA DA MAQUINA TEM STATE, MUDANDO COLLIDER");
                ChangeSizeCollider();
            }
            else
            {
                print("PORTA DA MAQUINA NÃO TEM STATE, PEDINDO PARA MUDAR O COLLIDER");
                RPC_SetChangeSizeCollider();
            }
        }*/
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

        net = GetComponentInParent<NetworkObject>();
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
