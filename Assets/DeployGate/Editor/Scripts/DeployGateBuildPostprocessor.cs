using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using DeployGate;
using DeployGate.Resources;

namespace DeployGate
{
	public class DeployGateBuildPostprocessor
	{
		private static string progressTitle = "Upload to DeployGate";
		private static string pathToBuiltProject = "";
		private static DeployGatePreference preference;

		[PostProcessBuild(9999)]
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
				Directory.Delete (preference.temp.directryPath, true);
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
			string text = File.ReadAllText (tempMessagePath ?? "");
			return string.IsNullOrEmpty (text) ? string.Empty : JsonFx.Json.JsonReader.Deserialize<Message> (text).text;
		}
		
		private static byte[] GetAPKBytes ()
		{
			return File.ReadAllBytes (pathToBuiltProject);
		}
		
		private static void SaveMessage ()
		{
			string text = File.ReadAllText (pathToBuiltProject.Replace (".apk", ".json"));
			if (string.IsNullOrEmpty (text))
				return;
			Message message = JsonFx.Json.JsonReader.Deserialize<Message> (text);
			
			if (!string.IsNullOrEmpty (message.text))
				File.WriteAllText (DeployGateUtility.messageLogFolderPath + DeployGateUtility.SEPARATOR + message.date.ToString ("u").Replace(":","-") + ".json", text);
		}

		
	
	}
}
