using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(Camera))]
public class CameraFollowComponent : NetworkBehaviour
{
    //public GameObject objectToFollow;
    [Networked] public NetworkObject objectToFollow { get; set; }
    [Networked] [SerializeField] Vector3 offset { get; set; }
    [Networked] [SerializeField] float followSpeed { get; set; }
    [Networked] [SerializeField] float rotationSpeed { get; set; }
    [Networked] public int cameraDepth { get; set; }

    private Vector3 velocity;
    private Transform cameraPivot;

    private void Start()
    {
        GetComponent<Camera>().depth = cameraDepth;
    }
    public void FollowObject()
    {     
        cameraPivot = transform.parent;
        Vector3 positionToLerp = new Vector3(objectToFollow.transform.position.x, objectToFollow.transform.position.y,
            objectToFollow.transform.position.z) + offset;
        cameraPivot.transform.position = Vector3.SmoothDamp(cameraPivot.position, positionToLerp, ref velocity, followSpeed);
    }
    public void RotateAroundObject(float mouseInput)
    {
        //update camera pivot position
        cameraPivot.transform.position = objectToFollow.transform.position;
        //rotate around the object
        cameraPivot.RotateAround(objectToFollow.transform.position, Vector3.up, mouseInput * rotationSpeed);


    }

}
