using UnityEngine;
using UnityEditor;
using System.IO;
using DeployGate;

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
		public static readonly char SEPARATOR = Path.DirectorySeparatorChar;
		public static readonly string DEPLOYGATE_PLUGINS_PATH = string.Format ("Assets{0}DeployGate{0}Plugins", SEPARATOR);
		public static readonly string PLUGINS_PATH = string.Format ("Assets{0}Plugins", SEPARATOR);
		
		private static string DeployGateFolderPath {
			get {
				string deployGateUtilityPath = new System.Diagnostics.StackTrace (true).GetFrame (0).GetFileName ();
				return deployGateUtilityPath.Replace (Application.dataPath.Replace (Path.AltDirectorySeparatorChar, SEPARATOR), "Assets").Replace (string.Format ("DeployGate{0}Editor{0}Scripts{0}DeployGateUtility.cs", SEPARATOR), "DeployGate");
			}
		}
	
		public static string scriptsFolderPath {
			get {
				
				return string.Format ("{0}{1}Editor{1}Scripts", DeployGateFolderPath, SEPARATOR);
			}
		}
	
		public static string imagesFolderPath {
			get {
				return string.Format ("{0}{1}Editor{1}Images", DeployGateFolderPath, SEPARATOR);
			}
		}

		public static string settingsFolderPath {
			get {
				return string.Format ("{0}{1}Editor{1}Settings", DeployGateFolderPath, SEPARATOR);
			}
		}
	
		public static string messageLogFolderPath {
			get {
				return string.Format ("DeployGate{0}MessageLogs", SEPARATOR);
			}
		}
		
		public static void MoveDeployGateSDK (string from, string to)
		{
			Directory.CreateDirectory("Assets/Plugins");
			Directory.CreateDirectory("Assets/Plugins/Android");
			AssetDatabase.Refresh ();
			MoveAsset (string.Format ("{0}{1}Android{1}deploygatesdk.jar", from, SEPARATOR), string.Format ("{0}{1}Android{1}deploygatesdk.jar", to, SEPARATOR));
			MoveAsset (string.Format ("{0}{1}DeployGateSDK.cs", from, SEPARATOR), string.Format ("{0}{1}DeployGateSDK.cs", to, SEPARATOR));
			AssetDatabase.Refresh ();
		}

		private	static void MoveAsset (string from, string to)
		{
			if (string.IsNullOrEmpty (AssetDatabase.ValidateMoveAsset (from, to)))
				AssetDatabase.MoveAsset (from, to);
		}
		
	}
}