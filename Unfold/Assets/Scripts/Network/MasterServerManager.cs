﻿using UnityEngine;
using System.Collections;

public class MasterServerManager : MonoBehaviour{
    /// <summary>
    /// @author: Michael Gonzalez
    /// This class implements the Unity Master Server to allow users to easily
    /// connect to one another. It provides the framework for both hosts and 
    /// clients.
    /// </summary>
    public const string gameTitle = "UnfoldX";
    private HostData[] gameList;
    private string clientConnectErr = "Error while connecting to host: ";
    private int portNumber = 26500;
    private string[] lastConnectionAttempt;
    public const uint CANCONNECT = 0x8000000;

    public bool debugOn = false;
    public HostData[] GetHostData()
    {
        if (debugOn)
        {
            Debug.Log("Entering GetHostData()");
        }
        if( gameList == null )
        {
            MasterServer.ClearHostList();
        }
        MasterServer.RequestHostList(gameTitle);
        gameList = MasterServer.PollHostList();
        return gameList;
    }
    public void RegisterServer(string gameName, TextureController.TextureChoice gameType)
    {
        if (debugOn)
        {
            Debug.Log("Entering RegisterServer()");
        }
        uint levelType = (uint)gameType;
        levelType |= CANCONNECT;
        MasterServer.RegisterHost(gameTitle, gameName, levelType.ToString());
        if (debugOn)
        {
            Debug.Log("Trying to register game as type (" + gameTitle + ") under name (" + gameName + ")");
        }
    }
    public void ConnectToGame(int hostIndex, GameObject connectionInfo)
    {
        if(!IndexInRange(--hostIndex, gameList.Length, "ConnectToGame"))
        {
            Debug.Log("Game Index: " + hostIndex);
            Debug.Log("Game List: " + gameList);
            Debug.Log("Game List Length: " + gameList.Length);
            return;
        }
        lastConnectionAttempt = gameList[hostIndex].ip;
        GameObject cInfo = (GameObject)Instantiate(connectionInfo);
        ConnectionInfo cInfoScript = cInfo.GetComponent<ConnectionInfo>();
        cInfoScript.setInfo(lastConnectionAttempt, portNumber);
        //Network.Connect(lastConnectionAttempt, portNumber);
        if(debugOn)
        {
            Debug.Log("Host IP: " + gameList[hostIndex].ip);
        }
    }
    public void RetryConnection()
    {
        Network.Connect(lastConnectionAttempt, portNumber);
    }

    /**void MasterServer.OnFailedToConnectToMasterServer(NetworkConnectionError err)
    {
        Debug.Log("Error connecting to Unity Master Server: " + err);
    }**/
    public TextureController.TextureChoice DetermineGameType(int hostIndex)
    {
        TextureController.TextureChoice retVal;
        if(!IndexInRange(hostIndex, gameList.Length, "ConnectToGame"))
        {
            Debug.Log("Setting level type to the default: Corn");
            retVal = TextureController.TextureChoice.Corn;
        }
        else
        {
            int gameType;
            int.TryParse(gameList[hostIndex].comment, out gameType);
            retVal = (TextureController.TextureChoice)gameType;
        }
        return retVal;
    }
    private bool IndexInRange(int num, int range, string method)
    {
        bool retVal = num < range;
        if (!retVal)
            Debug.Log("Index out of bounds in " + method);
        return retVal;
    }

	
}