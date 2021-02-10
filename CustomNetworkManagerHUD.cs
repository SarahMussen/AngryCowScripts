using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;

/// <summary>
/// The CustomNetworkManagerHUD class
/// <para> Contains all the methods to set up the host and the client for the game </para>
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(CustomNetworkManager))]
public class CustomNetworkManagerHUD : MonoBehaviour
{
    #region Variables

    /// <value> An instance of the NetworkManager </value>
    CustomNetworkManager manager;

    #endregion

    #region Custom methods

    /// <summary>
    ///  The awake method
    ///  <para> This class is called when the application awakens and assigns the <value> manager </value> to a component of the type NetworkManager. </para>
    /// </summary>
    public void Awake()
    {
        manager = GetComponent<CustomNetworkManager>();
    }

    /// <summary>
    /// The cowClicked method
    /// <para> This method is called when btnCow is clicked in the GUI and sets the cow as the host of the game. </para>
    /// </summary>
    public void cowClicked()
    {
        if(!NetworkClient.active)
        {
            if(Application.platform != RuntimePlatform.WebGLPlayer)
            {
                manager.StartHost();
                Debug.Log("Cow is host");
            }
        }
    }

    /// <summary>
    /// The farmerClicked method
    /// <para> This method is callen when btnFarmer is clicked in the GUI and sets the farmer as the client in the game. </para>
    /// </summary>
    public void farmerClicked()
    {
        if(!NetworkClient.active)
        {
            manager.StartClient();
            Debug.Log("Farmer is client");
        }
    }

    #endregion
}