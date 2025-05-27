using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassosController : MonoBehaviour
{
    [SerializeField] private AudioSource audioPassosSource;
    [SerializeField] private AudioClip[] audioPassosWClip;
    [SerializeField] private AudioClip[] audioPassosRClip;
    [SerializeField] private AudioClip[] audioPassosCClip;

    private void Passos()
    {
        audioPassosSource.PlayOneShot(audioPassosWClip[Random.Range(0, audioPassosWClip.Length)]);
    }
    private void PassosRun()
    {
        audioPassosSource.PlayOneShot(audioPassosRClip[Random.Range(0, audioPassosRClip.Length)]);
    }

    private void PassosCrouch()
    {
        audioPassosSource.PlayOneShot(audioPassosCClip[Random.Range(0, audioPassosCClip.Length)]);
    }

}
