using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveJN : MonoBehaviour
{
    public JoyRoots move;

    public float velocidadeMaxima = 5f; // Velocidade máxima do player
    public float suavizacao = 10f; // Taxa de suavização
    private float direcao = 0f; // Direção do movimento (-1 = esquerda, 1 = direita, 0 = parado)
    private float velocidadeAtualX = 0f; // Velocidade real aplicada ao player
    private float velocidadeAtualY = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         // Suaviza a transição de velocidade usando Lerp
        velocidadeAtualX = Mathf.Lerp(velocidadeAtualX, move.inputDirection.x * velocidadeMaxima, Time.deltaTime * suavizacao);
        velocidadeAtualY = Mathf.Lerp(velocidadeAtualY, move.inputDirection.y * velocidadeMaxima, Time.deltaTime * suavizacao);
        // Move o player suavemente
        transform.position += new Vector3(velocidadeAtualX * Time.deltaTime, velocidadeAtualY * Time.deltaTime, 0);
    }
}
