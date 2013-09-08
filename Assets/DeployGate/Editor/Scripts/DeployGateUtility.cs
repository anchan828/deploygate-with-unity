using System;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace DeployGate
{
    public class DeployGateUtility
    {
        public static readonly GUIContent DeploygateUrl = new GUIContent("https://deploygate.com/");
        public static readonly GUIContent DeploygateDashboardUrl = new GUIContent("https://deploygate.com/dashboard");
        public static readonly GUIContent DeploygateRemoteLogcatUrl = new GUIContent("https://deploygate.com/docs/sdk#remote-logcat");
        public static readonly GUIContent DeploygateAccountUrl = new GUIContent("https://deploygate.com/settings");
        public static readonly GUIContent DeploygateDocsUrl = new GUIContent("https://deploygate.com/docs");


        public static readonly char Separator = Path.DirectorySeparatorChar;
        public static readonly string PluginsPath = string.Format("Assets{0}Plugins{0}Android", Separator);
        public static readonly string SdkName = "deploygatesdk";
        public static readonly string ZipSdkPath = "DeployGate/SDK";
        private static string DeployGateFolderPath
        {
            get
            {
                string deployGateUtilityPath = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();
                return deployGateUtilityPath.Replace(Application.dataPath.Replace(Path.AltDirectorySeparatorChar, Separator), "Assets").Replace(string.Format("DeployGate{0}Editor{0}Scripts{0}DeployGateUtility.cs", Separator), "DeployGate");
            }
        }

        public static string scriptsFolderPath
        {
            get
            {

                return string.Format("{0}{1}Editor{1}Scripts", DeployGateFolderPath, Separator);
            }
        }

        public static string imagesFolderPath
        {
            get
            {
                return string.Format("{0}{1}Editor{1}Images", DeployGateFolderPath, Separator);
            }
        }

        public static string settingsFolderPath
        {
            get
            {
                return string.Format("{0}{1}Editor{1}Settings", DeployGateFolderPath, Separator);
            }
        }

        public static string messageLogFolderPath
        {
            get
            {
                return string.Format("DeployGate{0}MessageLogs", Separator);
            }
        }

        public static bool installedSDK
        {
            get
            {
                return File.Exists(PluginsPath + "/" + SdkName + ".jar");
            }
        }

        public static bool showWelcomeWindow
        {
            get
            {
                bool result;
                string configValue = EditorUserSettings.GetConfigValue("showdeployGateWelcomePage");
                if (string.IsNullOrEmpty(configValue))
                {
                    result = true;
                    EditorUserSettings.SetConfigValue("showdeployGateWelcomePage", Convert.ToString(true));
                }
                else
                {
                    result = Convert.ToBoolean(configValue);
                }
                return result;
            }
            set
            {
                EditorUserSettings.SetConfigValue("showdeployGateWelcomePage", Convert.ToString(value));
            }
        }
    }
}