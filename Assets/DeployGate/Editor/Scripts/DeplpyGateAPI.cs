using UnityEngine;
using UnityEditor;
using System.IO;
using DeployGate.Resources;
namespace DeployGate
{
    public class DeplpyGateAPI
    {
        private const string PUSH_URL = "https://deploygate.com/api/users/{0}/apps";
        private const string INVITE_API = "https://deploygate.com/api/users/{0}/apps/{1}/members";


        private static string progressTitle = "Upload to DeployGate";

        public static void Push(string pathToBuiltProject)
        {
            DeployGatePreference preference = Asset.Load<DeployGatePreference>();

            EditorUtility.DisplayProgressBar(progressTitle, "", 0);
           
            {
                WWWForm form = GetForm(preference, pathToBuiltProject);

                WWW www = new WWW(string.Format(PUSH_URL, preference.user.username), form);

                while (!www.isDone)
                {
                    EditorUtility.DisplayProgressBar(progressTitle,
                        string.Format("Uploading... {0}%", Mathf.FloorToInt(www.uploadProgress * 100)), www.uploadProgress);
                    System.Threading.Thread.Sleep(1);
                }
            }

            EditorUtility.ClearProgressBar();
            SaveMessage(pathToBuiltProject);
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
            return string.IsNullOrEmpty(text) ? string.Empty : JsonFx.Json.JsonReader.Deserialize<Message>(text).text;
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
            Message message = JsonFx.Json.JsonReader.Deserialize<Message>(text);

            if (!string.IsNullOrEmpty(message.text))
                File.WriteAllText(DeployGateUtility.messageLogFolderPath + DeployGateUtility.SEPARATOR + message.date.ToString("u").Replace(":", "-") + ".json", text);
        }
    }
}
