using System.Globalization;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using DeployGate.Resources;

namespace DeployGate
{
		public class DeployGateUploadWindow : DeployGateWindowUtility
		{
				private static List<Message> messages = new List<Message> ();
				private static int selectedMessage;
				private static string[] displayOptions = new string[0];

				public static void Reset ()
				{
						messages.Clear ();
						selectedMessage = 0;
						displayOptions = new string[0];
						GetMessages ();
				}

				public static void OnGUI_BuildWindow ()
				{
						if (displayOptions.Length == 0)
								GetMessages ();

						GUILayout.Label (I18n.upload, sectionHeader);

						Headline (I18n.identification);
						GUILayout.BeginHorizontal ();
						{
								GUILayout.Label ("Version", GUILayout.Width (50));
								PlayerSettings.bundleVersion = GUILayout.TextField (PlayerSettings.bundleVersion, GUILayout.Width (50));
								GUILayout.Label ("Version Code", GUILayout.Width (80));
								PlayerSettings.Android.bundleVersionCode = int.Parse (GUILayout.TextField (PlayerSettings.Android.bundleVersionCode.ToString (CultureInfo.InvariantCulture), GUILayout.Width (50)));
						}
						GUILayout.EndHorizontal ();

						Headline (I18n.message);
						GUILayout.BeginHorizontal ();
						{
								EditorGUI.BeginChangeCheck ();

								selectedMessage = EditorGUILayout.Popup (selectedMessage, displayOptions);
								if (EditorGUI.EndChangeCheck ()) {
										_Repaint ();
								}

								if (GUILayout.Button (I18n.delete)) {
										if (selectedMessage == 0) {
												messages [0].text = string.Empty;
												_Repaint ();
										} else {
												File.Delete (DeployGateUtility.messageLogFolderPath + DeployGateUtility.Separator + messages [selectedMessage].date.Replace (":", "-") + ".json");
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

						DrawBuildType ();
						DrawBuildButton ();
						OnGUI_DeployGateInfo ();
				}

				static void DrawBuildType ()
				{
						Headline (I18n.buildType);
						Asset.preference.buildType = (DeployGatePreference.BuildType)EditorGUILayout.EnumPopup (Asset.preference.buildType, GUILayout.Width (Screen.width / 3));
				}

				static void DrawBuildButton ()
				{
						if (GUI.Button (new Rect (Screen.width * 0.8f - 20, Screen.height - 40, Screen.width * 0.20f, 30), I18n.upload)) {
								string error = "";
								if (CanBuild (ref error)) {
										DeployGateWindow.CloseWindow ();
										DeployGateBuildPipeline.Build (messages [0]);
								} else {
										DeployGateWindow.GetWindow ().ShowNotification (new GUIContent (error));
								}
								GUIUtility.ExitGUI ();
						}
				}

				static bool CanBuild (ref string error)
				{
						if (string.IsNullOrEmpty (Asset.preference.user.username) || string.IsNullOrEmpty (Asset.preference.user.token)) {
								error = I18n.profileError.text;
								DeployGateWindow.GetWindow ().selection = DeployGateSelection.Setings;
						}
						if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable ()) {
								error = I18n.networkError.text;
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
								messages.Add (MiniJSON.Json.Deserialize<Message> (File.ReadAllText (file)));
						}
						messages.Add (new Message { title = "new Message" });
						messages.Reverse ();
						ArrayUtility.AddRange (ref displayOptions, messages.Select ((message, i) => message.title).ToArray ());
				}
		}
}