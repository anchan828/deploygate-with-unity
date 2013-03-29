using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;
using DeployGate.Resources;

namespace DeployGate
{
	public class DeployGateBuildPipeline
	{
		private static DeployGatePreference preference;

		public static void Build (Message message)
		{
			preference = Asset.Load<DeployGatePreference> ();
			Directory.CreateDirectory (preference.temp.directryPath);
			
			DeployGateUtility.MoveDeployGateSDK (DeployGateUtility.DEPLOYGATE_PLUGINS_PATH, DeployGateUtility.PLUGINS_PATH);
			
			var options = BuildOptions.None;
			
			if (EditorUserBuildSettings.development) {
				options |= BuildOptions.Development;
				
				if (EditorUserBuildSettings.allowDebugging)
					options |= BuildOptions.AllowDebugging;
				if (EditorUserBuildSettings.connectProfiler)
					options |= BuildOptions.ConnectWithProfiler;
			}
			
			
			DeployGatePreference.BuildType buildType = preference.buildType;
			
			
			string locationPath = string.Format ("{0}{1}{2}", preference.temp.directryPath, DeployGateUtility.SEPARATOR, System.Guid.NewGuid ());
			
			SaveTempMessage (message, locationPath + ".json");
			
			if (buildType == DeployGatePreference.BuildType.APK) {
				locationPath = locationPath.Contains (".apk") ? locationPath : locationPath += ".apk";
			} else {
				options |= BuildOptions.AcceptExternalModificationsToPlayer;
				locationPath = EditorUtility.SaveFolderPanel ("Export Eclipse Project", "", "");
				if (string.IsNullOrEmpty (locationPath))
					return;
			}
			
			preference.forceInternetPermission = PlayerSettings.Android.forceInternetPermission;
			PlayerSettings.Android.forceInternetPermission = true;
			
			if (preference.includeReadLog) {
				AddPermission (preference);
			}
			
			BuildPipeline.BuildPlayer (GetEnableSceneNames (), locationPath, BuildTarget.Android, options);
		}
		
		private static void AddPermission (DeployGatePreference preference)
		{
			string templeteManifest = "";
			
			string manifestPath = string.Format ("{0}{1}Plugins{1}Android{1}AndroidManifest.xml", Application.dataPath, DeployGateUtility.SEPARATOR);
			if (File.Exists (manifestPath)) {
				templeteManifest = File.ReadAllText (manifestPath);
				
				if (templeteManifest.Contains ("android.permission.READ_LOGS"))
					return;
				
			} else {
			
				templeteManifest = string.Format ("{0}{1}PlaybackEngines{1}AndroidPlayer{1}AndroidManifest.xml", EditorApplication.applicationContentsPath, DeployGateUtility.SEPARATOR);
			
				templeteManifest = templeteManifest.Replace ("</manifest>", "<uses-permission android:name=\"android.permission.READ_LOGS\" /></manifest>");
			
				File.WriteAllText (manifestPath, templeteManifest);
				AssetDatabase.Refresh ();
			}
		}
		
		private static void SaveTempMessage (Message message, string tempPath)
		{
			message.date = System.DateTime.UtcNow;
			System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo (I18n.local);
			message.title = message.date.ToLocalTime ().ToString ("U", culture) + " version=" + PlayerSettings.bundleVersion;
			File.WriteAllText (tempPath, JsonFx.Json.JsonWriter.Serialize (message));
			preference.temp.messagePath = tempPath;
		}
		
		private static string[] GetEnableSceneNames ()
		{
			return EditorBuildSettings.scenes.Where (scene => scene.enabled).Select (scene => scene.path).ToArray ();
		}
	}
}