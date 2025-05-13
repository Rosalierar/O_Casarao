using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionObjects : MonoBehaviour
{
    // Sobre os ITENS
    public ParentObjectReference parent;
    public Item item;
    public InteractiveObject interactiveObject; 

    CameraTouchController cameraTouchController; // Referência ao script MovePlayer
    [SerializeField] private Transform rayObjOrigin; // Origem do Raycast
    [SerializeField] LayerMask layerObject;
    [SerializeField] float distanceRay;  

    public bool isCollidingItem, isCollidingInteractiveObj; // Variável para verificar se o objeto está colidindo com o jogador

    private void Awake()
    {
        parent = GetComponent<ParentObjectReference>();
    }
    // Start is called before the first frame update
    void Start()
    {
        cameraTouchController = GetComponent<CameraTouchController>(); // Obtém a referência ao script MovePlayer
    }

    // Update is called once per frame
    void Update()
    {
        ActiveRayCast(); 
    }

    void ActiveRayCast() {

        //Vector3 originRayObj = rayObjOrigin.position; // Posição do Raycast

        RaycastHit hitObject; // Variável para armazenar o resultado do Raycast
        Physics.Raycast(rayObjOrigin.position,  cameraTouchController.lookDirection, out hitObject, distanceRay, layerObject);
        Debug.DrawRay(rayObjOrigin.position, cameraTouchController.lookDirection /** 10*/, Color.blue);

        if (hitObject.collider !=null)
        {
            try{
                interactiveObject = hitObject.collider.GetComponent<InteractiveObject>(); // Obtém o componente InteractiveObject do objeto atingido pelo Raycast
                interactiveObject.SetParentReference(parent);
            }
            catch (System.Exception e) // Se não conseguir pegar o componente, ignora
            {
                Debug.Log("Erro ao pegar o componente interactive: " + e.Message);
            }
            try{
                item = hitObject.collider.GetComponent<Item>(); // Obtém o componente Item do objeto atingido pelo Raycast
            }
            catch (System.Exception e) // Se não conseguir pegar o componente, ignora
            {
                Debug.Log("Erro ao pegar o componente item: " + e.Message);
            }
          
            isCollidingInteractiveObj = interactiveObject != null; //Se interactiveObject não for null, então isCollidingInteractiveObj será true. Se for null, será false
            isCollidingItem = item != null;

            if (isCollidingInteractiveObj) // Se o objeto atingido for um InteractiveObject
            {
                //parent = GetComponentInChildren<ParentObjectReference>();
                
                parent.useTheObject.enabled = true; // Atribui o objeto atingido ao ParentObjectReference
                parent.grabTheObject.enabled = false; // Limpa a referência ao Item
            }

            if (isCollidingItem) // Se o objeto atingido for um Item
            {
                
                parent.grabTheObject.enabled = true;
                parent.useTheObject.enabled = false;
            }
        }
        else{
            isCollidingInteractiveObj = false; // Se o Raycast não atingir nada, define como false
            isCollidingItem = false; // Se o Raycast não atingir nada, define como false
            
            parent.useTheObject.enabled = false; // Desabilita o script de usar
            parent.grabTheObject.enabled = false; // Desabilita o script de pegar
        }
    } 
}
