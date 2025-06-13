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
using WebSocketSharp;
using UnityEngine.SocialPlatforms;

public class LoobyManager : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private int indexSceneForStart;
    [SerializeField] private NetworkManager networkManager;
    public static LoobyManager Instance;

    [Header("Player Prefab")]
    [SerializeField] private NetworkObject playerPrefab;

    private List<PlayerRef> connectedPlayers = new List<PlayerRef>();

    [Header("UI")]
    [SerializeField] private GameObject[] painels = new GameObject[3];
    private string pendingJoinLobbyCode;

    public TMP_InputField lobbyCodeInputField;
    public TMP_Text lobbyCodeText;
    public TMP_Text player1StatusText;
    public TMP_Text player2StatusText;
    public Button startGameButton;

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
    }
    
    void OnDestroy()
    {
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

        /*
        Debug.Log("runnerInstance: " + runnerInstance);
        Debug.Log("runnerPrefab: " + runnerPrefab);

        string lobbyCode = lobbyCodeInputField.text.Trim().ToUpper();

        if (string.IsNullOrEmpty(lobbyCode))
        {
            Debug.LogWarning("Código da sala está vazio.");
            return;
        }

        await StartGame(GameMode.Client, lobbyCode);

        /*string lobbyCode = lobbyCodeInputField.text.Trim().ToUpper();

        if (string.IsNullOrEmpty(lobbyCode))
        {
            Debug.LogWarning("Codigo da sala est� vazio.");
            return;
        }

        SetupRunnerInstance();

        // Entra no lobby
        var result = await runnerInstance.JoinSessionLobby(SessionLobby.ClientServer);

        if (result.Ok)
        {
            Debug.Log("Entrou no lobby, aguardando lista de sessoes...");
            pendingJoinLobbyCode = lobbyCode; // Armazena para comparar no callback
        }
        else
        {
            Debug.LogError("Erro ao entrar no lobby: " + result.ShutdownReason);
        }
        */
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

        /*
            Debug.Log("runnerInstance: " + runnerInstance);
            Debug.Log("runnerPrefab: " + runnerPrefab);

            if (string.IsNullOrEmpty(pendingJoinLobbyCode)) return;

            var session = sessionList.FirstOrDefault(s => s.Name == pendingJoinLobbyCode);

            if (session != null)
            {
                Debug.Log("Sess�o encontrada, entrando: " + session.Name);
                StartGame(GameMode.Shared, session.Name);
                pendingJoinLobbyCode = null;
            }
            else
            {
                Debug.Log("Sessao com c�digo " + pendingJoinLobbyCode + " nao encontrada.");
            }
        */
    }

    // Bot�o: Come�ar Jogo
    public async void OnStartGameClicked()
    {
        print("START GAME CLICKED");

        Debug.Log("runnerInstance: " + runnerInstance);
        Debug.Log("runnerPrefab: " + runnerPrefab);

        if (runnerInstance != null)
        {
            // Carrega Cena 2
            //runnerInstance.Shutdown();
            //SceneManager.LoadScene(1); // Certifique-se de que a Cena 2 est� no Build Index 1
            //runnerInstance.SetActiveScene(SceneRef.FromIndex(1));

            //int currentSceneIndex = runnerInstance.SceneRef.FromIndex(1);
            await runnerInstance.LoadScene(SceneRef.FromIndex(indexSceneForStart));
        }
    }

    ////////////////////////////////////////////////////////////
    async Task StartGame(GameMode mode, string lobbyCode)
    {
        print("START GAME GAMEMODE");

        Debug.Log("runnerInstance: " + runnerInstance);
        Debug.Log("runnerPrefab: " + runnerPrefab);

        //if (isStartingGame) return;
        //isStartingGame = true; 

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

        Debug.Log("Player entrou: " + player.PlayerId);

        if (!connectedPlayers.Contains(player))
            connectedPlayers.Add(player);

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
        Scene fusionScene = SceneManager.GetActiveScene(); // obtém a cena ativa do Unity
        //int buildIndex = fusionScene.SceneRef; // índice da cena ativa
        string sceneName = fusionScene.name; // nome da cena ativa

        //Debug.Log($"Cena carregada: {sceneName} (índice: {buildIndex})");
        /*// Pega o índice da cena atual carregada no Unity
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        string currentSceneName = SceneManager.GetActiveScene().name;

        Debug.Log($"Cena carregada: {currentSceneName} (índice: {currentSceneIndex})");
            print("RUNNER IS PLAYER:" + runner.IsPlayer);

        if (currentSceneIndex == 2) // ou "Cutscene"
        {
            // Ações específicas para a cena 2 (cutscene)
            Debug.Log("Estamos na cutscene, nenhuma spawn por enquanto.");
        }

        else if (currentSceneIndex == 1  ) // ou "Gameplay"
        {
            if (runner.IsPlayer)
            {
                foreach (var player in connectedPlayers)
                {
                    SpawnPlayer(runner, player);
                }
            }
        }
        else
        {
            Debug.Log("Cena desconhecida, nenhuma ação definida.");
        }*/

        if (runner.IsPlayer)
        {
            SpawnPlayer(runner, runner.LocalPlayer);
        }

    }

    private void SpawnPlayer(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Spawnando player: " + player);

        if (runner.GetPlayerObject(player) != null)
        {
            Debug.Log("Jogador já possui avatar. Ignorando.");
            return;
        }

        Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-5f, 5f), 1f, UnityEngine.Random.Range(-5f, 5f));

        NetworkObject playerObj = runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, inputAuthority: runner.LocalPlayer); //ou só Player

        Debug.Log("Player spawnado: " + playerObj.name);
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
            startGameButton.interactable = true;
        }
        else
        {
            player2StatusText.text = "Player 2: Aguardando...";
            startGameButton.interactable = false;
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

        //CloseRunnerIfNotInLobby();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
