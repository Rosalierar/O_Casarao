using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonsAleatorios : MonoBehaviour
{
    public AudioClip[] sons;
    public float intervaloMin, intervaloMax;
    private AudioSource AS;
    private Coroutine Loop;

    void Awake()
    {
        AS = GetComponent<AudioSource>();
        AS.loop = false;
        AS.clip = null;
    }

   
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
            yield return new WaitForSeconds(Random.Range(intervaloMin, intervaloMax));

            int random = Random.Range(0, sons.Length);

            AudioClip somEscolhido = sons[random];

            if (somEscolhido != null)
            {
                AS.PlayOneShot(somEscolhido);
            }
            else
            {
                Debug.LogWarning($"O clipe n�o foi tocado.");
            }
        }
    }

}
