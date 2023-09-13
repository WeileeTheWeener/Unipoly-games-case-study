using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteractionComponent : MonoBehaviour
{
    private void Update()
    {
        HandleObjectDrop();
    }
    private void HandleObjectDrop()
    {
        if (gameObject.GetComponentInChildren<CollectibleComponent>() != null)
        {
            CollectibleComponent collectible = gameObject.GetComponentInChildren<CollectibleComponent>();
            if (Input.GetKeyDown(KeyCode.G) && !collectible.isCollectible)
            {
                collectible.DropCollectible();
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        //can pickup collectible if player is inside its collider
        if(other.gameObject.GetComponent<CollectibleComponent>() != null )
        {
            CollectibleComponent collectible = other.gameObject.GetComponent<CollectibleComponent>();

            if(Input.GetKeyDown(KeyCode.E) && collectible.isCollectible)
            {
                collectible.Interact(gameObject);
            }
            

        }
    }
    

}
