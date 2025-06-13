using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;

public class PlayerSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkPrefabRef objectToSpawn;

    private NetworkRunner _runner;

    void Awake()
    {
        _runner = FindFirstObjectByType<NetworkRunner>();
        if (_runner != null)
        {
            _runner.AddCallbacks(this);
        }
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        if (runner.IsSharedModeMasterClient)
        {
            StartCoroutine(SpawnPlayersWithDelay(runner));
        }
    }

    private IEnumerator SpawnPlayersWithDelay(NetworkRunner runner)
    {
        // Espera um pequeno tempo para garantir que os PlayerRefs estejam prontos
        yield return new WaitForSeconds(0.5f); // pode ajustar para 1f se necess�rio

        Debug.Log("Cena carregada. Fazendo spawn dos jogadores...");

        foreach (var player in runner.ActivePlayers)
        {
            Debug.Log($"Spawnando para PlayerRef {player}");

            Vector3 spawnPos = GetSpawnPosition(player);

            NetworkObject playerNet = runner.Spawn(objectToSpawn, spawnPos, Quaternion.identity, player);

            Debug.Log($"Spawnado: {playerNet.name} para {player} | InputAuthority: {playerNet.InputAuthority}");
        }
    }

    private Vector3 GetSpawnPosition(PlayerRef player)
    {
        // Simples alternador de posi��o baseado no ID
        int index = player.PlayerId;
        float offset = 3f;
        return new Vector3((index % 2 == 0 ? -1 : 1) * offset, 0, 0);
    }

    // Outros callbacks obrigat�rios (vazios)
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, System.ArraySegment<byte> data) { }
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

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        throw new NotImplementedException();
    }
}
