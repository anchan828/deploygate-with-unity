using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

namespace DeployGate
{
		public class DeployGateBuildPostprocessor
		{
				[PostProcessBuild (9999)]
				public static void OnPostprocessBuild (BuildTarget target, string pathToBuiltProject)
				{
						if (target == BuildTarget.Android && pathToBuiltProject.Contains (Asset.preference.temp.directryPath))
								AndroidPostprocessBuild (pathToBuiltProject);
				}

				private static void AndroidPostprocessBuild (string pathToBuiltProject)
				{
						EditorApplication.LockReloadAssemblies ();
			
						PlayerSettings.Android.forceInternetPermission = Asset.preference.forceInternetPermission;
		
						if (pathToBuiltProject.LastIndexOf(".apk", System.StringComparison.Ordinal) != -1) {
		
								DeployGateApi.Push (pathToBuiltProject);
                
								//Delete Temp
								Directory.Delete (Asset.preference.temp.directryPath, true);
								Asset.Load<DeployGatePreference> ().temp.messagePath = "";
			
						} 
						EditorApplication.UnlockReloadAssemblies ();
				}
		}
}
