using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
    private bool initialized;
    private string updateAvailableString = "";
    void Start()
    {
        DeployGateSDK.initializedChanged += (initialized) =>
        {
            this.initialized = initialized;
        };
        
        DeployGateSDK.updateAvailableChanged += (revision, versionName, code) =>
        {
            updateAvailableString = string.Format("新しいバージョンがあります {0} / {1} / {2}", revision, versionName, code);
        };

        DeployGateSDK.Install();
    }

    private string textField = "";

    void OnGUI()
    {

        if (initialized)
        {
            DrawLabel("IsInitialized", DeployGateSDK.IsInitialized().ToString());
            DrawLabel("IsDeployGateAvailable", DeployGateSDK.IsDeployGateAvailable().ToString());
            DrawLabel("IsInitialized", DeployGateSDK.IsInitialized().ToString());
            DrawLabel("IsAuthorized", DeployGateSDK.IsAuthorized().ToString());
            DrawLabel("GetLoginUsername", DeployGateSDK.GetLoginUsername());
            DrawLabel("GetAuthorUsername", DeployGateSDK.GetAuthorUsername());

            GUILayout.Label("<size=24><b>SendLog</b></size>");
            textField = GUILayout.TextField(textField);
            GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));

            if (GUILayout.Button("<size=18><b>LogDebug</b></size>"))
            {
                DeployGateSDK.LogDebug(textField);
            }
            if (GUILayout.Button("<size=18><b>LogInfo</b></size>"))
            {
                DeployGateSDK.LogInfo(textField);
            }
            if (GUILayout.Button("<size=18><b>LogVerbose</b></size>"))
            {
                DeployGateSDK.LogVerbose(textField);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
            if (GUILayout.Button("<size=18><b><color=yellow>LogWarn</color></b></size>"))
            {
                DeployGateSDK.LogWarn(textField);
            }
            if (GUILayout.Button("<size=18><b><color=red>LogError</color></b></size>"))
            {
                DeployGateSDK.LogError(textField);
            }
            GUILayout.EndHorizontal();

            GUILayout.Label("<size=24><b>Refresh</b></size>");

            if (GUILayout.Button("<size=18><b>Refresh</b></size>"))
            {
                DeployGateSDK.Refresh();
            }

            GUILayout.Label(updateAvailableString);
        }
    }

    void DrawLabel(string name, string value)
    {
        GUILayout.Label(string.Format("<size=24><b>{0}</b>: <color=red>{1}</color></size>", name, value));
    }
#endif
}
