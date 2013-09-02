using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEditor;
using System.IO;
using DeployGate.Resources;
namespace DeployGate
{
    public class DeployGateAPI
    {
        private const string PUSH_URL = "https://deploygate.com/api/users/{0}/apps";
        private const string INVITE_API = "https://deploygate.com/api/users/{0}/apps/{1}/members";

       

        public static void Push(string pathToBuiltProject)
        {
            DeployGatePreference preference = Asset.Load<DeployGatePreference>();

            EditorUtility.DisplayProgressBar("Upload to DeployGate", "", 0);
            {
                WWWForm form = GetForm(preference, pathToBuiltProject);

                WWW www = new WWW(string.Format(PUSH_URL, preference.user.username), form);

                while (!www.isDone)
                {
                    EditorUtility.DisplayProgressBar("Upload to DeployGate",
                        string.Format("Uploading... {0}%", Mathf.FloorToInt(www.uploadProgress * 100)), www.uploadProgress);
                    System.Threading.Thread.Sleep(1);
                }
            }

            EditorUtility.ClearProgressBar();
            SaveMessage(pathToBuiltProject);
        }

        private static WWW req;
        public static MembersInfo GetMembers()
        {

            if (req != null) return null;
            DeployGatePreference preference = Asset.Load<DeployGatePreference>();
            string url = string.Format(INVITE_API, preference.user.username, PlayerSettings.bundleIdentifier) +
                         "?token=" + preference.user.token;
            req = new WWW(url);
            while (!req.isDone)
            {
                Thread.Sleep(1);
            }
            string text = req.text;
            req = null;
            return MiniJSON.Json.Deserialize<MembersInfo>(text);
        }

        public static MembersInfo AddMember(int role, string name)
        {
            DeployGatePreference preference = Asset.Load<DeployGatePreference>();
            string url = string.Format(INVITE_API, preference.user.username, PlayerSettings.bundleIdentifier);
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
            DeployGatePreference preference = Asset.Load<DeployGatePreference>();
            string url = string.Format(INVITE_API, preference.user.username, PlayerSettings.bundleIdentifier);
#if UNITY_EDITOR_OSX
            Process process = new Process();
            process.StartInfo.FileName = "curl";
            process.StartInfo.Arguments = string.Format("-X DELETE -F \"users=[{0}]\" -F \"token={1}\" {2}",member.name,preference.user.token,url);
            process.Start();
            process.WaitForExit();
#elif
            Debug.LogException(new NotImplementedException());
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
            form.AddBinaryData("file", GetAPKBytes(pathToBuiltProject));
            return form;
        }
        private static string GetMessage(string tempMessagePath)
        {
            string text = File.ReadAllText(tempMessagePath ?? "");
            return string.IsNullOrEmpty(text) ? string.Empty : MiniJSON.Json.Deserialize<Message>(text).text;
        }

        private static byte[] GetAPKBytes(string pathToBuiltProject)
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
                File.WriteAllText(DeployGateUtility.messageLogFolderPath + DeployGateUtility.SEPARATOR + message.date.Replace(":", "-") + ".json", text);
        }
    }
}
