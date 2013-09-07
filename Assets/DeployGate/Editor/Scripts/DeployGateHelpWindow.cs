using UnityEngine;
using UnityEditor;

namespace DeployGate
{
    public class DeployGateHelpWindow : DeployGateWindowUtility
    {
        public static void OnGUI_DeployGateHelpWindow()
        {
            GUILayout.Label(I18n.help, sectionHeader);
            EditorGUILayout.Space();
            if (ButtonField(I18n.deployGate, I18n.goToPage))
            {
                Application.OpenURL(DeployGateUtility.DeploygateUrl.text);
            }
            EditorGUILayout.Space();
            if (ButtonField(I18n.profile, I18n.goToPage))
            {
                Application.OpenURL(DeployGateUtility.DeploygateAccountUrl.text);
            }
            GUILayout.Space(20);
            Headline(I18n.howTo);
            Howto();

            Headline(I18n.welcomeWindow);
            if (ButtonField(I18n.welcomeWindow, I18n.show))
            {
                DeployGateWelcomeWindow.GetWindow();
            }
            OnGUI_DeployGateInfo();
        }

        private static void Howto()
        {
            if (ButtonField(I18n.howToUseDeployGate, I18n.goToPage))
            {
                Application.OpenURL(DeployGateUtility.DeploygateDocsUrl.text);
            }
            EditorGUILayout.Space();
            if (ButtonField(I18n.howToUseAssets, I18n.goToPage))
            {
                Application.OpenURL("https://github.com/anchan828/deploygate-with-unity");
            }
        }

        private static bool ButtonField(GUIContent label, GUIContent text)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label);
            bool result = GUILayout.Button(text, GUILayout.Width(Screen.width * 0.45f));
            GUILayout.EndHorizontal();
            return result;
        }
    }
}