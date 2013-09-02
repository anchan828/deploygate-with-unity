using UnityEngine;
using UnityEditor;

namespace DeployGate
{
	public class DeployGateHelpWindow : DeployGateWindowUtility
	{
		public static void OnGUI_DeployGateHelpWindow ()
		{
			GUILayout.Label ("Help", sectionHeader);
			EditorGUILayout.Space ();
			if (ButtonField ("DeployGate", DeployGateUtility.DEPLOYGATE_URL.text)) {
				Application.OpenURL (DeployGateUtility.DEPLOYGATE_URL.text);
			}
			EditorGUILayout.Space ();
			if (ButtonField ("Profile", DeployGateUtility.DEPLOYGATE_ACCOUNT_URL.text)) {
				Application.OpenURL (DeployGateUtility.DEPLOYGATE_ACCOUNT_URL.text);
			}
			GUILayout.Space (20);
			Headline ("HowTo");
			Howto ();
			
			OnGUI_DeployGateInfo ();
		}
		
		private static void Howto ()
		{
			if (ButtonField ("How to use DeployGate", "Go to page")) {
				Application.OpenURL (DeployGateUtility.DEPLOYGATE_DOCS_URL.text);
			}
			EditorGUILayout.Space ();
			if (ButtonField ("How to use Assets", "Go to page")) {
				Application.OpenURL (DeployGateUtility.DEPLOYGATE_DOCS_URL.text);
			}
		}
		
		private static bool ButtonField (string label, string text)
		{
			bool result = false;
			GUILayout.BeginHorizontal ();
			GUILayout.Label (label);
			result = GUILayout.Button (text, GUILayout.Width (Screen.width * 0.45f));
			GUILayout.EndHorizontal ();
			return result;
		}
	}
}