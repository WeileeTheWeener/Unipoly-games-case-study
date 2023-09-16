using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerNetworkComponent : NetworkBehaviour,IPlayerLeft,IPlayerJoined
{
    public static PlayerNetworkComponent LocalPlayer { get; set; }
    private Rigidbody rb;
    private Vector3 movementVector;

    [Networked] public float movementSpeed { get; set; }
    [Networked] public float jumpForce { get; set; }
    [Networked] public float fallingForce { get; set; }
    [Networked] private bool isGrounded { get; set; }
    [Networked] public NetworkObject playersCameraPivot { get; set; }
    public NetworkObject cameraPivotPrefab;
    private bool isJumpedPressed;
    public override void Spawned()
    {
        rb = GetComponent<Rigidbody>();

        if (Object.HasInputAuthority)
        {          
            LocalPlayer = this;        
            Debug.Log("spawned local player");
        }
        else Debug.Log("spawned client player");

    }
    public void PlayerLeft(PlayerRef player)
    {
        if (player == LocalPlayer)
        {
            //Runner.Despawn(Object);
            Runner.Despawn(LocalPlayer.GetComponent<NetworkObject>());
        }
    }
    public override void FixedUpdateNetwork()
    {
       if(GetInput(out NetworkInputData data))
       {
            data.playerInput.Normalize();
            
            //handle player movement
            Vector3 cameraForward = playersCameraPivot.GetComponentInChildren<Camera>().transform.forward;
            Vector3 cameraRight = playersCameraPivot.GetComponentInChildren<Camera>().transform.right;
            cameraForward.y = 0;
            cameraRight.y = 0; 

            movementVector = (data.playerInput.z * cameraForward.normalized + data.playerInput.x * cameraRight.normalized).normalized;

            rb.AddForce(movementVector * movementSpeed);

            HandleJumping();

            //call 3rd person camera functions
            playersCameraPivot.GetComponentInChildren<CameraFollowComponent>().FollowObject();
            playersCameraPivot.GetComponentInChildren<CameraFollowComponent>().RotateAroundObject(data.xAxisMouseInput);

            //handle player rotation
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            targetRotation.eulerAngles = new Vector3(0, targetRotation.eulerAngles.y, 0);
            transform.rotation = targetRotation;

            isJumpedPressed = data.isJumpPressed;
       }

    }
    public void HandleJumping()
    {
        if (isJumpedPressed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
        if (rb.velocity.y <= -0.1f)
        {
            rb.AddForce(Vector3.down * fallingForce, ForceMode.Acceleration);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    public void PlayerJoined(PlayerRef player)
    {

        //handle camera depth
        if(Object.HasInputAuthority)
        {
            //playersCameraPivot.GetComponentInChildren<CameraFollowComponent>().cameraDepth = player.PlayerId + 1;
            playersCameraPivot.GetComponentInChildren<CameraFollowComponent>().cameraDepth = 4;
        }
        else
        {
            playersCameraPivot.GetComponentInChildren<CameraFollowComponent>().cameraDepth = player.PlayerId;
            //LocalPlayer.playersCameraPivot.GetComponentInChildren<CameraFollowComponent>().cameraDepth = 0;
            //playersCameraPivot.GetComponentInChildren<CameraFollowComponent>().cameraDepth = player.PlayerId;
            //playersCameraPivot.GetComponentInChildren<CameraFollowComponent>().cameraDepth = 5+player.PlayerId;
            //playersCameraPivot.GetComponentInChildren<CameraFollowComponent>().cameraDepth = 5+player.PlayerId;
            //Debug.Log("set camera pivot to "+player.PlayerId);
        }
    }
}
