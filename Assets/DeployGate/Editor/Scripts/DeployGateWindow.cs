using UnityEngine;
using UnityEditor;

namespace DeployGate
{
    public class DeployGateWindow : EditorWindow
    {
        private string[] secsions = new string[] { "Build & Upload", "Members", "Settings", "Help" };
        private DeployGatePreference preference;

        void OnEnable()
        {
            preference = Asset.Load<DeployGatePreference>();
            DeployGateBuildWindow.Reset();
            EditorUtility.UnloadUnusedAssets();
        }

        void OnDisable()
        {
            Asset.Save<DeployGatePreference>(preference);
        }

        public static DeployGateWindow GetWindow()
        {
            DeployGateWindow window = GetWindow<DeployGateWindow>(true, "DeployGateWindow");
            SetWindowSize(window);
            return window;
        }

        public static void CloseWindow()
        {
            GetWindow<DeployGateWindow>().Close();
        }

        private static void SetWindowSize(EditorWindow window)
        {
            int left = EditorPrefs.GetInt("UnityEditor.PreferencesWindowx", 96);
            int top = EditorPrefs.GetInt("UnityEditor.PreferencesWindowy", 271);
            int width = EditorPrefs.GetInt("UnityEditor.PreferencesWindoww", 500);
            int height = EditorPrefs.GetInt("UnityEditor.PreferencesWindowh", 400);
            window.position = new Rect(left, top, width, height);
            window.minSize = new Vector2(width, height);
            window.maxSize = window.minSize;
        }

        private GUIStyle sectionStyle = new GUIStyle("PreferencesSection");
        public DeployGateWindowUtility.DeployGateSelection selection = DeployGateWindowUtility.DeployGateSelection.BuildUpload;

        void OnGUI()
        {
            try
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.BeginVertical(GUILayout.Width(120));
                    {
                        ///===============
                        ///  SelectionBox
                        ///===============

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
                        ///==============
                        ///  Selections
                        ///==============

                        switch (selection)
                        {
                            case DeployGateWindowUtility.DeployGateSelection.BuildUpload:
                                DeployGateBuildWindow.OnGUI_BuildWindow(preference);
                                break;

                            case DeployGateWindowUtility.DeployGateSelection.Members:
                                DeployGateMembersWindow.OnGUI_DeployGateMembersWindow();
                                break;

                            case DeployGateWindowUtility.DeployGateSelection.Setings:
                                DeployGatePreferenceWindow.OnGUI_PreferenceWindow(preference);
                                break;

                            case DeployGateWindowUtility.DeployGateSelection.Help:
                            default:
                                DeployGateHelpWindow.OnGUI_DeployGateHelpWindow();
                                break;
                        }
                    }

                    GUILayout.EndVertical();

                }
                GUILayout.EndHorizontal();
            }
            catch (System.InvalidOperationException)
            {
            }
        }
    }
}