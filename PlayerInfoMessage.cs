using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


/// <summary>
/// The PlayerClass enum.
/// Enum to define the type of the player.
/// </summary>
public enum PlayerClass
{
    Cow,
    Farmer
}

/// <summary>
/// The PlayerInfoMessage class.
/// This class defines the message a player sends to the server.
/// </summary>
public struct PlayerInfoMessage : NetworkMessage
{
    /// <value> An instance of the PlayerClass enumeration </value>
    public PlayerClass playerClass;
    /// <value> A description of the player </value>
    public string description;
    /// <value> Boolean to see if the message is send by a host or a client </value>
    public bool isHost;
}