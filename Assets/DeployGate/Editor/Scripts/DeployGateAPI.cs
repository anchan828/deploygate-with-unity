using System.Threading;
using UnityEngine;
using UnityEditor;
using System.IO;
using DeployGate.Resources;

namespace DeployGate
{
    public class DeployGateApi
    {
        private const string PushApi = "https://deploygate.com/api/users/{0}/apps";
        private const string InviteApi = "https://deploygate.com/api/users/{0}/apps/{1}/members";
        public static readonly string SdkUrl = "https://deploygate.com/client/deploygatesdk-r2.zip";
       
        public static void InstallSdk()
        {
            Directory.CreateDirectory("Assets/Plugins/Android");
            EditorUtility.DisplayProgressBar("Downloading DeployGate SDK...", "", 0);
            WWW www = new WWW(SdkUrl);
            while (!www.isDone)
            {
                EditorUtility.DisplayProgressBar("Downloading DeployGate SDK...",
                                   www.progress * 100 + "%", www.progress);
                Thread.Sleep(10);
            }
            EditorUtility.ClearProgressBar();
            if (string.IsNullOrEmpty(www.error))
            {
                Directory.CreateDirectory("DeployGate/SDK");
                File.WriteAllBytes("DeployGate/SDK/deploygatesdk-r2.zip", www.bytes);
                EditorUtility.RevealInFinder("DeployGate/SDK/deploygatesdk.zip");
            }
        }

        public static void Push(string pathToBuiltProject)
        {
            DeployGatePreference preference = Asset.Load<DeployGatePreference>();

            EditorUtility.DisplayProgressBar("Upload to DeployGate", "", 0);
            {
                WWWForm form = GetForm(preference, pathToBuiltProject);

                WWW www = new WWW(string.Format(PushApi, preference.user.username), form);

                while (!www.isDone)
                {
                    EditorUtility.DisplayProgressBar("Upload to DeployGate",
                        string.Format("{0}%", Mathf.FloorToInt(www.uploadProgress * 100)), www.uploadProgress);
                    Thread.Sleep(1);
                }
            }

            EditorUtility.ClearProgressBar();
            SaveMessage(pathToBuiltProject);
        }

        private static WWW _req;
        public static MembersInfo GetMembers()
        {

            if (_req != null) return null;
            DeployGatePreference preference = Asset.Load<DeployGatePreference>();
            string url = string.Format(InviteApi, preference.user.username, PlayerSettings.bundleIdentifier) +
                         "?token=" + preference.user.token;
            _req = new WWW(url);
            while (!_req.isDone)
            {
                Thread.Sleep(1);
            }
            string text = _req.text;
            _req = null;
            return MiniJSON.Json.Deserialize<MembersInfo>(text);
        }

        public static MembersInfo AddMember(int role, string name)
        {
            DeployGatePreference preference = Asset.Load<DeployGatePreference>();
            string url = string.Format(InviteApi, preference.user.username, PlayerSettings.bundleIdentifier);
            var bytes = GetForm(preference, role, name).data;
            WWW www = new WWW(url, bytes);
            while (!www.isDone)
            {
                Thread.Sleep(1);
            }
            return GetMembers();
        }

        public static MembersInfo DeleteMember(Member member)
        {
#if UNITY_EDITOR_OSX
            string url = string.Format(InviteApi, Asset.preference.user.username, PlayerSettings.bundleIdentifier);
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "curl";
            process.StartInfo.Arguments = string.Format("-X DELETE -F \"users=[{0}]\" -F \"token={1}\" {2}", member.name, Asset.preference.user.token, url);
            process.Start();
            process.WaitForExit();
#endif
            return GetMembers();
        }

        private static WWWForm GetForm(DeployGatePreference preference, int role, string name)
        {
            WWWForm form = new WWWForm();
            form.AddField("token", preference.user.token);
            form.AddField("role", role);
            form.AddField("users", "[" + name + "]");
            return form;
        }

        private static WWWForm GetForm(DeployGatePreference preference, string pathToBuiltProject)
        {
            WWWForm form = new WWWForm();
            form.AddField("token", preference.user.token);
            if (!string.IsNullOrEmpty(preference.temp.messagePath))
                form.AddField("message", GetMessage(preference.temp.messagePath) ?? "");
            form.AddBinaryData("file", GetApkBytes(pathToBuiltProject));
            return form;
        }
        private static string GetMessage(string tempMessagePath)
        {
            string text = File.ReadAllText(tempMessagePath ?? "");
            return string.IsNullOrEmpty(text) ? string.Empty : MiniJSON.Json.Deserialize<Message>(text).text;
        }

        private static byte[] GetApkBytes(string pathToBuiltProject)
        {
            return File.ReadAllBytes(pathToBuiltProject);
        }

        private static void SaveMessage(string pathToBuiltProject)
        {
            string text = File.ReadAllText(pathToBuiltProject.Replace(".apk", ".json"));
            if (string.IsNullOrEmpty(text))
                return;
            Message message = MiniJSON.Json.Deserialize<Message>(text);

            if (!string.IsNullOrEmpty(message.text))
                File.WriteAllText(DeployGateUtility.messageLogFolderPath + DeployGateUtility.Separator + message.date.Replace(":", "-") + ".json", text);
        }
    }
}
