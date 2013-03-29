using UnityEngine;
using System;

namespace DeployGate.Resources
{
	[System.Serializable]
	public class Message
	{
		public string title;
		public string text;
		public DateTime date;
		public string version;
		public int versionCode;
	}
	
	[System.Serializable]
	public class TempPath
	{
		public string directryPath{
			get{
				return string.Format("DeployGate{0}Temp",DeployGateUtility.SEPARATOR);
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