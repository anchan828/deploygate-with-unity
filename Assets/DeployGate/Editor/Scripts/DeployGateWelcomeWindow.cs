using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DeployGate
{
    public class DeployGateWelcomeWindow : EditorWindow
    {
        public static DeployGateWelcomeWindow GetWindow()
        {
            var window = GetWindow<DeployGateWelcomeWindow>(true, I18n.welcome.text);
            DeployGateWindowUtility.SetWindowSize(window);

            return window;
        }

        enum Page
        {
            Welcome,
            Install,
            Account,
            Finish
        }

        private Page page = Page.Welcome;

        void OnGUI()
        {
            bool buttonRich = GUI.skin.button.richText;
            bool labelRich = EditorStyles.label.richText;
            EditorStyles.label.richText = true;
            GUI.skin.button.richText = true;

            GUILayout.BeginHorizontal();
            GUILayout.Space(position.width * 0.15f);
            string[] pageNames = Enum.GetNames(typeof(Page));
            for (int index = 0; index < pageNames.Length; index++)
            {
                pageNames[index] = (index + 1) + ". " + pageNames[index];
            }
            GUILayout.SelectionGrid((int)page, pageNames, pageNames.Length, EditorStyles.toolbarButton, GUILayout.Width(position.width * 0.7f));
            Asset.preference.language = (DeployGatePreference.Language)EditorGUILayout.EnumPopup(Asset.preference.language, GUILayout.Width(position.width * 0.14f));
          
            GUILayout.EndHorizontal();

            switch (page)
            {
                case Page.Welcome:
                    DrawWelcome();
                    break;
                case Page.Install:
                    DrawInstall();
                    break;
                case Page.Account:
                    DrawAccount();
                    break;
                case Page.Finish:
                    DrawFinish();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            GUI.skin.button.richText = buttonRich;
            EditorStyles.label.richText = labelRich;
        }

        private void DrawWelcome()
        {
            EditorGUILayout.LabelField(I18n.welcome, DeployGateWindowUtility.GetStyle("welcome"));
            GUIStyle logo = DeployGateWindowUtility.GetStyle("logo");

            GUILayout.Space(position.width * 0.1f);
            GUILayout.BeginHorizontal();
            GUILayout.Space(position.width * 0.5f - logo.normal.background.width * 0.5f);
            GUILayout.Label(logo.normal.background, logo);
            GUILayout.EndHorizontal();

            GUILayout.Space(position.width * 0.05f);
            GUILayout.BeginHorizontal();
            GUILayout.Space(position.width * 0.1f);

            if (GUILayout.Button("<size=50><b>" + I18n.letsStart.text + "</b></size>", GUILayout.Width(position.width * 0.8f),
                                  GUILayout.Height(position.height * 0.2f)))
            {
                page = Page.Install;
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(position.width * 0.05f);
            GUILayout.BeginHorizontal();
            GUILayout.Space(position.width * 0.8f);

          
            if (GUILayout.Button(I18n.skip))
            {
                DeployGateUtility.showWelcomeWindow = false;
                EditorApplication.ExecuteMenuItem("Window/DeployGate");
                Close();
            }
            GUILayout.EndHorizontal();

        }

        private void DrawInstall()
        {
            EditorApplication.delayCall += () =>
            {
                if (DeployGateUtility.installedSDK)
                {
                    page = Page.Account;
                }
            };
            EditorGUILayout.LabelField(I18n.installSdk, DeployGateWindowUtility.GetStyle("welcome"));

            GUIStyle install = DeployGateWindowUtility.GetStyle("install");
            Vector2 iconSize = EditorGUIUtility.GetIconSize();

            bool disable = File.Exists(DeployGateUtility.ZipSdkPath + DeployGateUtility.Separator + DeployGateUtility.SdkName + "-r2.zip");

            EditorGUIUtility.SetIconSize(Vector2.one * 128);
            GUILayout.Space(position.width * 0.2f);
            GUILayout.BeginHorizontal();
            GUILayout.Space(position.width * 0.1f);
            EditorGUI.BeginDisabledGroup(disable);
            if (GUILayout.Button(new GUIContent("<size=48><b>" + I18n.getSdk.text + "</b></size>", install.normal.background),
                                  GUILayout.Width(position.width * 0.8f)))
            {
                DeployGateApi.InstallSdk();
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();
            GUILayout.Space(position.width * 0.05f);
            GUILayout.BeginHorizontal();
            GUILayout.Space(position.width * 0.1f);

            EditorGUI.BeginDisabledGroup(!disable);

            if (GUILayout.Button(new GUIContent("<size=32><b>" + I18n.importSDK.text + "</b></size>"),
                                  GUILayout.Width(position.width * 0.8f)))
            {
                string path = EditorUtility.OpenFilePanel("Select DeployGate SDK", DeployGateUtility.ZipSdkPath, "jar");
                File.Copy(path, DeployGateUtility.PluginsPath + DeployGateUtility.Separator + DeployGateUtility.SdkName + ".jar");
                EditorApplication.delayCall += AssetDatabase.Refresh;
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();
            EditorGUIUtility.SetIconSize(iconSize);
            Repaint();
        }

        private void DrawAccount()
        {
            EditorGUILayout.LabelField(I18n.account, DeployGateWindowUtility.GetStyle("welcome"));
            EditorGUIUtility.LookLikeControls(64);
            GUILayout.Space(position.width * 0.3f);

            GUILayout.BeginHorizontal();
            GUILayout.Space(position.width * 0.1f);
            EditorGUI.BeginChangeCheck();
            string username = EditorGUILayout.TextField(I18n.username, Asset.preference.user.username, GUILayout.Width(position.width * 0.8f));
            if (EditorGUI.EndChangeCheck())
            {
                Asset.preference.user.username = username;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(position.width * 0.1f);
            EditorGUI.BeginChangeCheck();
            string apiKey = EditorGUILayout.TextField(I18n.apiKey, Asset.preference.user.token, GUILayout.Width(position.width * 0.8f));
            if (EditorGUI.EndChangeCheck())
            {
                Asset.preference.user.token = apiKey;
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(position.width * 0.05f);
            GUILayout.BeginHorizontal();
            GUILayout.Space(position.width * 0.5f);
            if (GUILayout.Button(I18n.showApiKey, GUILayout.Width(position.width * 0.2f)))
            {
                Help.BrowseURL(DeployGateUtility.DeploygateAccountUrl.text);
            }
            if (GUILayout.Button(I18n.save, GUILayout.Width(position.width * 0.2f)))
            {
                Asset.Save(Asset.preference);
                page = Page.Finish;
            }
            GUILayout.EndHorizontal();

        }

        private void DrawFinish()
        {
            EditorGUILayout.LabelField(I18n.finish, DeployGateWindowUtility.GetStyle("welcome"), GUILayout.Height(80));


            EditorGUI.indentLevel += 4;
            EditorGUILayout.LabelField(I18n.showDeployGateWindow);

            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField(I18n.clickMenuItemWindowDeployGate);
            GUILayout.BeginHorizontal();
            GUILayout.Space(position.width * 0.15f);
            Texture2D hel1Texture = DeployGateWindowUtility.GetStyle("help1").normal.background;
            GUILayout.Label(hel1Texture, GUILayout.Width(hel1Texture.width * 0.8f));
            GUILayout.EndHorizontal();

            DeployGateUtility.showWelcomeWindow = false;

        }
    }
}
