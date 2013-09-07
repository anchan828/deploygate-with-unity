using System;

namespace DeployGate.Resources
{
    [Serializable]
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
    [Serializable]
    public class MembersInfo
    {
        public bool error;
        public Usage usage;
        public Member[] members;
    }

    [Serializable]
    public class Member
    {
        public string name;
        public int role;
    }

    public class Usage
    {
        public int used;
        public int max;
    }

    [Serializable]
    public class TempPath
    {
        public string directryPath
        {
            get
            {
                return string.Format("DeployGate{0}Temp", DeployGateUtility.Separator);
            }
        }
        public string messagePath = "";
    }

    [Serializable]
    public class UserInfo
    {
        public string username;
        public string token;
    }
}