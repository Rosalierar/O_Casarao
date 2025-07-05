using UnityEngine;
using Fusion;
using System.Linq;

public class GameManagerNet : NetworkBehaviour
{
    [SerializeField] private NetworkObject housePrefab;
    [SerializeField] private NetworkObject patraoPrefab;

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            Vector3 posH = new Vector3(-0.378198445f, 8.20464134f, -1.46559298f);
            Vector3 posP = new Vector3(-13.6618013f, -1.07464123f, -7.29440737f);

            Debug.Log("GameManagerNet Spawned. Tem autoridade? " + HasStateAuthority);
            NetworkObject house = Runner.Spawn(housePrefab, posH, Quaternion.identity);
            //NetworkObject patrao = Runner.Spawn(patraoPrefab, posP, Quaternion.identity);

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

            if (netObj.tag == "Itens" || netObj.tag == "Portas")
            {
                if (netObj == house) continue; // Evita spawnar o pai novamente
                if (netObj != null && !netObj.IsValid) continue;

                Runner.Spawn(netObj, netObj.transform.position, netObj.transform.rotation);
                Debug.Log($"Spawnado objeto: {netObj.name}");
            }
        }
    }
}
