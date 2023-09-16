using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public Vector3 playerInput;   
    public float xAxisMouseInput;
    public NetworkBool isJumpPressed;
    public NetworkBool isCollectButtonPressed;
    public NetworkBool isReleaseButtonPressed;

}
