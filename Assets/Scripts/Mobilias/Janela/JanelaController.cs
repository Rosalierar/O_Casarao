using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JanelaController : MonoBehaviour
{
    [SerializeField] private byte lifeLost = 0;
    public GameObject[] railings;

    private void OnEnable()
    {
        MovePlayer.OnLifeLost += AtivarGrade;
    }

    private void OnDisable()
    {
        MovePlayer.OnLifeLost -= AtivarGrade;
    }

    void AtivarGrade()
    {
        for (int index = 0; index < railings.Length; index++)
        {
            if (railings != null && index == lifeLost)
                railings[index].SetActive(true);
        }

        lifeLost++;
    }
}
