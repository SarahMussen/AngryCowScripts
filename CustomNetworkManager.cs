using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using Mirror;

/*
	Documentation: https://mirror-networking.com/docs/Components/NetworkManager.html
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

public class CustomNetworkManager : NetworkManager
{

    #region Variables
    /// <value> Boolean to check if the client connecting is also a server (true) or not (false) </value>
    private bool isServer;

    private bool isSuperPoop = false;
    private int spawnedSuperPoops = 0;
    private Vector3 shootDirection;
    private GameObject pointer;
    private Button fireButton;
    private Button superFireButton;
    private List<GameObject> superPoops;

    MoveInDirection moveInDirectionController;
    MoveInRandomDirection moveInRandomDirectionController;

    [SerializeField]
    ObjectPool objectPool;

    #endregion

    #region Server System Callbacks

    /// <summary>
    /// Called on the server when a new client connects.
    /// <para>Unity calls this on the Server when a Client connects to the Server. Use an override to tell the NetworkManager what to do when a client connects to the server.</para>
    /// <para>This method is also called when the host connects to the game. We make sure when that happens the value of isServer is set to true.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerConnect(NetworkConnection conn)
    {
        isServer = true;
    }

    #endregion

    #region Client System Callbacks

    /// <summary>
    /// Called on the client when connected to a server.
    /// <para>The default implementation of this function sets the client as ready and adds a player. In this method we first determine if the connection is from the host or the client by checking if the connection is also a server.</para>
    /// <para>We do this because the host is a client and the server at the same time. If we don't check the server part of the host, the host get's the same PlayerInfoMessage as a normal client.</para>
    /// <para>For each outcome (host or client) we send a different PlayerInfoMessage. This is done by setting the isServer variable to false when we have send the PlayerInfoMessage from the host.</para>
    /// </summary>
    /// <param name="conn">Connection to the server.</param>
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        PlayerInfoMessage message;

        if(isServer)
        {
            message = new PlayerInfoMessage
            {
                playerClass = PlayerClass.Cow,
                description = "hostCow",
                isHost = true
            };
        }
        else
        {
            message = new PlayerInfoMessage
            {
                playerClass = PlayerClass.Farmer,
                description = "clientFarmer",
                isHost = false
            };
        }

        conn.Send(message);  
        /// Set isServer to false when the hostmessage is send, we don't need it anymore
        isServer = false;
    }
    #endregion

    #region Start & Stop Callbacks

    /// <summary>
    /// This is invoked when the client is started.
    /// <para>This method registers a handler for a PlayerInfoMessage NetworkMessage when the client starts. The action leads to the method OnCreatePlayer.</para>
    /// </summary>
    public override void OnStartClient()
    {
        base.OnStartClient();

        NetworkServer.RegisterHandler<PlayerInfoMessage>(OnCreatePlayer);
    }

    #endregion

    #region Custom methods

    /// <summary>
    /// The OnCreatePlayer method
    /// <para>This method is called when a client starts.</para>
    /// <para>It requires a variable from the type GameObject to make the player prefab. We check if the message is coming from the host or from the client.
    /// For each different type of player we instantiate a different playerprefab from the spawnPrefabs list. This spawnPrefabs list refers to the "Registered Spawnable Prefabs"
    /// option in the NetworkManager.</para>
    /// </summary>
    /// <param name="conn">The connection to the server.</param>
    /// <param name="message">The PlayerInfoMessage network message.</param>
    void OnCreatePlayer(NetworkConnection conn, PlayerInfoMessage message)
    {
        GameObject gameobject;

        if(!message.isHost)
        {
            gameobject = Instantiate(spawnPrefabs[1]);
        }
        else
        {
            gameobject = Instantiate(spawnPrefabs[0]);
        }

        NetworkServer.AddPlayerForConnection(conn, gameobject);
    }

    public void setUIObjects(GameObject cowPointer, Button cowFireButton, Button cowSuperFireButton)
    {
        pointer = cowPointer;
        fireButton = cowFireButton;
        superFireButton = cowSuperFireButton;
    }

    public void setShootDirection(Vector3 shootDirection)
    {
        this.shootDirection = shootDirection;
    }

    public void setSuperPoop(bool isSuperPoop)
    {
        this.isSuperPoop = isSuperPoop;
    }

    public void Shoot()
    {
        DisableUIObjects();

        if(!isSuperPoop)
        {
            Vector3 spawnPos = new Vector3(2, 4, 60);
            GameObject poop = Instantiate(spawnPrefabs[2]);
            poop.transform.localPosition = spawnPos;

            moveInDirectionController = poop.GetComponent<MoveInDirection>();
            moveInDirectionController.SetShootDirection(shootDirection);

            NetworkServer.Spawn(poop);
        }
        else
        {
            InvokeRepeating("SpawnSuperPoop", 1, 1f);
        }

        Invoke("EnableUIObjects", 7);
    }

    void SpawnSuperPoop()
    {
        Vector3 spawnPos = new Vector3(2, 4, 60);
        GameObject superPoop = objectPool.GetObject();

        superPoop.transform.localPosition = spawnPos;

        if(spawnedSuperPoops != 3)
        {
            NetworkServer.Spawn(superPoop);
            spawnedSuperPoops += 1;
        }
        else 
        {
            spawnedSuperPoops = 0;
            GameManager.Instance.resetSuperPoopSliderValue();
            CancelInvoke("SpawnSuperPoop");
        }
    }

    void DisableUIObjects()
    {
        if(isSuperPoop)
        {
            superFireButton.interactable = false;
        }
        pointer.SetActive(false);
        fireButton.interactable = false;
    }

    void EnableUIObjects()
    {
        if(isSuperPoop)
        {
            superFireButton.interactable = true;
            setSuperPoop(false);
            GameManager.Instance.enableSuperFireButton(false);
        }
        pointer.SetActive(true);
        fireButton.interactable = true;
    }

    #endregion
}
