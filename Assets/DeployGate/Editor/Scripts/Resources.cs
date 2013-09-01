using UnityEngine;
using System;

namespace DeployGate.Resources
{
    [System.Serializable]
    public class Message
    {
        public string title;
        public string text;
        public string date;
        public string version;
        public int versionCode;

        public override string ToString()
        {
            string json = "{";
            json += string.Format("\"date\": \"{0}\",", date);
            json += string.Format("\"text\": \"{0}\",", text);
            json += string.Format("\"title\": \"{0}\",", title);
            json += string.Format("\"version\": \"{0}\",", version);
            json += string.Format("\"versionCode\": {0}", versionCode);
            json += "}";
            return json;
        }
    }

    [System.Serializable]
    public class TempPath
    {
        public string directryPath
        {
            get
            {
                return string.Format("DeployGate{0}Temp", DeployGateUtility.SEPARATOR);
            }
        }
        public string messagePath = "";
    }

    [System.Serializable]
    public class UserInfo
    {
        public string username;
        public string token;
    }
}