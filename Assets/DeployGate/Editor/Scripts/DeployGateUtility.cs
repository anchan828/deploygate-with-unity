using UnityEngine;
using System.Collections;
using DeployGate;
using UnityEditor;
namespace DeployGate
{
	public class DeployGateUtility
	{
		public static readonly GUIContent DEPLOYGATE_URL = new GUIContent ("https://deploygate.com/");
		public static readonly GUIContent DEPLOYGATE_DASHBOARD_URL = new GUIContent ("https://deploygate.com/dashboard");
		public static readonly GUIContent DEPLOYGATE_REMOTE_LOGCAT_URL = new GUIContent ("https://deploygate.com/docs/sdk#remote-logcat");
		public static readonly GUIContent DEPLOYGATE_ACCOUNT_URL = new GUIContent ("https://deploygate.com/settings");
		public static readonly GUIContent DEPLOYGATE_DOCS_URL = new GUIContent ("https://deploygate.com/docs");
		public const string PUSH_URL = "https://deploygate.com/api/users/{0}/apps";
		public const string DEPLOYGATE_PLUGINS_PATH = "Assets/DeployGate/Plugins";
		public const string PLUGINS_PATH = "Assets/Plugins";
		
		private static string currentFolderPath {
			get {
				string currentFilePath = new System.Diagnostics.StackTrace (true).GetFrame (0).GetFileName ();
				return "Assets" + currentFilePath.Substring (0, currentFilePath.LastIndexOf ("/") + 1).Replace (Application.dataPath, string.Empty);
			}
		}
	
		public static string scriptsFolderPath {
			get {
				return currentFolderPath;
			}
		}
	
		public static string imagesFolderPath {
			get {
				return currentFolderPath.Replace ("Scripts", "Images");
			}
		}

		public static string settingsFolderPath {
			get {
				return currentFolderPath.Replace ("Scripts", "Settings");
			}
		}
	
		public static string messageLogFolderPath {
			get {
				return  Application.dataPath + "/../DeployGate/MessageLogs";
			}
		}
		
		public static void MoveDeployGateSDK (string from, string to)
		{
			MoveAsset (from + "/Android/deploygatesdk.jar", to + "/Android/deploygatesdk.jar");
			MoveAsset (from + "/DeployGateSDK.cs", to + "/DeployGateSDK.cs");
			AssetDatabase.Refresh ();
		}

		private	static void MoveAsset (string from, string to)
		{
			if (string.IsNullOrEmpty (AssetDatabase.ValidateMoveAsset (from, to)))
				AssetDatabase.MoveAsset (from, to);
		}
		
	}
}