using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Fusion;

public class NetControllerPlayer : NetworkBehaviour
{
    public bool wasCatch;
    [Networked] public byte totalPlayersCatch { get; set; }
    public Vector3 spawnPoint;
    public GameObject blackPainel;
    public TextMeshProUGUI tmpSpeaks;
    public MyButton crunchBtn;
    public MyButton interectBtn;
    public JoyRoots moveJoy;

    public bool ChangeBoolWasCatch(byte number)
    {
        wasCatch = number != 0;
        return wasCatch;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SetCatch(NetworkObject network)
    {
        Catch(network);
    }

    public void Catch(NetworkObject network)
    {
        Transform rootPortaPorao = GameObject.FindWithTag("Porao").transform.root;
        print("rootPortaPorao" + rootPortaPorao);
        NetDoorMoviment NetDoorMoviment = rootPortaPorao.GetComponentInChildren<NetDoorMoviment>();
        print("NetDoorMoviment" + NetDoorMoviment);
        NetInteractiveObjects netInteractive = rootPortaPorao.GetComponentInChildren<NetInteractiveObjects>();
        print("netInteractive" +netInteractive);

        if (network.HasInputAuthority)
        {
            netInteractive.GetPlayer(this);
        }

        if (FindFirstObjectByType<NetPatraoController>().networkObject.HasStateAuthority)
            netInteractive.UnlockedController(false);
        else
            netInteractive.RPC_SetUnlockedController(false);

        if (NetDoorMoviment.isOpen)
        {
            NetDoorMoviment.enabled = true;

            if (NetDoorMoviment.networkObject.HasStateAuthority)
                NetDoorMoviment.TryActiveDoor();
            else if (!NetDoorMoviment.networkObject.HasStateAuthority)
                NetDoorMoviment.Rpc_RequestToggleDoor();
        }

        if (totalPlayersCatch + 1 >= 2)
        {
            if (FindFirstObjectByType<NetPatraoController>().networkObject.HasStateAuthority)
                GameOverScene();
            else
                RPC_SetGameOverScene();
        }

        totalPlayersCatch++;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SetUnlockedPlayer()
    {
        UnlockedPlayer();
    }

    public void UnlockedPlayer()
    {
        totalPlayersCatch = 0;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SetGameOverScene()
    {
        GameOverScene();
    }

    private void GameOverScene()
    {
        NetworkRunner runner = FindObjectOfType<NetworkRunner>();
        runner.LoadScene(SceneRef.FromIndex(1));
    }

    int languageText;
    [SerializeField] private string[] textLostLifePt = 
    {
        "Eu não sou o problema aqui. Isso vai além de mim... Eu preciso sair daqui antes que me apaguem por completo.",
        "Por que tudo sempre recai sobre mim? Não importa o quanto eu me esforce, eu sou sempre a suspeita.",
        "Talvez... se eu fizer tudo certo, ele pare de me tratar assim. Eu só preciso provar que sou confiável.",
    };

    [SerializeField] private string[] textLostLifeEn =
    {
        "Maybe... if I do everything right, he'll stop treating me like this. I just need to prove that I can be trusted.",
        "Why does everything always fall on me? No matter how hard I try, I'm always the suspect.",
        "I'm not the problem here. This is beyond me... I need to get out of here before they erase me completely."
    };
    
    [SerializeField] private string textPtLost = "Não acho que aquela reportagem se aplique a mim… então não devo me preocupar.";
    [SerializeField] private string textEnLost = "I don't think that report applies to me... so I shouldn't worry.";

    [SerializeField] private string textPtWin = "No fim… aquela reportagem me libertou. Da casa… e de tudo que eu pensava sobre mim. Eu mereço mais. E aqui… nunca teve.";
    [SerializeField] private string textEnWin = "In the end... that report freed me. From the house... and from everything I thought about myself. I deserve more. And here... there never was.";
}
