using UnityEngine;
using Fusion;

public class EnabledPlayers : MonoBehaviour
{
    private NetworkRunner runner;


    // Update is called once per frame
    void Update()
    {
        try
        {
            runner = FindObjectOfType<NetworkRunner>();
        }
        catch
        {
            print("Não está no Multiplayer");
        }

        if (runner != null)
        {
            foreach (GameObject playerObj in GameObject.FindGameObjectsWithTag("Player"))
            {
                playerObj.SetActive(false);
            }
            print("Multiplayer");
        }
        else
        {
            print("Não Multiplayer");
        }
    }
}
