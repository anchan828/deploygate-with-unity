using UnityEngine;
using UnityEditor;
using DeployGate.Resources;

namespace DeployGate
{
    [System.Serializable]
    public class DeployGatePreference : ScriptableObject
    {
        public UserInfo user;
        public TempPath temp;
        public bool includeReadLog;
        public BuildType buildType = BuildType.APK;
        public bool forceInternetPermission = PlayerSettings.Android.forceInternetPermission;
        public Language language = Language.English;
        public enum BuildType
        {
            APK,
            EclipseProject
        }

        public enum Language
        {
            English, Japanese
        }
    }
}