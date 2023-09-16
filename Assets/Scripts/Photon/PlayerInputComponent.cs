using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerInputComponent : MonoBehaviour
{
    [Networked] Vector3 input { get; set; }
    [Networked] float horizontalMouseInput { get; set; }
    [Networked] bool jumpPressed { get; set; }

    [Networked] bool collectPressed { get; set; }
    [Networked] bool releasePressed { get; set; }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        input = new Vector3(horizontal, 0, vertical);
        horizontalMouseInput = Input.GetAxis("Mouse X");

        if(Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
        }
        if(Input.GetButtonDown("Collect"))
        {
            collectPressed = true;
        }
        if (Input.GetButtonDown("Release"))
        {
            releasePressed = true;
        }


    }
    public NetworkInputData ReturnInputData()
    {
        NetworkInputData networkInputData = new NetworkInputData();
        networkInputData.playerInput = input;
        networkInputData.xAxisMouseInput = horizontalMouseInput;
        networkInputData.isJumpPressed = jumpPressed;
        networkInputData.isCollectButtonPressed = collectPressed;
        networkInputData.isReleaseButtonPressed = releasePressed;
        jumpPressed = false;
        collectPressed = false;
        releasePressed = false;
        return networkInputData;
    }
}
