using UnityEngine;
using System.Collections;
using UnityEditor;

namespace DeployGate
{
	public class DeployGatePreferenceWindow: DeployGateWindowUtility
	{
		private static DeployGatePreference preference;

		public static	void OnGUI_PreferenceWindow (DeployGatePreference preference)
		{
			DeployGatePreferenceWindow.preference = preference;
			GUILayout.Label ("Settings", sectionHeader);
			///================================
			
			DrawAccount ();
			
			///================================
			EditorGUILayout.Space ();
			DrawPermissions ();
			
			///================================
			
			OnGUI_DeployGateInfo ();
			
		}

		static void DrawPermissions ()
		{
			if (preference == null)
				return;
			Headline ("Permissions");
			GUILayout.BeginVertical ("box");
			preference.includeReadLog = GUILayout.Toggle (preference.includeReadLog, "Include android.permission.READ_LOGS");
			GUILayout.EndVertical ();
		}

		static void DrawAccount ()
		{
			Headline ("Account");
		
			GUILayout.BeginVertical ("box");
			{
				preference.user.username = EditorGUILayout.TextField ("Your Name", preference.user.username);
				
				preference.user.token = EditorGUILayout.TextField ("API Key", preference.user.token);
			
				
			}
			
			GUILayout.EndVertical ();
		}
	}
}
