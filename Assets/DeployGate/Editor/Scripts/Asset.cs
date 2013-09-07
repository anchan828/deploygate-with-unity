using UnityEngine;
using UnityEditor;
using System.IO;

namespace DeployGate
{
		public class Asset
		{
				private static DeployGatePreference _preference;

				public static DeployGatePreference preference {
						get {
								if (_preference == null) {
										_preference = Load<DeployGatePreference> ();
								}
								return _preference;
						}
				}

				public static void Save<T> (T asset) where T : ScriptableObject
				{
						EditorUtility.SetDirty (asset);
						AssetDatabase.SaveAssets ();
						AssetDatabase.Refresh ();
				}

				public static T Load<T> () where T : ScriptableObject
				{
						var asset = LoadAsset<T> ();
						if (asset == null) {
								asset = ScriptableObject.CreateInstance<T> ();
								Save (asset);
						}
						return asset;
				}

				private static T LoadAsset<T> () where T : ScriptableObject
				{
						Directory.CreateDirectory (DeployGateUtility.settingsFolderPath);
						string assetPath = GetAssetPath<T> ();
						return (T)AssetDatabase.LoadAssetAtPath (assetPath, typeof(T));
				}

				private static string GetAssetPath<T> () where T : ScriptableObject
				{
						return DeployGateUtility.settingsFolderPath + DeployGateUtility.Separator + typeof(T).Name + ".asset";
				}
		}
}
