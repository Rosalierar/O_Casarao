using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashingMachineController : MonoBehaviour
{
    [SerializeField] AudioClip[] audios;
    AudioSource audioSource;

    public void StartTime()
    {
        StartCoroutine(TimerWashingClothes());
    }

    IEnumerator TimerWashingClothes()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audios[0];
        audioSource.Play();

        yield return new WaitForSeconds(8f);

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audios[1];
        audioSource.Play();

        GetComponent<InteractiveObject>().unlocked = true;
    }
}
