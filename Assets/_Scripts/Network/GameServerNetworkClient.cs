﻿//Connect to GameServer(Unity)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Networking.Transport;
using NetworkMessages;
using System.Text;
using UnityEngine.UI;

public class GameServerNetworkClient : MonoBehaviour
{
    public NetworkDriver m_Driver; //simliar to socket
    public NetworkConnection m_Connection;
    public string serverIP;
    public ushort gameServerPort = 12346;

    [SerializeField]
    Transform player = null; //reference to client-player
    string playerInternalID; //internal id from server

    [SerializeField]
    GameObject clientAvatar = null; //Player Prefab to spawn in
    private Dictionary<string, GameObject> listOfClients = new Dictionary<string, GameObject>(); //Dictionary for all clients

    PlayerUpdateMsg playerInfo = new PlayerUpdateMsg(); 

    // Start is called before the first frame update
    void Start()
    {
        m_Driver = NetworkDriver.Create(); //creat socket

        m_Connection = default(NetworkConnection);

        //serverIP = "3.15.221.96";
        
        //serverIP = "174.95.96.92"; // Was testing on my Laptop did not work

        //serverIP = "127.0.0.1";
        var endpoint = NetworkEndPoint.Parse(serverIP, gameServerPort);
        m_Connection = m_Driver.Connect(endpoint); //connect to server

        //playerInternalIdText = player.gameObject.GetComponentInChildren<TextMesh>();
    }

    private void OnDestroy()
    {
        m_Driver.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        //You start the same way as you did in the server by calling m_Driver.ScheduleUpdate().Complete(); and make sure that the connection worked.
        m_Driver.ScheduleUpdate().Complete();

        //Check if connection is alive
        if (!m_Connection.IsCreated)
        {
            return;
        }

        DataStreamReader stream; //Where to store data
        NetworkEvent.Type cmd; //To check what data this is

        cmd = m_Connection.PopEvent(m_Driver, out stream); //Get data from socket
        while (cmd != NetworkEvent.Type.Empty)
        {
            //This event tells you that you have received a ConnectionAccept message and you are now connected to the remote peer.
            if (cmd == NetworkEvent.Type.Connect)
            {
                OnConnect();
            }
            //If you get data from server...
            else if (cmd == NetworkEvent.Type.Data)
            {
                OnData(stream);
            }
            //Lastly we just want to make sure we handle the case that a server disconnects us for some reason.
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                OnDisconnect();
            }

            cmd = m_Connection.PopEvent(m_Driver, out stream);
        }
    }

    void OnConnect()
    {
        Debug.Log("Connected to the game server");

        //Debug.Log("My IP: " + m_Driver.);
        //Debug.Log("My PORT: " + m_Driver.LocalEndPoint().Port);

        InvokeRepeating("SendPlayerInfo", 0.1f, 0.03f); //Start sending player's position to server

        //// Example to send a handshake message:
        // HandshakeMsg m = new HandshakeMsg();
        // m.player.id = m_Connection.InternalId.ToString();
        // SendToServer(JsonUtility.ToJson(m));
    }


    void OnData(DataStreamReader stream)
    {
        NativeArray<byte> bytes = new NativeArray<byte>(stream.Length, Allocator.Temp);
        stream.ReadBytes(bytes); //Get bytes
        string recMsg = Encoding.ASCII.GetString(bytes.ToArray()); //convert bytes to JSON string
        NetworkHeader header = JsonUtility.FromJson<NetworkHeader>(recMsg); //convert JSON to c# class

        switch (header.cmd)
        {
            case Commands.HANDSHAKE:
               HandshakeMsg hsMsg = JsonUtility.FromJson<HandshakeMsg>(recMsg);
               Debug.Log("Handshake message received!");
               SpawnClientOwnedPlayer(hsMsg);
               break;
            case Commands.PLAYER_HIT:
               PlayerHitMsg playerHitMsg = JsonUtility.FromJson<PlayerHitMsg>(recMsg);
               DealDamageToClient(playerHitMsg.playerInternalID);
               Debug.Log("Player Hit message received: "+playerHitMsg.playerInternalID);
               break;
            case Commands.PROJECTILE_FIRE:
               ProjectileFireMsg projectileFireMsg = JsonUtility.FromJson<ProjectileFireMsg>(recMsg);
               FireProjectileFromClient(projectileFireMsg.projectileOwnerID);
               Debug.Log("Player Fire message received from: "+projectileFireMsg.projectileOwnerID);
               break;

            case Commands.PLAYER_INTERNALID:
                PlayerInternalIDMsg internalIDMsg = JsonUtility.FromJson<PlayerInternalIDMsg>(recMsg);
                playerInternalID = internalIDMsg.playerInternalID;
                Debug.Log("Got internalId from server : " + playerInternalID);
                break;

            case Commands.PLAYER_UPDATE:
               PlayerUpdateMsg puMsg = JsonUtility.FromJson<PlayerUpdateMsg>(recMsg);
               Debug.Log("Player update message received!");
               Debug.Log("Got data from server, player ID: " + puMsg.player.internalID);
               break;

            case Commands.SERVER_UPDATE:
               ServerUpdateMsg suMsg = JsonUtility.FromJson<ServerUpdateMsg>(recMsg);
               Debug.Log("Server update message received!");
               UpdateClientsInfo(suMsg);
               break;

            //To spawn existed players
            case Commands.SPAWN_EXISTED_PLAYERS:
                ServerUpdateMsg existedPlayerInfo = JsonUtility.FromJson<ServerUpdateMsg>(recMsg);
                Debug.Log("existed player info received!");
                SpawnExistedPlayer(existedPlayerInfo);
                break;

            //Spawn new player
            case Commands.SPAWN_NEW_PLAYER:
                PlayerUpdateMsg newPlayerInfo = JsonUtility.FromJson<PlayerUpdateMsg>(recMsg);
                Debug.Log("new client info received!");
                SpawnNewPlayer(newPlayerInfo);
                break;

            //handle disconnected player
            case Commands.DISCONNECTED_PLAYER:
               DisconnectedPlayersMsg dpm = JsonUtility.FromJson<DisconnectedPlayersMsg>(recMsg);
               Debug.Log("Disconnected player info recieved!");
               DeleteDisconnectPlayer(dpm);
               break;

            default:
                Debug.Log("Unrecognized message received!");
                break;
        }
    }

    void OnDisconnect()
    {
        Debug.Log("Client got disconnected from server");
        m_Connection = default(NetworkConnection);
    }

    void SendToServer(string message)
    {
       //When you establish a connection between the client and the server, 
       //you send a data. The use of the BeginSend / EndSend pattern together with the DataStreamWriter,
       //write data into the stream, and finally send it out on the network.

       var writer = m_Driver.BeginSend(m_Connection);
       NativeArray<byte> bytes = new NativeArray<byte>(Encoding.ASCII.GetBytes(message), Allocator.Temp);
       writer.WriteBytes(bytes);
       m_Driver.EndSend(writer);
    }

    void Disconnect()
    {
        Debug.Log("Disconnect from server");
        m_Connection.Disconnect(m_Driver);
        m_Connection = default(NetworkConnection);
    }


    void SendPlayerInfo()
    {
       //player.position

       //// Example to send a handshake message:
       //HandshakeMsg m = new HandshakeMsg();
       //m.player.id = m_Connection.InternalId.ToString();
       //SendToServer(JsonUtility.ToJson(m));
       Debug.Log("Sending Player Info to Server");
       playerInfo.player.internalID = playerInternalID;
       playerInfo.player.pos = player.position;
       SendToServer(JsonUtility.ToJson(playerInfo));

    }

    public void SendHitScanMsg(Vector3 origin, Vector3 direction)
    {
        Debug.Log("Sending HitScan Message to Server"); 
        HitScanMsg hitScanMsg = new HitScanMsg();
        hitScanMsg.origin = origin;
        hitScanMsg.direction = direction;
        SendToServer(JsonUtility.ToJson(hitScanMsg)); 
    }

    public void SendProjectileFireMsg(string internalID)
    {
        Debug.Log("Sending Projectile Fire Message to Server"); 
        ProjectileFireMsg projectileFireMsg = new ProjectileFireMsg();
        projectileFireMsg.projectileOwnerID = internalID; 
        SendToServer(JsonUtility.ToJson(projectileFireMsg)); 
    }

    public void SendProjectileHitMsg(Vector3 origin, Vector3 direction, string ownerID)
    {
        Debug.Log("Sending Projectile Hit Message to Server");
        ProjectileHitMsg projectileHit = new ProjectileHitMsg();
        projectileHit.hitLocation = origin;
        projectileHit.direction = direction;
        projectileHit.projectileOwnerID = ownerID;
        SendToServer(JsonUtility.ToJson(projectileHit));
    }

    //Spawn existed player in server
    void SpawnExistedPlayer(ServerUpdateMsg data)
    {
        for (int i = 0; i < data.players.Count; ++i)
        {
            GameObject avatar = Instantiate(clientAvatar);

            listOfClients[data.players[i].internalID] = avatar;
            avatar.transform.position = data.players[i].pos;
            avatar.GetComponentInChildren<PlayerCharacter>().IsPlayer2 = true;
            avatar.GetComponent<PlayerCharacter>().internalID = data.players[i].internalID; 
            avatar.GetComponentInChildren<Text>().text = "Player "+data.players[i].internalID;
            //avatar.GetComponentInChildren<TextMesh>().text = data.players[i].id;
        }
    }

    void SpawnNewPlayer(PlayerUpdateMsg data)
    {
        GameObject avatar = Instantiate(clientAvatar);

        listOfClients[data.player.internalID] = avatar;
        avatar.transform.position = data.player.pos;
        avatar.GetComponentInChildren<Text>().text = "Player "+data.player.internalID;
        avatar.GetComponentInChildren<PlayerCharacter>().IsPlayer2 = true;
        avatar.GetComponent<PlayerCharacter>().internalID = data.player.internalID;  
    }

    void SpawnClientOwnedPlayer(HandshakeMsg data)
    {
        GameObject avatar = Instantiate(clientAvatar);
        listOfClients[data.player.internalID] = avatar;
        avatar.transform.position = data.player.pos;
        avatar.GetComponent<PlayerSpawner>().spawnPoint = data.player.pos;
        avatar.GetComponentInChildren<Text>().text = "Player "+data.player.internalID;
        avatar.GetComponent<PlayerCharacter>().clientConnection = this;
        avatar.GetComponent<PlayerCharacter>().internalID = data.player.internalID; 
        player = avatar.transform; 
    }

    void DealDamageToClient(string clientID)
    {
        if(listOfClients.ContainsKey(clientID))
        {
            listOfClients[clientID].GetComponent<PlayerCharacter>().TakeHit(); 
        }
    }

    void FireProjectileFromClient(string clientID)
    {
        if(clientID != playerInternalID)
        {
            if(listOfClients.ContainsKey(clientID))
            {
                listOfClients[clientID].GetComponent<PlayerCharacter>().ShootProjectile();
            }
        }
    }

    // Update all client info with data from server
    void UpdateClientsInfo(ServerUpdateMsg data)
    {
       for (int i = 0; i < data.players.Count; ++i)
       {
           if (listOfClients.ContainsKey(data.players[i].internalID))
           {
               if(data.players[i].internalID != playerInternalID)
               {
                   listOfClients[data.players[i].internalID].transform.position = data.players[i].pos;
               }
               //listOfClients[data.players[i].id].GetComponent<Renderer>().material.color = data.players[i].color;
           }
        //    //My info, my information is not in listOfClients
        //    else if (playerInfo.player.id == data.players[i].id)
        //    {
        //        player.gameObject.GetComponent<Renderer>().material.color = data.players[i].color;
        //        playerInfo.player.color = data.players[i].color;
        //    }
       }
    }

    void DeleteDisconnectPlayer(DisconnectedPlayersMsg data)
    {
       for (int i = 0; i < data.disconnectedPlayers.Count; ++i)
       {
           if (listOfClients.ContainsKey(data.disconnectedPlayers[i]))
           {
               Destroy(listOfClients[data.disconnectedPlayers[i]]);
               listOfClients.Remove(data.disconnectedPlayers[i]);
           }
       }
    }
}
