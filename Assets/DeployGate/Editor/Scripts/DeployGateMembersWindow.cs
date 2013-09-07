using DeployGate.Resources;
using UnityEditor;
using UnityEngine;

namespace DeployGate
{
		public class DeployGateMembersWindow : DeployGateWindowUtility
		{
				private static MembersInfo membersInfo;
				private static GUIContent[] roles;
				private static int role;
				private static string name = "";

				public static void OnGUI_DeployGateMembersWindow ()
				{
						if (membersInfo == null) {
								membersInfo = DeployGateApi.GetMembers ();
								return;
						}
						roles = new[] { I18n.developer, I18n.tester };
						GUILayout.Label (I18n.member, sectionHeader);

						if (GUI.Button (new Rect (Screen.width - 70, 25, 60, 16), I18n.refresh)) {
								membersInfo = DeployGateApi.GetMembers ();
								GUIUtility.ExitGUI ();
						}

						EditorGUILayout.Space ();

						if (!membersInfo.error) {
								foreach (var member in membersInfo.members) {
										DrawMember (member);
								}
						}

						DrawAddMember ();

						OnGUI_DeployGateInfo ();
				}

				static void DrawMember (Member member)
				{
						EditorGUILayout.BeginHorizontal ("box");
						EditorGUILayout.LabelField (member.role == 1 ? I18n.developer : I18n.tester, new GUIContent (member.name));
						if (GUILayout.Button ("x")) {
								membersInfo = DeployGateApi.DeleteMember (member);
						}
						EditorGUILayout.EndHorizontal ();
				}

				static void DrawAddMember ()
				{
						Rect rect = GUILayoutUtility.GetRect (I18n.addMember, sectionHeader);
						rect.y = Screen.height - 100;
						GUI.Label (rect, I18n.addMember, sectionHeader);
						rect.y += 25;
						rect.width = 80;
						role = EditorGUI.Popup (rect, role, roles);
						rect.x += rect.width + 10;
						rect.width = 200;
						rect.height = 16;
						name = EditorGUI.TextField (rect, name);
						rect.x += rect.width + 10;
						rect.width = 50;
						if (GUI.Button (rect, I18n.add)) {
								membersInfo = DeployGateApi.AddMember (role + 1, name);
						}
				}
		}
}