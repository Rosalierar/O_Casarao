using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    ParentObjectReference parent; // Referência ao objeto pai
    [SerializeField] private Rigidbody rbObj;
    [SerializeField] private Transform localDeDrop; // Local onde o item será solto
    [SerializeField] private Inventory inventario; // Referência ao inventário do jogador
    public TipoDeItem tipoDoItem;
    private string descricao;

    // Método chamado quando o jogador coleta o item
    /*public void Coletar()
    {
        whatItem = this;
        inventario.itemCarregado = whatItem; // Atualiza a referência do item carregado no inventário
        painelInventario.sprite = image.sprite;// Atualiza a imagem do item no painel de inventário

        if (inventario.itens.Count == 0)
        {
            Debug.Log("Item coletado: " + descricao);
            inventario.AdicionarItem(whatItem); // Adiciona o item ao inventário

              // codigo para Colocar para cOLOCAR DO CONVA ************
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Você já tem um item no inventário!");
        }
    }

    public void Soltar()
    {
        if (inventario.itens.Contains(this)) // Verifica se o item está no inventário
        {
            inventario.itens.Remove(this); // Remove o item do inventário
            inventario.itemCarregado = null;

            gameObject.SetActive(true); // Ativa o objeto do item no mundo

            rbObj.useGravity = true;
            painelInventario.sprite = null; // Remove a imagem do item no painel de inventário
            
            transform.position = localDeDrop.position;

            //codigo para Colocar para TiRAR DO CONVA ************

            Debug.Log("Item solto: " + descricao);
            //whatItem = null;
            //inventario.itemCarregado = whatItem; // Atualiza a referência do item carregado no inventário
        }
    }*/
    /*void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            parent = other.GetComponentInChildren<ParentObjectReference>();

            parent.grabTheObject.enabled = true; 
            parent.useTheObject.enabled = false; // Desabilita o script grabTheObject se o objeto estiver sendo segurado para usar apenas o script de interação
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica se o objeto que saiu da colisão tem a tag "Player"
        {
            parent.grabTheObject.enabled = false; // Desabilita o script grabTheObject se o objeto estiver sendo segurado para usar apenas o script de interação
            parent.useTheObject.enabled = true; // Habilita o script grabTheObject se o objeto não estiver mais sendo segurado para usar apenas o script de pegar
            parent.detectionObjects.isCollidingItem = false; // Desabilita a detecção de colisão
        
        }
    }*/
}

