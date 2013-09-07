using UnityEngine;
using UnityEditor;

namespace DeployGate
{
    public class DeployGateWindow : EditorWindow
    {
        private GUIContent[] secsions = new GUIContent[] { I18n.upload, I18n.member, I18n.settings, I18n.help };

        void OnEnable()
        {
            DeployGateUploadWindow.Reset();
            EditorUtility.UnloadUnusedAssets();
        }

        void OnDisable()
        {
            Asset.Save(Asset.preference);
        }

        public static DeployGateWindow GetWindow()
        {
            var window = GetWindow<DeployGateWindow>(true, "DeployGateWindow");
            DeployGateWindowUtility.SetWindowSize(window);
            return window;
        }

        public static void CloseWindow()
        {
            GetWindow<DeployGateWindow>().Close();
        }

        private GUIStyle sectionStyle = new GUIStyle("PreferencesSection");
        public DeployGateWindowUtility.DeployGateSelection selection = DeployGateWindowUtility.DeployGateSelection.BuildUpload;

        void OnGUI()
        {
            secsions = secsions = new[] { I18n.upload, I18n.member, I18n.settings, I18n.help };
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical(GUILayout.Width(120));
                {
                    GUILayout.Space(30);
                    EditorGUIUtility.LookLikeControls(180f);
                    GUI.DrawTexture(new Rect(0, 0, Screen.width * 0.24f, Screen.height), DeployGateWindowUtility.backgroundTexture);
                    GUI.Label(new Rect(0, 0, Screen.width * 0.24f, Screen.height), "", "PreferencesSectionBox");
                    sectionStyle.normal.textColor = EditorGUIUtility.isProSkin ? new Color(0.7f, 0.7f, 0.7f, 1) : Color.black;
                    selection = (DeployGateWindowUtility.DeployGateSelection)GUILayout.SelectionGrid((int)selection, secsions, 1, sectionStyle);
                    sectionStyle.onNormal.background = DeployGateWindowUtility.onNomalTexture;
                }
                GUILayout.EndVertical();


                GUILayout.BeginVertical();
                {
                    switch (selection)
                    {
                        case DeployGateWindowUtility.DeployGateSelection.BuildUpload:
                            DeployGateUploadWindow.OnGUI_BuildWindow();
                            break;

                        case DeployGateWindowUtility.DeployGateSelection.Members:
                            DeployGateMembersWindow.OnGUI_DeployGateMembersWindow();
                            break;

                        case DeployGateWindowUtility.DeployGateSelection.Setings:
                            DeployGatePreferenceWindow.OnGUI_PreferenceWindow();
                            break;

                        default:
                            DeployGateHelpWindow.OnGUI_DeployGateHelpWindow();
                            break;
                    }
                }

                GUILayout.EndVertical();

            }
            GUILayout.EndHorizontal();
        }
    }
}