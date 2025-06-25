using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Fusion;

public class JoyRoots : MonoBehaviour, IPointerUpHandler, IDragHandler, IPointerDownHandler
{
    NetworkRunner runner;
    NetworkObject networkObject;
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
        try
        {
            runner = FindObjectOfType<NetworkRunner>();
            print(runner);
        }
        catch
        {
            print("Runer erro no JoyRoots");
        }

        if (runner != null)
        {
            networkObject = GetComponentInParent<NetworkObject>();
            print(runner);

            if (networkObject.HasInputAuthority)
            {
                // Obtém a referência à imagem do joystick e salva a posição inicial.
                thisImage = GetComponent<Image>();
                startPos = transform.position;
            }
        }
        else if (runner == null)
        {
            // Obtém a referência à imagem do joystick e salva a posição inicial.
            thisImage = GetComponent<Image>();
            startPos = transform.position;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Exceção lançada indicando que o método ainda não foi implementado.
        //throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData){
        if (PodeExecutar())
        {
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
    }
    public void OnPointerUp(PointerEventData eventData){
        if (PodeExecutar())
        {
            // Retorna o joystick para a posição inicial.
            transform.position = startPos;

            // Reseta os eixos virtuais para 0.
            UpdateVirtualAxes(Vector3.zero);
        }
    }
    
    bool PodeExecutar()
    {
        // Se tiver NetworkObject e InputAuthority → multiplayer local
        if (TryGetComponent<NetworkObject>(out var netObj))
            return netObj.HasInputAuthority;

        // Se não tem NetworkObject → estamos no singleplayer
        return true;
    }

    // Método responsável por atualizar os eixos virtuais do joystick.

    void UpdateVirtualAxes(Vector3 value)
    {
        if (PodeExecutar())
        {
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
}
