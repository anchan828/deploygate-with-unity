using UnityEngine;
using UnityEditor;
using System.IO;

namespace DeployGate
{
	public class Asset
	{
		public static void  Save <T> (T asset) where T : ScriptableObject
		{
			T _asset = LoadAsset<T> ();
			
			if (_asset == null)
				AssetDatabase.CreateAsset (asset, GetAssetPath<T> ());
			
			EditorUtility.SetDirty (asset);
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh ();
		}
		
		public static T Load<T> () where T : ScriptableObject
		{
			T asset = LoadAsset<T> ();
			if (asset == null) {
				asset = ScriptableObject.CreateInstance<T> ();
				Save<T> (asset);
			}
			return asset;
		}

		private static T LoadAsset<T> () where T : ScriptableObject
		{
			if (!Directory.Exists (DeployGateUtility.settingsFolderPath))
				Directory.CreateDirectory (DeployGateUtility.settingsFolderPath);
			string assetPath = GetAssetPath<T> ();
			return (T)AssetDatabase.LoadAssetAtPath (assetPath, typeof(T));
		}

		private static string GetAssetPath<T> () where T : ScriptableObject
		{
			return DeployGateUtility.settingsFolderPath + DeployGateUtility.SEPARATOR + typeof(T).Name + ".asset";
		}
	}
}
