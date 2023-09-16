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
        if (gameObject.GetComponentInChildren<InteractableComponent>() != null)
        {
            InteractableComponent collectible = gameObject.GetComponentInChildren<InteractableComponent>();

            if (dropPressed && !collectible.isCollectible)
            {
                collectible.DropCollectible();
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        //can pickup collectible if player is inside its collider
        if(other.gameObject.GetComponent<InteractableComponent>() != null)
        {
            InteractableComponent collectible = other.gameObject.GetComponent<InteractableComponent>();

            if(pickupPressed && collectible.isCollectible)
            {
                collectible.Interact(gameObject);
            }
            
        }
        if (other.gameObject.GetComponent<DoorComponent>() != null)
        {
            DoorComponent door = other.gameObject.GetComponent<DoorComponent>();

            if (pickupPressed)
            {
                door.Interact(gameObject);
            }

        }
    }
    

}
