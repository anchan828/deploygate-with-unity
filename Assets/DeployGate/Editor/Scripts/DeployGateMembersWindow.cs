using DeployGate.Resources;
using UnityEditor;
using UnityEngine;
using System.Collections;

namespace DeployGate
{
    public class DeployGateMembersWindow : DeployGateWindowUtility
    {
        private static MembersInfo membersInfo = null;

        private static string[] roles = new[] { "Developer", "Tester" };
        private static int role = 0;
        private static string name = "";
        public static void OnGUI_DeployGateMembersWindow()
        {
            if (membersInfo == null)
            {
                membersInfo = DeployGateAPI.GetMembers() ?? default(MembersInfo);
                return;
            }
            GUILayout.Label("Members", sectionHeader);

            if (GUI.Button(new Rect(Screen.width - 70, 25, 60, 16), "Refresh"))
            {
                membersInfo = DeployGateAPI.GetMembers() ?? default(MembersInfo);
                EditorGUIUtility.ExitGUI();
            }

            EditorGUILayout.Space();

            if (!membersInfo.error)
            {
                foreach (var member in membersInfo.members)
                {
                    DrawMember(member);
                }
            }

            DrawAddMember();

            OnGUI_DeployGateInfo();
        }

        static void DrawMember(Member member)
        {
            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.LabelField(member.role == 1 ? "開発者" : "テスター", member.name);
            if (GUILayout.Button("x"))
            {
                membersInfo = DeployGateAPI.DeleteMember(member);
            }
            EditorGUILayout.EndHorizontal();
        }

        static void DrawAddMember()
        {
            Rect rect = GUILayoutUtility.GetRect(new GUIContent("Add Member"), sectionHeader);
            rect.y = Screen.height - 100;
            GUI.Label(rect, "Add Member", sectionHeader);
            rect.y += 25;
            rect.width = 80;
            role = EditorGUI.Popup(rect, role, roles);
            rect.x += rect.width + 10;
            rect.width = 200;
            rect.height = 16;
            name = EditorGUI.TextField(rect, name);
            rect.x += rect.width + 10;
            rect.width = 50;
            if (GUI.Button(rect, "Add"))
            {
                membersInfo = DeployGateAPI.AddMember(role + 1, name);
            }
        }

    }




}