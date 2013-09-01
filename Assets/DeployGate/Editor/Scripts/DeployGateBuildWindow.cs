using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using DeployGate.Resources;


namespace DeployGate
{
	public class DeployGateBuildWindow: DeployGateWindowUtility
	{
		private static List<Message> messages = new List<Message> ();
		private static int selectedMessage = 0;
		private static string[] displayOptions = new string[0];
		private static DeployGatePreference preference;

		public static void Reset ()
		{
			messages.Clear ();
			selectedMessage = 0;
			displayOptions = new string[0];
			GetMessages ();
		}

		public static void OnGUI_BuildWindow (DeployGatePreference preference)
		{
			DeployGateBuildWindow.preference = preference;	
			
			if (displayOptions.Length == 0)
				GetMessages ();
			
			/// ====================
			/// Header
			/// ====================
			GUILayout.Label ("Build & Upload", sectionHeader);
			
			
			Headline ("Identification");
			GUILayout.BeginHorizontal ();
			{
				GUILayout.Label ("Version", GUILayout.Width (50));
				PlayerSettings.bundleVersion = GUILayout.TextField (PlayerSettings.bundleVersion, GUILayout.Width (50));
				GUILayout.Label ("Version Code", GUILayout.Width (80));
				PlayerSettings.Android.bundleVersionCode = int.Parse (GUILayout.TextField (PlayerSettings.Android.bundleVersionCode.ToString (), GUILayout.Width (50)));
			}
			GUILayout.EndHorizontal ();
			
			Headline ("Message");
			///================================
			GUILayout.BeginHorizontal ();
			{
				EditorGUI.BeginChangeCheck ();
			
				selectedMessage = EditorGUILayout.Popup (selectedMessage, displayOptions);
				if (EditorGUI.EndChangeCheck ()) {
					_Repaint ();
				}
			
				if (GUILayout.Button ("Delete")) {
					if (selectedMessage == 0) {
						messages [0].text = string.Empty;
						_Repaint ();
					} else {
						File.Delete (DeployGateUtility.messageLogFolderPath + "/" + messages [selectedMessage].date.ToString ("u") + ".json");
						Reset ();
					}
				}
			}
			GUILayout.EndHorizontal ();
			
			if (selectedMessage != 0) {
				GUILayout.BeginVertical ("box");
				GUILayout.TextArea (messages [selectedMessage].text, GUI.skin.label, GUILayout.Height (Screen.height * 0.4f));
				GUILayout.EndVertical ();
			} else
				messages [0].text = EditorGUILayout.TextArea (messages [0].text, GUILayout.Height (Screen.height * 0.4f));
			
			///================================
			
			DrawBuildType ();
			
			///================================
			
			DrawBuildButton ();
			
			///================================
			
			OnGUI_DeployGateInfo ();
			
		}

		static void DrawBuildType ()
		{
			Headline ("BuildType");
			if (preference != null)
				preference.buildType = (DeployGatePreference.BuildType)EditorGUILayout.EnumPopup (preference.buildType, GUILayout.Width (Screen.width / 3));
		}

		static void DrawBuildButton ()
		{
			if (GUI.Button (new Rect (Screen.width * 0.8f, Screen.height - 40, Screen.width * 0.18f, 30), "Build & Upload")) {
				string error = "";
				if (CanBuild (ref error)) {
					DeployGateWindow.CloseWindow ();
					DeployGateBuildPipeline.Build (messages [0]);
				} else {
					DeployGateWindow.GetWindow ().ShowNotification (new GUIContent (error));
				}
                EditorGUIUtility.ExitGUI();
			}
		}
		
		static bool CanBuild (ref string error)
		{
			if (string.IsNullOrEmpty (preference.user.username) || string.IsNullOrEmpty (preference.user.token)) {
				error = I18n.profileError;
				DeployGateWindow.GetWindow ().selection = DeployGateWindowUtility.DeployGateSelection.Setings;
			}
			if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) {
				error = I18n.networkError;
			}
			return string.IsNullOrEmpty (error);
		}

		static void _Repaint ()
		{
			GUIUtility.keyboardControl = 0;
			GUIUtility.hotControl = 0;
		}

		static void GetMessages ()
		{
			Directory.CreateDirectory (DeployGateUtility.messageLogFolderPath);
			string[] files = Directory.GetFiles (DeployGateUtility.messageLogFolderPath, "*.json");
		
			foreach (string file in files) {
				messages.Add (JsonFx.Json.JsonReader.Deserialize<Message> (File.ReadAllText (file)));
			}
			messages.Add (new Message {title="new Message"});
			messages.Reverse ();
			ArrayUtility.AddRange (ref displayOptions, messages.Select ((message,i) => message.title).ToArray ());
		}

	}

}