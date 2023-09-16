using Fusion;
using UnityEngine;

public class PlayerInteractionComponent : NetworkBehaviour
{
    public bool pickupPressed;

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            HandleObjectDrop(data.isReleaseButtonPressed);
            pickupPressed = data.isCollectButtonPressed;
        }
    }
    public void HandleObjectDrop(bool dropPressed)
    {
        if (gameObject.GetComponentInChildren<CollectibleComponent>() != null)
        {
            CollectibleComponent collectible = gameObject.GetComponentInChildren<CollectibleComponent>();

            if (dropPressed && !collectible.isCollectible)
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

            if(pickupPressed && collectible.isCollectible)
            {
                collectible.Interact(gameObject);
            }
            

        }
    }
    

}
