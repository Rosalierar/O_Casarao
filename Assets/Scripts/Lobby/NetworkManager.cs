using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;

public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkPrefabRef _playerPrefab;

    private bool _playersSpawned = false;
    private Dictionary<PlayerRef, NetworkObject> _spawnedPlayers = new Dictionary<PlayerRef, NetworkObject>();

    /*public override void Spawned()
    {
        print("METODO SPAWNOU FOI CHAMADO");

        print(Runner.IsSharedModeMasterClient + " " + Object.HasStateAuthority);
        //if (Runner.IsSharedModeMasterClient || _playersSpawned)
        //    return;

        //_playersSpawned = true;

        if (Object.HasStateAuthority)
        {
            print("Tenho StateAutority");
            // Garante que cada jogador só se spawne uma vez
            if (!HasPlayerAlreadySpawned(Runner.LocalPlayer))
            {
                var spawnPos = GetSpawnPosition(Runner.LocalPlayer);
                Runner.Spawn(_playerPrefab, spawnPos, Quaternion.identity, Runner.LocalPlayer);
            }
        }

        /*foreach (var player in Runner.ActivePlayers)
        {
            Vector3 spawnPosition = GetSpawnPosition(player);

            var character = Runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            _spawnedPlayers.Add(player, character);

            Debug.Log($"Player {player} spawnado em {spawnPosition}");
        }
    }
    bool HasPlayerAlreadySpawned(PlayerRef player)
    {
        foreach (var kvp in _spawnedPlayers)
        {
            if (kvp.Key == player)
                return true;
        }
        return false;
    }

    private Vector3 GetSpawnPosition(PlayerRef player)
    {
        // Simples alternador de posi��o baseado no ID
        int index = player.PlayerId;
        float offset = 3f;
        return new Vector3((index % 2 == 0 ? -1 : 1) * offset, 0, 0);
    }*/

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new System.NotImplementedException();
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new System.NotImplementedException();
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {

    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        throw new System.NotImplementedException();
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        throw new System.NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        throw new System.NotImplementedException();
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        throw new System.NotImplementedException();
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        throw new System.NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, System.ArraySegment<byte> data)
    {
        throw new System.NotImplementedException();
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        throw new System.NotImplementedException();
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        throw new System.NotImplementedException();
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        throw new System.NotImplementedException();
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        throw new System.NotImplementedException();
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        throw new System.NotImplementedException();
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        throw new System.NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        throw new System.NotImplementedException();
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        foreach (var player in runner.ActivePlayers)
        {
            if (player == runner.LocalPlayer) // ou runner.IsServer se tiver usando Server/Client
            {
                NetworkObject playerObj = runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, inputAuthority: player);
                runner.SetPlayerObject(player, playerObj);
            }
        }
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        throw new System.NotImplementedException();
    }

    /*[SerializeField] private NetworkPrefabRef _playerPrefab;
    private List<PlayerRef> _spawnedCharacters = new List<PlayerRef>();
    public override void Spawned()
    {
        if (Runner.IsServer)
        {
            foreach (var player in Runner.ActivePlayers)
            {
                _spawnedCharacters.Add(player);
                Debug.Log($"Player conectado: {player}");

                Vector3 spawnPosition = player.PlayerId == 0 ? new Vector3(-3, 0, 0) : new Vector3(3, 0, 0);
                Runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            }
        }
    }*/
}
