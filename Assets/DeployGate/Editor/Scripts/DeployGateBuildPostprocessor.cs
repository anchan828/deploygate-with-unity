using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Callbacks;
using DeployGate;
using DeployGate.Resources;

namespace DeployGate
{
	public class DeployGateBuildPostprocessor
	{
		private static string progressTitle = "Upload to DeployGate";
		private static string pathToBuiltProject = "";
		private static DeployGatePreference preference;

		[PostProcessBuild]
		public static void OnPostprocessBuild (BuildTarget target, string pathToBuiltProject)
		{
			DeployGateBuildPostprocessor.pathToBuiltProject = pathToBuiltProject;
			preference = Asset.Load<DeployGatePreference> ();
			if (target == BuildTarget.Android && pathToBuiltProject.Contains (preference.temp.directryPath))
				AndroidPostprocessBuild ();
		}

		private static 	WWW www = null;

		private static void AndroidPostprocessBuild ()
		{
			EditorApplication.LockReloadAssemblies ();
			EditorUtility.DisplayProgressBar (progressTitle, "", 0);
	
			
			
			PlayerSettings.Android.forceInternetPermission = preference.forceInternetPermission;
		
			if (pathToBuiltProject.LastIndexOf (".apk") != -1) {
				
				
				WWWForm form = GetForm (preference);
				
				
				
				www = new WWW (string.Format (DeployGateUtility.PUSH_URL, preference.user.username), form);
				while (!www.isDone) {
					EditorUtility.DisplayProgressBar (progressTitle, string.Format ("Uploading... {0}%", Mathf.FloorToInt (www.uploadProgress * 100)), www.uploadProgress);
					System.Threading.Thread.Sleep (1);
				}
				EditorUtility.ClearProgressBar ();	
				SaveMessage ();
		
				//Delete Temp
				System.IO.Directory.Delete (preference.temp.directryPath, true);
				DeployGateUtility.MoveDeployGateSDK (DeployGateUtility.PLUGINS_PATH, DeployGateUtility.DEPLOYGATE_PLUGINS_PATH);
				Asset.Load<DeployGatePreference> ().temp.messagePath = "";
			
			} else {
				// Eclipse Project
			}
			EditorApplication.UnlockReloadAssemblies ();
		}

		private static WWWForm GetForm (DeployGatePreference preference)
		{
			WWWForm form = new WWWForm ();
			form.AddField ("token", preference.user.token);
			if (!string.IsNullOrEmpty (preference.temp.messagePath))
				form.AddField ("message", GetMessage (preference.temp.messagePath) ?? "");
			form.AddBinaryData ("file", GetAPKBytes ());
			return form;
		}
		
		private static string GetMessage (string tempMessagePath)
		{
			string text = System.IO.File.ReadAllText (tempMessagePath ?? "");
			return string.IsNullOrEmpty (text) ? string.Empty : JsonFx.Json.JsonReader.Deserialize<Message> (text).text;
		}
		
		private static byte[] GetAPKBytes ()
		{
			return System.IO.File.ReadAllBytes (pathToBuiltProject);
		}
		
		private static void SaveMessage ()
		{
			string text = System.IO.File.ReadAllText (pathToBuiltProject.Replace (".apk", ".json"));
			if (string.IsNullOrEmpty (text))
				return;
			Message message = JsonFx.Json.JsonReader.Deserialize<Message> (text);
			
			if (!string.IsNullOrEmpty (message.text))
				System.IO.File.WriteAllText (DeployGateUtility.messageLogFolderPath + "/" + message.date.ToString ("u") + ".json", text);
		}

		
	
	}
}
