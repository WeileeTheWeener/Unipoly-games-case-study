using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using Fusion.Photon;
using System;
using UnityEngine.SceneManagement;
using System.Linq;

public class PhotonFusionConnection : NetworkBehaviour, INetworkRunnerCallbacks
{
    NetworkRunner networkRunner;
    [SerializeField] private NetworkPrefabRef playerPrefab;
    [SerializeField] private NetworkPrefabRef cameraPivotPrefab;
    [SerializeField] private NetworkPrefabRef collectibleCubePrefab;
    [SerializeField] private NetworkPrefabRef housePrefab;
    private Dictionary<PlayerRef, NetworkObject> spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    public Dictionary<PlayerRef, NetworkObject> spawnedCameras = new Dictionary<PlayerRef, NetworkObject>();


    private void OnGUI()
    {
        if (networkRunner == null)
        {
            GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
            myButtonStyle.fontSize = 24;
            GUIStyle textStyle = new GUIStyle();
            textStyle.fontSize = 36;

            GUI.Label(new Rect(400, 0, 100, 20), "Welcome!", textStyle);

            if (GUI.Button(new Rect(0, 0, 400, 40), "Host",myButtonStyle))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0, 40, 400, 40), "Join",myButtonStyle))
            {
                StartGame(GameMode.Client);
            }
        }
    }
    public async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        networkRunner = gameObject.AddComponent<NetworkRunner>();
        networkRunner.ProvideInput = true;

        // Start or join (depends on gamemode) a session with a specific name
        await networkRunner.StartGame(new StartGameArgs()
        {
            //max player count is 4
            GameMode = mode,
            SessionName = "TestRoom",
            PlayerCount =4,
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        //spawn a collectible object
        if(Runner.IsServer)
        {
            NetworkObject networkCollectibleObject = Runner.Spawn(collectibleCubePrefab, Vector3.zero, Quaternion.identity);
            NetworkObject houseObject = Runner.Spawn(housePrefab, new Vector3(-2f,0f,7f), Quaternion.identity);
        }
    }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("onplayerjoined");

        if (runner.IsServer)
        {
            //spawn players
            Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.DefaultPlayers) * 3, 1, 0);
            NetworkObject networkPlayerObject = runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);
            networkPlayerObject.name = "player" + player.PlayerId;
            spawnedCharacters.Add(player, networkPlayerObject);

            //spawn cameras
            NetworkObject networkCameraObject = runner.Spawn(cameraPivotPrefab, Vector3.zero, Quaternion.identity, player);
            networkCameraObject.name = "player_"+player.PlayerId + "_CameraPivot";
            spawnedCameras.Add(player, networkCameraObject);

            
            //set camera properties
            networkCameraObject.GetComponentInChildren<CameraFollowComponent>().objectToFollow = networkPlayerObject;
            networkPlayerObject.GetComponent<PlayerNetworkComponent>().playersCameraPivot = networkCameraObject;

        }



    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("onplayerleft");
        // Find and remove the players avatar
        if (spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            spawnedCharacters.Remove(player);
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {

        if(PlayerNetworkComponent.LocalPlayer != null)
        {
            PlayerInputComponent playerInputComponent = PlayerNetworkComponent.LocalPlayer.GetComponent<PlayerInputComponent>();
            input.Set(playerInputComponent.ReturnInputData());
        }

    }
    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("onconnectedtoserver");
    }
    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        Debug.Log("ondisconnectedfromserver");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }


    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
       
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }
}
