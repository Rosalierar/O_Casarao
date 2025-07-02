using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class GameManagerNet : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] private NetworkObject housePrefab;
    [SerializeField] private NetworkPrefabRef playerPrefab;
    [Networked, Capacity(12)] private NetworkDictionary<PlayerRef, NetMovePlayer> Players => default;

    private Vector3[] spawnPoints = new Vector3[2]
    {
        //new Vector3(0f, 0f, 0f),
        //new Vector3(0f, 0f, 0f),
        new Vector3(-3.18f, 7.18f, 0.36f),
        new Vector3(-6.17f, 7.18f, 0.36f),
        //new Vector3(-3.18f, 7.18f, 0.36f)
    };

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            Vector3 pos = new Vector3(-0.378198445f, 8.20464134f, -1.46559298f);

            Debug.Log("GameManagerNet Spawned. Tem autoridade? " + HasStateAuthority);
            NetworkObject house = Runner.Spawn(housePrefab, pos, Quaternion.identity);

            SpawnPlayers();

            SpawnObjectsFromHouse(house);
        }
    }

    void SpawnObjectsFromHouse(NetworkObject house)
    {
        // Pega todos os filhos com NetworkObject (inclusive os desativados)
        var networkObjects = house.GetComponentsInChildren<NetworkObject>(true);

        foreach (var netObj in networkObjects)
        {
            print("RUNNER IS RUNNIG: " + Runner.IsRunning);

            if (netObj.tag == "Itens")
            {


            if (netObj == house) continue; // Evita spawnar o pai novamente
            if (netObj != null && !netObj.IsValid) continue;

            Runner.Spawn(netObj, netObj.transform.position, netObj.transform.rotation);
            Debug.Log($"Spawnado objeto: {netObj.name}");
            }
        }
    }

    public void PlayerJoined(PlayerRef player)
    {
        /*if (HasStateAuthority)
        {
            int index = player.RawEncoded % spawnPoints.Length;
            Vector3 spawnPosition = spawnPoints[index];

            NetworkObject playerObj = Runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, inputAuthority: player);
            Players.Add(player, playerObj.GetComponent<NetMovePlayer>());

            Debug.Log("Player entrou: " + player.PlayerId);
        }*/
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (!HasStateAuthority)
            return;

        if (Players.TryGet(player, out NetMovePlayer playerBehaviour))
        {
            Players.Remove(player);
            Runner.Despawn(playerBehaviour.Object);

            Debug.Log("Player saiu: " + player.PlayerId);
        }
    }
    
    void SpawnPlayers()
    {
        /*print("Chamou AQUI");
        foreach (var player in Runner.ActivePlayers)
        {
            if (!Players.ContainsKey(player))
            {
                int index = player.RawEncoded % spawnPoints.Length;
                Vector3 spawnPos = spawnPoints[index];

                NetworkObject obj = Runner.Spawn(playerPrefab, spawnPos, Quaternion.identity, inputAuthority: player);
                Players.Add(player, obj.GetComponent<NetMovePlayer>());

                Debug.Log("Spawn manual do jogador: " + player);
            }
        }*/
    }
}
