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
        public string level;
        public string hp;
        public string damage;
    };
}
