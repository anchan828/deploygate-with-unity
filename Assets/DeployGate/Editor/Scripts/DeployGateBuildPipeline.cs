using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;
using DeployGate.Resources;

namespace DeployGate
{
	public class DeployGateBuildPipeline
	{
		private static DeployGatePreference preference;

		public static void Build (Message message)
		{
			preference = Asset.Load<DeployGatePreference> ();
			System.IO.Directory.CreateDirectory (preference.temp.directryPath);
			
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
			
			string locationPath = string.Format ("{0}/{1}", preference.temp.directryPath, System.Guid.NewGuid ());
			
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
			string manifestPath = Application.dataPath + "/Plugins/Android/AndroidManifest.xml";
			if (System.IO.File.Exists (manifestPath)) {
				templeteManifest = System.IO.File.ReadAllText (manifestPath);
				
				if (templeteManifest.Contains ("android.permission.READ_LOGS"))
					return;
				
			} else {
			
				templeteManifest = System.IO.File.ReadAllText (EditorApplication.applicationContentsPath + "/PlaybackEngines/AndroidPlayer/AndroidManifest.xml");
			
				templeteManifest = templeteManifest.Replace ("</manifest>", "<uses-permission android:name=\"android.permission.READ_LOGS\" /></manifest>");
			
				System.IO.File.WriteAllText (manifestPath, templeteManifest);
				AssetDatabase.Refresh ();
			}
		}
		
		private static void SaveTempMessage (Message message, string tempPath)
		{
			message.date = System.DateTime.UtcNow;
			System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo (Application.systemLanguage == SystemLanguage.Japanese ? "ja-JP" : "en-US");
			message.title = message.date.ToLocalTime ().ToString ("U", culture) + "version=" + PlayerSettings.bundleVersion;
			System.IO.File.WriteAllText (tempPath, JsonFx.Json.JsonWriter.Serialize (message));
			preference.temp.messagePath = tempPath;
		}
		
		private static string[] GetEnableSceneNames ()
		{
			return EditorBuildSettings.scenes.Where (scene => scene.enabled).Select (scene => scene.path).ToArray ();
		}
	}
}