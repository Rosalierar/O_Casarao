using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Fusion;
using Fusion.Sockets;
using System;

public class LoobyManager : MonoBehaviour, INetworkRunnerCallbacks
{
    bool isFirstPlayer;
    [Header("Spawnner Objects Controller")]
    private string[] tagDosObjetos = new string[] { "Itens", "Enemy" };
    GameObject[] gameObj;
    private Vector3[] spawnPoints = new Vector3[2]
    {
        new Vector3(-6.3f,6.68f,0.68f),
        new Vector3(-2.95f,6.68f,0.68f), 
        //new Vector3(-800.55f, 65.5f, 0.8f),
        //new Vector3(-9.2f,65.5f, 0.8f)
    };
    [SerializeField] private int indexSceneForStart;
    [SerializeField] private NetworkManager networkManager;
    NetworkRunner networkRunner;
    public static LoobyManager Instance;

    [Header("Player Prefab")]
    //Animator anim;
    //[SerializeField] AnimatorOverrideController[] animatorOverrideController;
    [SerializeField] private NetworkObject playerPrefab1, playerPrefab2;
    [SerializeField] private NetworkObject HouseMultiplayer;

    public List<PlayerRef> connectedPlayers = new List<PlayerRef>();
    public Dictionary<PlayerRef, NetworkObject> playerObjects = new Dictionary<PlayerRef, NetworkObject>();

    [Header("UI")]
    [SerializeField] private GameObject[] painels = new GameObject[3];
    [SerializeField] private GameObject[] painelsHost = new GameObject[2];
    private string pendingJoinLobbyCode;

    public TMP_InputField lobbyCodeInputField;
    public TMP_Text lobbyCodeText;
    public TMP_Text player1StatusText;
    public TMP_Text player2StatusText;
    public Button startGameButton;
    public Button changeDifficultyButton;

    [Header("Runner Prefab")]
    private bool isStartingGame = false;
    public NetworkRunner runnerPrefab;
    private NetworkRunner runnerInstance;
    private string currentLobbyCode;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Já existe uma instância de LoobyManager, destruindo duplicata.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("LoobyManager criado e persistente");
    }
    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("runnerInstance: " + runnerInstance);
        Debug.Log("runnerPrefab: " + runnerPrefab);

        startGameButton.interactable = false;
        changeDifficultyButton.interactable = false;
    }

    void OnDestroy()
    {
        runnerInstance.LoadScene(SceneRef.FromIndex(0));
        runnerInstance.Shutdown();
        runnerInstance = null;
        
        DestroyImmediate(gameObject);
        Debug.LogWarning("LobbyManager foi destruído!");
    }


    private void SetupRunnerInstance()
    {
        print("SETUP CHAMADO");

        Debug.Log("runnerInstance: " + runnerInstance);
        Debug.Log("runnerPrefab: " + runnerPrefab);

        if (runnerInstance != null) return;

        if (runnerPrefab == null)
        {
            Debug.LogError("runnerPrefab está NULL ao tentar entrar no lobby! Verifique se está atribuído no objeto ativo na cena.");
            return;
        }

        //NetworkRunner net = Runner.Spawn(runnerPrefab);
        runnerInstance = Instantiate(runnerPrefab);
        runnerInstance.AddCallbacks(this);

        Debug.Log("runnerInstance: " + runnerInstance);
        Debug.Log("runnerPrefab: " + runnerPrefab);

    }
    // Bot�o: Criar Sala
    public async void OnCreateLobbyClicked()
    {
        Debug.Log("BOTÃO CRIAR SALA CLICADO");

        Debug.Log("runnerInstance: " + runnerInstance);
        Debug.Log("runnerPrefab: " + runnerPrefab);
        //runnerInstance = null; 
        SetupRunnerInstance();

        currentLobbyCode = GenerateRandomLobbyCode(6);
        //changeDifficultyButton.interactable = true;

        await StartGame(GameMode.Shared, currentLobbyCode);
    }

    // Bot�o: Procurar e entrar na sala
    public async void OnFindLobbyClicked()
    {
        Debug.Log("BOTÃO ENTRAR COM CÓDIGO CLICADO");

        Debug.Log("runnerInstance: " + runnerInstance);
        Debug.Log("runnerPrefab: " + runnerPrefab);

        //runnerInstance = null; 

        string lobbyCode = lobbyCodeInputField.text.Trim().ToUpper();

        if (string.IsNullOrEmpty(lobbyCode))
        {
            Debug.LogWarning("Código da sala está vazio.");
            return;
        }

        SetupRunnerInstance();

        pendingJoinLobbyCode = lobbyCode;

        // Entra no lobby de listagem de sessões
        var result = await runnerInstance.JoinSessionLobby(SessionLobby.Shared);

        if (!result.Ok)
        {
            Debug.LogError("Falha ao entrar no Session Lobby: " + result.ShutdownReason);
        }

        changeDifficultyButton.interactable = false;
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        if (string.IsNullOrEmpty(pendingJoinLobbyCode)) return;

        var session = sessionList.FirstOrDefault(s => s.Name == pendingJoinLobbyCode);

        if (session != null)
        {
            Debug.Log("Sessão encontrada: " + session.Name);
            StartGame(GameMode.Shared, session.Name);
            pendingJoinLobbyCode = null;
        }
        else
        {
            Debug.Log("Sessão com código " + pendingJoinLobbyCode + " não encontrada.");
        }
    }

    // Bot�o: Come�ar Jogo
    public async void OnStartGameClicked()
    {
        print("START GAME CLICKED");

        Debug.Log("runnerInstance: " + runnerInstance);
        Debug.Log("runnerPrefab: " + runnerPrefab);

        if (runnerInstance != null)
        {
            await runnerInstance.LoadScene(SceneRef.FromIndex(indexSceneForStart));
        }
    }

    ////////////////////////////////////////////////////////////
    async Task StartGame(GameMode mode, string lobbyCode)
    {
        print("START GAME GAMEMODE");

        Debug.Log("runnerInstance: " + runnerInstance);
        Debug.Log("runnerPrefab: " + runnerPrefab);
        networkRunner = runnerInstance; 

        SetupRunnerInstance();

        var args = new StartGameArgs()
        {
            GameMode = mode,
            SessionName = lobbyCode,
            
            Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
            SceneManager = runnerInstance.gameObject.AddComponent<NetworkSceneManagerDefault>()
        };

        await runnerInstance.StartGame(args);

        Debug.Log("Conectado � sala: " + lobbyCode);
        lobbyCodeText.text = lobbyCode;
    }

    string GenerateRandomLobbyCode(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        System.Random random = new System.Random();
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("runnerInstance: " + runnerInstance);
        Debug.Log("runnerPrefab: " + runnerPrefab);

        Debug.Log("/Player entrou: " + player.PlayerId);

        if (!connectedPlayers.Contains(player))
        {
            connectedPlayers.Add(player);
            print("ADICIONADO PLAYER ALISTA COM SUCESSO");
        }

        // Detecta se o jogador local é o primeiro a entrar
        if (runner.LocalPlayer == player && player.PlayerId == 1)
        {
            isFirstPlayer = true;
            Debug.Log("Sou o primeiro jogador (Host Lógico).");
        }
        else
        {
            print("não sou o primeiro não sou o Host");
        }

        OpenPainels(1);

        UpdateLobbyUI();
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.LogError($"Player {player.PlayerId} saiu");

        connectedPlayers.Remove(player);

        UpdateLobbyUI();
    }

    // Callbacks n�o usados (necess�rios para interface)
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, System.ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner)
    {
        GameObject cenaGame = GameObject.Find("CenaGame");

        Debug.Log("RUNNER: " + runner.IsServer);

        if (cenaGame != null)
        {
            if (runner.IsPlayer)
            {
                SpawnPlayer(runner, runner.LocalPlayer);
                print("CENA DE JOGO!");
            }
            
            //isFirstPlayer = playerCount > 0 && playerCount < 2;

            if (isFirstPlayer)
            {
                Debug.Log("Sou o primeiro jogador!");
                SpawnObject(runner);
            }
            else
            {
                Debug.Log("Não sou o primeiro jogador.");
            }
        }
        else
        {
            Debug.Log("Não Estamos Na Jogo, nenhuma spawn por enquanto.");
        }
    }

    private void SpawnObject(NetworkRunner runner)
    {
        // Pega todos os filhos com NetworkObject (inclusive os desativados)
        /*var networkObjects = HouseMultiplayer.GetComponentsInChildren<NetworkObject>(true);

        foreach (var netObj in networkObjects)
        {
            print("RUNNER IS RUNNIG: " + runner.IsRunning);

            if (netObj.tag == "Itens")
            {
                if (netObj == HouseMultiplayer) continue; // Evita spawnar o pai novamente
                if (netObj != null && !netObj.IsValid) continue;

                runner.Spawn(netObj, netObj.transform.position, netObj.transform.rotation);
                Debug.Log($"Spawnado objeto: {netObj.name}");
            }
        }*/
    }

    private void SpawnPlayer(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Spawnando player: " + player);

        if (runner.GetPlayerObject(player) != null)
        {
            Debug.Log("Jogador já possui avatar. Ignorando.");
            return;
        }

        int playerCount = runner.ActivePlayers.Count();
        print("JOGADORES NO MOMENTEO: " + playerCount);
        //-6.17F 7.18F 0.36F //-3.18
        
        Vector3 pos = spawnPoints[player.PlayerId - 1];

        if (runner.LocalPlayer == player && player.PlayerId == 1)
        {
            NetworkObject playerObj = runner.Spawn(playerPrefab1, pos, Quaternion.identity, inputAuthority: runner.LocalPlayer);
            playerObjects.Add(player, playerObj);
        }
        else
        {
            NetworkObject playerObj = runner.Spawn(playerPrefab2, pos, Quaternion.identity, inputAuthority: runner.LocalPlayer);
            playerObjects.Add(player, playerObj);
        }

        //Debug.Log("Player spawnado: " + playerObj.name); 
    }

    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        throw new NotImplementedException();
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        throw new NotImplementedException();
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        throw new NotImplementedException();
    }
    private void UpdateLobbyUI()
    {
        int count = connectedPlayers.Count;

        if (count >= 1)
            player1StatusText.text = "Player 1: Conectado";
        else
            player1StatusText.text = "Player 1: Aguardando...";

        if (count >= 2)
        {
            player2StatusText.text = "Player 2: Conectado";

            // Apenas o "host lógico" pode alterar dificuldade
            changeDifficultyButton.interactable = isFirstPlayer;

            /*else
                changeDifficultyButton.interactable = false;*/
           
            //startGameButton.interactable = true;
        }
        else
        {
            player2StatusText.text = "Player 2: Aguardando...";
            startGameButton.interactable = false;
            //changeDifficultyButton.interactable = true;
        }
    }

    public void OnLeaveLobbyClicked()
    {
        print("LEAVE LOBBY CLICKED");

        Debug.Log("runnerInstance: " + runnerInstance);
        Debug.Log("runnerPrefab: " + runnerPrefab);

        if (runnerInstance != null)
        {
            runnerInstance.Shutdown();
            runnerInstance = null;

            Debug.Log("runnerInstance: " + runnerInstance);
            Debug.Log("runnerPrefab: " + runnerPrefab);
        }

        OpenPainels(0); // volta pro menu
        connectedPlayers.Clear(); // limpa lista local
        player1StatusText.text = "Player 1: Aguardando...";
        player2StatusText.text = "Player 2: Aguardando...";
        startGameButton.interactable = false;
        changeDifficultyButton.interactable = false;
    }
    
    public void CloseRunnerIfNotInLobby()
    {
        Debug.Log("runnerInstance: " + runnerInstance);
        Debug.Log("runnerPrefab: " + runnerPrefab);

        // Suponha que o índice 1 seja o painel de lobby (ajuste se necessário)
        bool isInLobby = painels[1].activeSelf;

        if (!isInLobby && runnerInstance != null && runnerInstance.LocalPlayer == runnerInstance.LocalPlayer)
        {
            Debug.Log("Fechando Runner pois não está no painel de lobby.");
            runnerInstance.Shutdown();
            runnerInstance = null;
        }
    }

    public void OpenPainels(int index)
    {
        Debug.Log("BOTÃO TROCANDO DE PAINEL CLICADO");

        for (int i = 0; i < painels.Length; i++)
        {
            if (i == index)
            {
                painels[index].SetActive(true);
            }
            else
            {
                painels[i].SetActive(false);
            }
        }
    }

    public void Open(int index)
    {
        if (index == 0)
        {
            painelsHost[index].SetActive(true);
            painelsHost[1].SetActive(false);

            if (isFirstPlayer)
                changeDifficultyButton.interactable = true;
            else
                changeDifficultyButton.interactable = false;
        }
        else if (index == 1)
        {
            painelsHost[index].SetActive(true);
            painelsHost[0].SetActive(false);
        }
    }

    public void ChangeDificculty(int dificulty)
    {
        if (!isFirstPlayer)
        return;

        if (dificulty > 0 && dificulty <= 3)
        {
            PlayerPrefs.SetInt("Dificulty", dificulty);
            startGameButton.interactable = true;
        }
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
        OnLeaveLobbyClicked();
        DestroyImmediate(gameObject);
    }
}
