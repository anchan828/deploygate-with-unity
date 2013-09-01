using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using DeployGate;
using DeployGate.Resources;

namespace DeployGate
{
	public class DeployGateBuildPostprocessor
	{
		
		private static DeployGatePreference preference;
		
        [PostProcessBuild(9999)]
		public static void OnPostprocessBuild (BuildTarget target, string pathToBuiltProject)
		{
			preference = Asset.Load<DeployGatePreference> ();
			if (target == BuildTarget.Android && pathToBuiltProject.Contains (preference.temp.directryPath))
                AndroidPostprocessBuild(pathToBuiltProject);
		}

        private static void AndroidPostprocessBuild(string pathToBuiltProject)
		{
			EditorApplication.LockReloadAssemblies ();
			
			PlayerSettings.Android.forceInternetPermission = preference.forceInternetPermission;
		
			if (pathToBuiltProject.LastIndexOf (".apk") != -1) {
		
                DeplpyGateAPI.Push(pathToBuiltProject);
                
				//Delete Temp
				Directory.Delete (preference.temp.directryPath, true);
				DeployGateUtility.MoveDeployGateSDK (DeployGateUtility.PLUGINS_PATH, DeployGateUtility.DEPLOYGATE_PLUGINS_PATH);
				Asset.Load<DeployGatePreference> ().temp.messagePath = "";
			
			} else {
				// Android Project
			}
			EditorApplication.UnlockReloadAssemblies ();
		}

	}
}
