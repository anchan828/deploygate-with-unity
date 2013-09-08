using System;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;
using DeployGate.Resources;

namespace DeployGate
{
    public class DeployGateBuildPipeline
    {
        public static void Build(Message message)
        {
            Directory.CreateDirectory(Asset.preference.temp.directryPath);

            var options = BuildOptions.None;

            if (EditorUserBuildSettings.development)
            {
                options |= BuildOptions.Development;

                if (EditorUserBuildSettings.allowDebugging)
                    options |= BuildOptions.AllowDebugging;
                if (EditorUserBuildSettings.connectProfiler)
                    options |= BuildOptions.ConnectWithProfiler;
            }


            DeployGatePreference.BuildType buildType = Asset.preference.buildType;


            string locationPath = string.Format("{0}{1}{2}", Asset.preference.temp.directryPath, DeployGateUtility.Separator, Guid.NewGuid());

            SaveTempMessage(message, locationPath + ".json");

            if (buildType == DeployGatePreference.BuildType.APK)
            {
                locationPath = locationPath.Contains(".apk") ? locationPath : locationPath + ".apk";
            }
            else
            {
                options |= BuildOptions.AcceptExternalModificationsToPlayer;
                locationPath = EditorUtility.SaveFolderPanel("Export Android Project", "", "");
                if (string.IsNullOrEmpty(locationPath))
                    return;
            }

            Asset.preference.forceInternetPermission = PlayerSettings.Android.forceInternetPermission;
            PlayerSettings.Android.forceInternetPermission = true;

            if (Asset.preference.includeReadLog)
            {
                AddPermission();
            }

            BuildPipeline.BuildPlayer(GetEnableSceneNames(), locationPath, BuildTarget.Android, options);
        }

        private static void AddPermission()
        {
            string templateManifest;

            string manifestPath = string.Format("{0}{1}Plugins{1}Android{1}AndroidManifest.xml", Application.dataPath, DeployGateUtility.Separator);
            if (File.Exists(manifestPath))
            {
                templateManifest = File.ReadAllText(manifestPath);

                if (templateManifest.Contains("android.permission.READ_LOGS"))
                    return;

            }
            else
            {
                string path = string.Format("{0}{1}PlaybackEngines{1}AndroidPlayer{1}AndroidManifest.xml", EditorApplication.applicationContentsPath, DeployGateUtility.Separator);
                templateManifest = File.ReadAllText(path);
            }
            templateManifest = templateManifest.Replace("</manifest>", "<uses-permission android:name=\"android.permission.READ_LOGS\" /></manifest>");
            File.WriteAllText(manifestPath, templateManifest);
            AssetDatabase.Refresh();
        }

        private static void SaveTempMessage(Message message, string tempPath)
        {
            DateTime now = DateTime.UtcNow;
            message.date = now.ToString("u");
            message.version = PlayerSettings.bundleVersion;
            message.title = now.ToLocalTime().ToString("U") + " version=" + PlayerSettings.bundleVersion;
            string json = MiniJSON.Json.Serialize(message);
            json = json.Substring(1, json.Length - 2).Replace("\\\"", "\"");
            File.WriteAllText(tempPath, json);
            Asset.preference.temp.messagePath = tempPath;
        }

        private static string[] GetEnableSceneNames()
        {
            return EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray();
        }
    }
}