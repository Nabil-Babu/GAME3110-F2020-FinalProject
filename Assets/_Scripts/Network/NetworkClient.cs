/*
Data from client to server:  (ReadBytes) -> bytes -> (GetString) -> JSON string -> (FromJson) -> c# class
Data from server to client:   c# class -> (ToJson) -> JSON string -> (GetBytes) -> bytes -> (WriteBytes)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Unity.Collections;
using Unity.Networking.Transport;
using System;
using System.Text;
using System.Net.Sockets;
using System.Net;

public class NetworkClient : MonoBehaviour
{
    #region UDPClientForMatchMakingServer
    //Like socket in python
    public UdpClient udp;
    public string matchMakingServerIP = "localhost";
    public ushort matchMakingServerPort = 12345;
    #endregion

    public NetworkDriver m_Driver; //simliar to socket
    public NetworkConnection m_Connection;
    public string serverIP;
    public ushort serverPort;

    bool HasConnectedToMatchMakingServer = false;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (HasConnectedToMatchMakingServer == true)
        //{
        //    //You start the same way as you did in the server by calling m_Driver.ScheduleUpdate().Complete(); and make sure that the connection worked.
        //    m_Driver.ScheduleUpdate().Complete();




        //    //Check if connection is alive
        //    if (!m_Connection.IsCreated)
        //    {
        //        return;
        //    }





        //    DataStreamReader stream; //Where to store data
        //    NetworkEvent.Type cmd; //To check what data this is

        //    cmd = m_Connection.PopEvent(m_Driver, out stream); //Get data from socket
        //    while (cmd != NetworkEvent.Type.Empty)
        //    {
        //        //This event tells you that you have received a ConnectionAccept message and you are now connected to the remote peer.
        //        if (cmd == NetworkEvent.Type.Connect)
        //        {
        //            OnConnect();
        //        }
        //        //If you get data from server...
        //        else if (cmd == NetworkEvent.Type.Data)
        //        {
        //            //OnData(stream);
        //        }
        //        //Lastly we just want to make sure we handle the case that a server disconnects us for some reason.
        //        else if (cmd == NetworkEvent.Type.Disconnect)
        //        {
        //            //OnDisconnect();
        //        }

        //        cmd = m_Connection.PopEvent(m_Driver, out stream);
        //    }
        //}
    }

    private void OnDestroy()
    {
        //m_Driver.Dispose();
        udp.Close();
    }

    //Start to connect to match making server
    public void StartConnetToMatchMakingServer()
    {
        //m_Driver = NetworkDriver.Create(); //creat socket

        //m_Connection = default(NetworkConnection);

        ////serverIP = "3.15.221.96";
        ////serverIP = "127.0.0.1";
        //var endpoint = NetworkEndPoint.Parse(serverIP, serverPort);
        //m_Connection = m_Driver.Connect(endpoint); //connect to server

        //HasConnectedToMatchMakingServer = true;

        //Create socket
        udp = new UdpClient();

        //Set socket to connet server
        //udp.Connect("ec2-3-15-221-96.us-east-2.compute.amazonaws.com", 12345);
        udp.Connect(matchMakingServerIP, matchMakingServerPort);



        //Send data
        //We can only send Byte type so we need to convert data to Bytes
        Byte[] sendBytes = Encoding.ASCII.GetBytes("connect");
        udp.Send(sendBytes, sendBytes.Length);


        //Make OnReceived Function to handle all receving data, pass argument for OnReceived function
        udp.BeginReceive(new AsyncCallback(OnReceived), udp);
    }

    void OnReceived(IAsyncResult result)
    {
        // this is what had been passed into BeginReceive as the second parameter from BeginReceive function:
        UdpClient socket = result.AsyncState as UdpClient;

        // points towards whoever had sent the message:
        //This will be information of who sent the message
        IPEndPoint source = new IPEndPoint(0, 0);

        // get the actual message and fill out the source:
        //using socket, get last data,
        //when EndReceive is called, we stop handle next message until handling current message and continue when BeginReceive called
        byte[] message = socket.EndReceive(result, ref source);


        // do what you'd like with `message` here:
        //convert byte[] data(jason) to string
        string returnData = Encoding.ASCII.GetString(message);
        Debug.Log("Got this: " + returnData);

        //convert string to Message class
        //latestMessage = JsonUtility.FromJson<Message>(returnData);
        //try
        //{
        //    //What kind of message is this?
        //    string msg = "";
        //    switch (latestMessage.cmd)
        //    {
        //        //New client connected
        //        case commands.NEW_CLIENT:
        //            Debug.Log("NewClient");
        //            latestPlayerInfo = JsonUtility.FromJson<Player>(returnData);

        //            msg = "New client connected!\n";
        //            msg += "ID: " + latestPlayerInfo.id;
        //            Debug.Log(msg);

        //            //Spawn new userAvatar
        //            ShouldSpawnAvatar = true;
        //            //Instantiate(userAvatar, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        //            break;
        //        //Update game with new info
        //        case commands.UPDATE:
        //            lastestGameState = JsonUtility.FromJson<GameState>(returnData);
        //            break;
        //        //Get all clients info
        //        case commands.All_CLIENT_INFO:
        //            //lastestAllClietnsInfo = JsonUtility.FromJson<AllClientsInfo>(returnData);
        //            //msg = "All clients list\n";

        //            //msg += "Number of clients: " + lastestAllClietnsInfo.numOfClients.ToString() + "\n";

        //            //for (int i = 0; i < lastestAllClietnsInfo.numOfClients; ++i)
        //            //{
        //            //    msg += "Client" + i + "\n";
        //            //    msg += "IP: " + lastestAllClietnsInfo.allClients[i].IP + "\n";
        //            //    msg += "PORT: " + lastestAllClietnsInfo.allClients[i].PORT + "\n\n";
        //            //}

        //            //Debug.Log(msg);

        //            //Save all client info to create cube
        //            latestAllClientInfo = JsonUtility.FromJson<GameState>(returnData);

        //            msg = "All clients list\n";

        //            for (int i = 0; i < latestAllClientInfo.players.Length; ++i)
        //            {
        //                msg += "Client" + (i + 1) + "\n";
        //                msg += "ID: " + latestAllClientInfo.players[i].id + "\n\n";
        //            }

        //            Debug.Log(msg);

        //            ShouldSpawnOtherClients = true;

        //            break;
        //        case commands.DISCONNECTED_CLIENT:
        //            DisconnectedClientID disconnectedClientID = JsonUtility.FromJson<DisconnectedClientID>(returnData);

        //            //msg = "disconnected client\n";
        //            //msg += "IP: " + disconnectedClient.IP + "\n";
        //            //msg += "PORT: " + disconnectedClient.PORT;
        //            //Debug.Log(msg);

        //            Debug.Log("Disconnected Client: " + disconnectedClientID.id);

        //            latestDisconnectedClientID = disconnectedClientID.id;
        //            ShouldDeleteClient = true;



        //            break;
        //        case commands.SENDER_IP_PORT:
        //            ClientInfo clientIPPORT = JsonUtility.FromJson<ClientInfo>(returnData);
        //            myIP = clientIPPORT.IP;
        //            myPORT = clientIPPORT.PORT;
        //            ShouldSetIP = true;
        //            break;
        //        default:
        //            Debug.Log("Error");
        //            break;
        //    }
        //}
        //catch (Exception e)
        //{
        //    Debug.Log(e.ToString());
        //}

        // schedule the next receive operation once reading is done:
        //continue get next message
        socket.BeginReceive(new AsyncCallback(OnReceived), socket);
    }

    void OnConnect()
    {
        //Debug.Log("We are now connected to the server");

        //Debug.Log("My IP: " + m_Driver.);
        //Debug.Log("My PORT: " + m_Driver.LocalEndPoint().Port);

        //InvokeRepeating("SendPlayerInfo", 0.1f, 0.03f); //Start sending player's position to server

        //// Example to send a handshake message:
        // HandshakeMsg m = new HandshakeMsg();
        // m.player.id = m_Connection.InternalId.ToString();
        // SendToServer(JsonUtility.ToJson(m));
    }
}
