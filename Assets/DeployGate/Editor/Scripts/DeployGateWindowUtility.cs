using System.Linq;
using UnityEngine;
using UnityEditor;

namespace DeployGate
{
    public class DeployGateWindowUtility
    {
        protected static void Headline(string text)
        {
            Headline(new GUIContent(text));
        }

        protected static void Headline(GUIContent text)
        {
            GUIStyle style = new GUIStyle(EditorStyles.label);
            style.stretchWidth = false;
            style.fontSize = 14;
            style.margin.top = 5;
            style.margin.bottom = 5;
            style.fontStyle = FontStyle.Bold;
            GUILayout.Label(text, style);
        }
        private static GUISkin skin;

        public static GUIStyle GetStyle(string name)
        {
            if (!skin) LoadGUISkin();
            GUIStyle style = skin.FindStyle(name);
            if (EditorGUIUtility.isProSkin)
            {
                GUIStyle pStyle = skin.FindStyle(name + "_p");
                if (pStyle != null) style = pStyle;
            }
            return style ?? skin.label;
        }

        private static void LoadGUISkin()
        {
            string guiskinPath = AssetDatabase.GetAllAssetPaths().ToList().Find(path => path.EndsWith("DeployGateGUISkin.guiskin"));

            if (!string.IsNullOrEmpty(guiskinPath))
            {
                skin = AssetDatabase.LoadAssetAtPath(guiskinPath, typeof(GUISkin)) as GUISkin;
            }
        }
        protected static void OnGUI_DeployGateInfo()
        {
            DrawDeployGateLogo();
        }

        private static Texture texture;

        protected static void DrawDeployGateLogo()
        {
            texture = texture ?? (Texture)AssetDatabase.LoadAssetAtPath(DeployGateUtility.imagesFolderPath + DeployGateUtility.Separator + "DeployGate_Logo.png", typeof(Texture));
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

        public static void SetWindowSize(EditorWindow window)
        {
            int left = EditorPrefs.GetInt("UnityEditor.PreferencesWindowx", 96);
            int top = EditorPrefs.GetInt("UnityEditor.PreferencesWindowy", 271);
            int width = EditorPrefs.GetInt("UnityEditor.PreferencesWindoww", 500);
            int height = EditorPrefs.GetInt("UnityEditor.PreferencesWindowh", 400);
            window.position = new Rect(left, top, width, height);
            window.minSize = new Vector2(width, height);
            window.maxSize = window.minSize;
        }
    }

}