using UnityEngine;
using UnityEditor;
using DeployGate;

namespace DeployGate
{
    public class DeployGateWindowUtility
    {
        protected static void Headline(string text)
        {
            GUIStyle style = new GUIStyle(EditorStyles.label);
            style.stretchWidth = false;
            style.fontSize = 14;
            style.margin.top = 5;
            style.margin.bottom = 5;
            style.fontStyle = FontStyle.Bold;
            GUILayout.Label(text, style);
        }

        protected static void OnGUI_DeployGateInfo()
        {
            DrawDeployGateLogo();
        }

        private static Texture texture;

        protected static void DrawDeployGateLogo()
        {
            texture = texture ?? (Texture)AssetDatabase.LoadAssetAtPath(DeployGateUtility.imagesFolderPath + DeployGateUtility.SEPARATOR + "DeployGate_Logo.png", typeof(Texture));
            Graphics.DrawTexture(new Rect(13, Screen.height - 95, 100, 100), texture);
        }

        private static GUIStyle _sectionHeader;
        private static GUIStyle _sectionHeader_p;
        protected static GUIStyle sectionHeader
        {
            get
            {
                if (_sectionHeader == null)
                {
                    _sectionHeader = new GUIStyle(EditorStyles.largeLabel);
                    _sectionHeader.fontStyle = FontStyle.Bold;
                    _sectionHeader.fontSize = 18;
                    _sectionHeader.normal.textColor = new Color(0.4f, 0.4f, 0.4f, 1f);
                }
                if (_sectionHeader_p == null)
                {
                    _sectionHeader_p = new GUIStyle(EditorStyles.largeLabel);
                    _sectionHeader_p.fontStyle = FontStyle.Bold;
                    _sectionHeader_p.fontSize = 18;
                    _sectionHeader_p.normal.textColor = new Color(0.7f, 0.7f, 0.7f, 1f);
                }
                return EditorGUIUtility.isProSkin ? _sectionHeader_p : _sectionHeader;
            }
        }

        private static Texture2D _onNomalTexture;
        private static Texture2D _onNomalTexture_p;
        public static Texture2D onNomalTexture
        {
            get
            {
                if (_onNomalTexture == null)
                {
                    _onNomalTexture = new Texture2D(1, 1);
                    _onNomalTexture.SetPixel(0, 0,
                         new Color32(72, 129, 227, 255));
                    _onNomalTexture.Apply();
                }
                if (_onNomalTexture_p == null)
                {
                    _onNomalTexture_p = new Texture2D(1, 1);
                    _onNomalTexture_p.SetPixel(0, 0,
                         new Color32(66, 96, 147, 255));
                    _onNomalTexture_p.Apply();

                }
                return EditorGUIUtility.isProSkin ? _onNomalTexture_p : _onNomalTexture;
            }
        }

        private static Texture2D _backgroundTexture;

        public static Texture2D backgroundTexture
        {
            get
            {
                if (_backgroundTexture == null)
                {
                    _backgroundTexture = new Texture2D(1, 1);
                    _backgroundTexture.SetPixel(0, 0, new Color32(222, 222, 222, 255));
                }
                return _backgroundTexture;
            }
        }

        public enum DeployGateSelection
        {
            BuildUpload = 0,
            Members = 1,
            Setings = 2,
            Help = 3
        }
    }

}