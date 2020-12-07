using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkMessages
{
    [System.Serializable]
    public class UserInfo
    {
        public string user_id;
        public string password;

        public string userName;
        public string skillLevel;
    };




    public enum Commands
    {
        CONNECT,
        CONNECT_SUCCESS,
        WAITING_TIME,
        MATCH_FOUND,
        PLAYER_INTERNALID,
        PLAYER_UPDATE,
        SERVER_UPDATE,
        SPAWN_EXISTED_PLAYERS
    }

    [System.Serializable]
    public class NetworkHeader
    {
        public Commands cmd;
    }

    [System.Serializable]
    public class ConnectMSG : NetworkHeader
    {
        public UserInfo userInfo;

        public ConnectMSG()
        {
            cmd = Commands.CONNECT;
        }
    };

    [System.Serializable]
    public class WaitingTimeMSG : NetworkHeader
    {
        public string waitingTime;

        public WaitingTimeMSG()
        {
            cmd = Commands.WAITING_TIME;
        }
    };

    [System.Serializable]
    public class MatchFoundMSG : NetworkHeader
    {
        public MatchFoundMSG()
        {
            cmd = Commands.MATCH_FOUND;
        }
    };

    [System.Serializable]
    public class PlayerInternalIDMsg : NetworkHeader
    {
        public string playerInternalID;
        public PlayerInternalIDMsg()
        {      // Constructor
            cmd = Commands.PLAYER_INTERNALID;
        }
    };

    [System.Serializable]
    public class PlayerUpdateMsg : NetworkHeader
    {
        public Vector3 pos = Vector3.zero;
        public PlayerUpdateMsg()
        {      // Constructor
            cmd = Commands.PLAYER_UPDATE;
        }
    };

    [System.Serializable]
    public class ServerUpdateMsg : NetworkHeader
    {
        public List<NetworkObjects.NetworkPlayer> players;
        public ServerUpdateMsg()
        {      // Constructor
            cmd = Commands.SERVER_UPDATE;
            players = new List<NetworkObjects.NetworkPlayer>();
        }
    }
}

namespace NetworkObjects
{
    //[System.Serializable]
    //public class NetworkObject
    //{
    //    public string id;
    //}

    [System.Serializable]
    public class NetworkPlayer// : NetworkObject
    {
        public string internalID;
        public Vector3 pos;

        public NetworkPlayer()
        {
            pos = Vector3.zero;
        }
    }
}
