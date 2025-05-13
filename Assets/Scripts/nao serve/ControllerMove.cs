using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMove : MonoBehaviour
{
    public float velocidadeMaxima = 5f; // Velocidade máxima do player

    public float suavizacao = 10f; // Taxa de suavização

    private float direcao = 0f; // Direção do movimento (-1 = esquerda, 1 = direita, 0 = parado)

    private float velocidadeAtual = 0f; // Velocidade real aplicada ao player

    void Update(){

        //Suaviza a transição de velocidade usando Lerp
        velocidadeAtual = Mathf.Lerp(velocidadeAtual, direcao * velocidadeMaxima, Time.deltaTime * suavizacao);
        
        // Move o player suavemente
        transform.position += new Vector3(velocidadeAtual * Time.deltaTime, 0, 0);
    }

    public void MovimentarEsquerda(){
        direcao = -1f;
    }

    public void MovimentarDireita(){
        direcao = 1f;
    }

    public void Parar(){
        direcao = 0f;
    }
}
