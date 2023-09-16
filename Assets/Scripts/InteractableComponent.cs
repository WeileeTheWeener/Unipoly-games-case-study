using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableComponent : MonoBehaviour,IInteractible
{
    [Networked] public bool isCollectible { get; set; }
    [Networked] Transform parentBeforeCollected { get; set; }
    [Networked] Transform parentAfterCollected { get; set; }
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        parentBeforeCollected = transform.parent;
        isCollectible = true;
    }
    public virtual void Interact(GameObject triggeredBy)
    {
        //is a collectible
       if(isCollectible)
       {
            parentAfterCollected = triggeredBy.transform;
            transform.parent = parentAfterCollected;
            isCollectible = false;
            rb.isKinematic = true;
            transform.localPosition = new Vector3(0f, 0f,1f);                    
       }

    }
    public void DropCollectible()
    {
        transform.parent = parentBeforeCollected;
        isCollectible = true;
        rb.isKinematic = false;
    }


   
}
