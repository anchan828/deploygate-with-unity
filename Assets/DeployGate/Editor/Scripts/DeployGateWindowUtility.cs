using UnityEngine;
using System.Collections;
using UnityEditor;
using DeployGate;

namespace DeployGate
{
	public class DeployGateWindowUtility
	{
		protected static void Headline (string text)
		{
			GUIStyle style = new GUIStyle (EditorStyles.label);
			style.stretchWidth = false;
			style.fontSize = 14;
			style.margin.top = 5;
			style.margin.bottom = 5;
			style.fontStyle = FontStyle.Bold;
			GUILayout.Label (text, style);
		}

		protected static void OnGUI_DeployGateInfo ()
		{
			DrawDeployGateLogo ();
		}
		
		private static Texture texture;

		protected  static void DrawDeployGateLogo ()
		{
				
			texture = texture ?? (Texture)AssetDatabase.LoadAssetAtPath (DeployGateUtility.imagesFolderPath + "DeployGate_Logo.png", typeof(Texture));
			
			Graphics.DrawTexture (new Rect (13, Screen.height - 95, 100, 100), texture);
		}

		protected static GUIStyle sectionHeader {
			get {
				GUIStyle style = new GUIStyle (EditorStyles.largeLabel);
				style.fontStyle = FontStyle.Bold;
				style.fontSize = 18;
				style.normal.textColor = EditorGUIUtility.isProSkin ? new Color (0.7f, 0.7f, 0.7f, 1f) : new Color (0.4f, 0.4f, 0.4f, 1f);
				return style;
			}
		}

		private static Texture2D _onNomalTexture;

		public static Texture2D onNomalTexture {
			get {
				_onNomalTexture = new Texture2D (1, 1);
				_onNomalTexture.SetPixel (0, 0, EditorGUIUtility.isProSkin ? new Color32 (66, 96, 147, 255) : new Color32 (72, 129, 227, 255));
				_onNomalTexture.Apply ();
				return _onNomalTexture;
			}
		}

		private static Texture2D _backgroundTexture;

		public static Texture2D backgroundTexture {
			get {
				if (_backgroundTexture == null) {
					_backgroundTexture = new Texture2D (1, 1);
					_backgroundTexture.SetPixel (0, 0, new Color32 (222, 222, 222, 255));
				}
				return	_backgroundTexture;
			}
		}
		
		public enum DeployGateSelection
		{
			BuildUpload = 0,
			Setings = 1,
			Help = 2
		}
	}

}