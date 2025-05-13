using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class JoyRoots : MonoBehaviour, IPointerUpHandler, IDragHandler, IPointerDownHandler
{
    // Define o raio máximo de movimentação do joystick.
    public int movimentRange = 100;

    // Posição inicial do joystick.
    Vector3 startPos;

    // Direção do input do jogador.
    public Vector3 inputDirection; 

    // Imagem de fundo do joystick.
    public Image bgImage;

    // Referência à imagem do próprio joystick.
    private Image thisImage;

    // Start is called before the first frame update
    void Start()
    {
        // Obtém a referência à imagem do joystick e salva a posição inicial.
        thisImage = GetComponent<Image>();
        startPos = transform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Exceção lançada indicando que o método ainda não foi implementado.
        //throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData){
       Vector3 newPos = Vector3.zero; 

       // Calcula o deslocamento horizontal (eixo X) e limita dentro do raio máximo.
       int delta = (int)(eventData.position.x - startPos.x);
       delta = Math.Clamp(delta, -movimentRange, movimentRange);
       newPos.x = delta;

       // Calcula o deslocamento vertical (eixo Y) e limita dentro do raio máximo.
       int delta2 = (int)(eventData.position.y - startPos.y);
       delta2 = Math.Clamp(delta2, -movimentRange, movimentRange);
       newPos.y = delta2;

       // Atualiza a posição do joystick na tela.
       transform.position = new Vector3(startPos.x + newPos.x, startPos.y + newPos.y, startPos.z + newPos.z);

       // Normaliza os valores para que fiquem entre -1 e 1 e atualiza os eixos virtuais.
       UpdateVirtualAxes(new Vector3(newPos.x * 1f / movimentRange, newPos.y * 1f / movimentRange, 0));
    }
    public void OnPointerUp(PointerEventData eventData){

        // Retorna o joystick para a posição inicial.
        transform.position = startPos;   

        // Reseta os eixos virtuais para 0.
        UpdateVirtualAxes(Vector3.zero);
    }

// Método responsável por atualizar os eixos virtuais do joystick.

    void UpdateVirtualAxes(Vector3 value){

        // Caulcula a diferença entre a posição inicial e o valor atal.
        var delta = startPos - value;   

        // Inverte o eixo Y para manter a coerência da movimentação.
        delta.y = -delta.y;  

        // Normaliza a diferença pelo raio de movimentação.
        delta /= movimentRange;

        // Atualiza a direção do input do jogador.
        inputDirection = value;
    }
}
