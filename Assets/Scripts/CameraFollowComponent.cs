using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(Camera))]
public class CameraFollowComponent : MonoBehaviour
{
    [SerializeField] GameObject objectToFollow;
    [SerializeField] Vector3 offset;
    [SerializeField] float followSpeed;
    [SerializeField] float rotationSpeed;
    private Vector3 velocity;
    private Transform cameraPivot;

    private void Start()
    {
        cameraPivot = transform.parent;
    }
    void FixedUpdate()
    {
        FollowObject();
        
    }
    private void LateUpdate()
    {
        RotateAroundObject();
    }
    void FollowObject()
    {
        Vector3 positionToLerp = new Vector3(objectToFollow.transform.position.x, objectToFollow.transform.position.y, 
            objectToFollow.transform.position.z) + offset;
        cameraPivot.position = Vector3.SmoothDamp(cameraPivot.position, positionToLerp, ref velocity, followSpeed);
    }
    void RotateAroundObject()
    {
        //update camera pivot position
        cameraPivot.transform.position = objectToFollow.transform.position;
        cameraPivot.RotateAround(objectToFollow.transform.position, Vector3.up, Input.GetAxis("Mouse X") * rotationSpeed);
        
    }

}
