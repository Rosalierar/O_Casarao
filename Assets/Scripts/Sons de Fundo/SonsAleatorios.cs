using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonsAleatorios : MonoBehaviour
{
    public AudioClip[] sons;
    public float intervalo = 30f;
    private AudioSource audio;
    private Coroutine Loop;

    void Awake()
    {
        audio = GetComponent<AudioSource>();
        audio.loop = false;
        audio.clip = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (sons.Length > 0)
        {
            Loop = StartCoroutine(TocarAleatorios());
        }
        else
        {
            Debug.LogWarning("Nenhum som atribuido" + gameObject.name);
        }
    }

    private IEnumerator TocarAleatorios()
    {
        while (true) 
        {
            yield return new WaitForSeconds(intervalo);

            int random = Random.Range(0, sons.Length);

            AudioClip somEscolhido = sons[random];

            if (somEscolhido != null)
            {
                audio.PlayOneShot(somEscolhido);
            }
            else
            {
                Debug.LogWarning($"O clipe não foi tocado.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
