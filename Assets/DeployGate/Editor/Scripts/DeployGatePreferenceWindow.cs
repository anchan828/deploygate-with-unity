using UnityEngine;
using UnityEditor;

namespace DeployGate
{
    public class DeployGatePreferenceWindow : DeployGateWindowUtility
    {
        public static void OnGUI_PreferenceWindow()
        {
            GUILayout.Label(I18n.settings, sectionHeader);
            DrawAccount();
            EditorGUILayout.Space();
            DrawPermissions();
            DrawInstallSdk();
            DrawLanguage();
            OnGUI_DeployGateInfo();
        }

        static void DrawInstallSdk()
        {
            Headline(I18n.installSdk);
            if (GUILayout.Button(I18n.getSdk))
            {
                DeployGateApi.InstallSdk();
            }
        }

        static void DrawLanguage()
        {
            Headline(I18n.language);
            EditorGUI.BeginChangeCheck();
            var language = (DeployGatePreference.Language)EditorGUILayout.EnumPopup(" ", Asset.preference.language);
            if (EditorGUI.EndChangeCheck())
            {
                Asset.preference.language = language;
            }
        }

        static void DrawPermissions()
        {

            Headline(I18n.permissions);
            GUILayout.BeginVertical("box");
            Asset.preference.includeReadLog = GUILayout.Toggle(Asset.preference.includeReadLog, I18n.permissions_readlog);
            GUILayout.EndVertical();
        }

        static void DrawAccount()
        {
            Headline(I18n.account);

            GUILayout.BeginVertical("box");
            {
                Asset.preference.user.username = EditorGUILayout.TextField(I18n.username, Asset.preference.user.username);

                Asset.preference.user.token = EditorGUILayout.TextField(I18n.apiKey, Asset.preference.user.token);
            }

            GUILayout.EndVertical();
        }
    }
}
