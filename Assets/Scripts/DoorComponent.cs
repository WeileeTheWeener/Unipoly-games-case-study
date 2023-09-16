using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorComponent : InteractableComponent
{
    [Networked] public bool isOpen { get; set; }
    private float openedRotation;
    private float closedRotation;
    private void Start()
    {
        closedRotation = 0f;
        openedRotation = -90f;
    }

    public override void Interact(GameObject triggeredBy)
    {
        if(isOpen)
        {
            transform.rotation = Quaternion.Euler(0f, closedRotation, 0f);
            isOpen = false;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, openedRotation, 0f);
            isOpen = true;
        }
    }
 
}
