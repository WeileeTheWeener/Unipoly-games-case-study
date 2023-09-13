using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleComponent : MonoBehaviour,IInteractible
{
    public bool isCollectible;
    Transform parentBeforeCollected;
    Transform parentAfterCollected;
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        parentBeforeCollected = transform.parent;
    }
    public void Interact(GameObject triggeredBy)
    {
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
