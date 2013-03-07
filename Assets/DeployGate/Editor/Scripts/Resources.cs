using UnityEngine;
using System.Collections;

namespace DeployGate.Resources
{
	[System.Serializable]
	public class Message
	{
		public string title;
		public string text;
		public System.DateTime date;
		public string version;
		public int versionCode;
	}
	
	[System.Serializable]
	public class TempPath
	{
		public string directryPath = Application.dataPath.Replace ("Assets", "DeployGate/Temp");
		public string messagePath = "";
	}
	
	[System.Serializable]
	public class UserInfo
	{
		public string username;
		public string token;
	}
}