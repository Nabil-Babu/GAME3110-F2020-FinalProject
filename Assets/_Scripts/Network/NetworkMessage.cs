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
        WAITING_TIME
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
}
