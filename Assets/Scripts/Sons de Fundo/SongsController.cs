using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongsController : MonoBehaviour
{
    public AudioClip[] songsBackGround = new AudioClip[5]; //MUSICA DE CHAVA, MUSICA DE FUNDO, PERSEGUIÇÃO e ESCONDER
    public AudioSource[] audioSorceBackGround;

    //SOM ALEATORIOS
    public AudioClip[] sonsAleatorios;
    public float intervaloMin, intervaloMax;
    [SerializeField] private AudioSource AS;
    private Coroutine Loop;

    void Awake()
    {
        AS.loop = false;
        AS.clip = null;

        for (int i = 0; i < audioSorceBackGround.Length; i++)
        {
            audioSorceBackGround[i].loop = true;
            audioSorceBackGround[i].clip = songsBackGround[i];

            OptionsController OP = FindObjectOfType<OptionsController>();

            if (OP.sliderVolume[0].value > OP.sliderVolume[1].value)
            AS.volume = OP.sliderVolume[0].value;
            else
            AS.volume = OP.sliderVolume[1].value;
            
            if (songsBackGround[i] != null && i < 2)
            {
                audioSorceBackGround[i].Play();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (sonsAleatorios.Length > 0)
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

            int random = Random.Range(0, sonsAleatorios.Length);

            AudioClip somEscolhido = sonsAleatorios[random];

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
