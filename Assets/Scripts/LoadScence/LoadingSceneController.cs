using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    [SerializeField] int sceneToLoadIndex; // Index of the scene to load (0 is the first scene, 1 is the second, etc.)
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScene(sceneToLoadIndex)); // Start the coroutine to load the scene
    }

    IEnumerator LoadScene(int sceneName)
    {
        // Start loading the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
            Debug.Log("Loading scene: " + asyncLoad.progress * 100 + "%"); // Log the loading progress (0 to 1)
        }
    }
}
