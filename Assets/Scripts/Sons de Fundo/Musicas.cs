using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musicas : MonoBehaviour
{
    public AudioClip som, som2; 
    private AudioSource audio, audio2;

    void Awake()
    {
        AudioSource[] existingSources = GetComponents<AudioSource>();

        AudioSource[] todosAudioSources = GetComponents<AudioSource>();
        audio = todosAudioSources[0];
        audio2 = todosAudioSources[1];

        audio.loop = true;
        audio.clip = som;

        audio2.loop = true;
        audio2.clip = som2;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (som != null)
        {
            audio.Play();
        }
        else
        {
            Debug.LogWarning("não tem audioclip atribuido " + gameObject.name);
        }

        if (som2 != null)
        {
            audio2.Play();
        }
        else
        {
            Debug.LogWarning("não tem audioclip atribuido " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
