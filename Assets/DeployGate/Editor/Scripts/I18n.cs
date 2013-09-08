using System;
using UnityEngine;

namespace DeployGate
{
		public class I18n
		{
				public static GUIContent __temp { get { return Words (new GUIContent (""), new GUIContent ("")); } }

				public static GUIContent deployGate { get { return Words (new GUIContent ("DeployGate"), new GUIContent ("DeployGate")); } }

				public static GUIContent profileError { get { return Words ("UsernameまたはAPIKeyを入力してください", "Please Enter Username or APIKey"); } }

				public static GUIContent networkError { get { return Words ("ネットワークに接続してください", "Not connecting to network"); } }

				public static GUIContent upload { get { return Words (new GUIContent ("アップロード"), new GUIContent ("Uplaod")); } }

				public static GUIContent member { get { return Words (new GUIContent ("メンバー"), new GUIContent ("Members")); } }

				public static GUIContent settings { get { return Words (new GUIContent ("設定"), new GUIContent ("Settings")); } }

				public static GUIContent help { get { return Words (new GUIContent ("ヘルプ"), new GUIContent ("Help")); } }

				#region DeployGateWelcomeWindow

				public static GUIContent welcome { get { return Words (new GUIContent ("ようこそ"), new GUIContent ("Welcome")); } }

				public static GUIContent letsStart { get { return Words (new GUIContent ("さぁ始めよう"), new GUIContent ("Let's Start")); } }

				public static GUIContent skip { get { return Words (new GUIContent ("スキップ >>"), new GUIContent ("Skip >>")); } }

				public static GUIContent installSdk { get { return Words (new GUIContent ("インストール"), new GUIContent ("Install SDK")); } }

				public static GUIContent showApiKey { get { return Words (new GUIContent ("APIキーを表示"), new GUIContent ("Show APIKey")); } }

				public static GUIContent save { get { return Words (new GUIContent ("保存"), new GUIContent ("Save")); } }

				public static GUIContent getSdk { get { return Words (new GUIContent ("SDKを取得"), new GUIContent ("Get SDK")); } }

                public static GUIContent importSDK { get { return Words(new GUIContent("SDKをインポート"), new GUIContent("Import SDK")); } }
                
				public static GUIContent finish { get { return Words (new GUIContent ("Thank You"), new GUIContent ("Thank You")); } }

				public static GUIContent showDeployGateWindow { get { return Words (new GUIContent ("DeployGateウィンドウを表示する"), new GUIContent ("Show DeployGate Window")); } }

				public static GUIContent clickMenuItemWindowDeployGate { get { return Words (new GUIContent ("メニューから「Window/DeployGate」を選択してください"), new GUIContent ("Click MenuItem \" Window/DeployGate \"")); } }

				#endregion DeployGateWelcomeWindow

				#region DeployGateUploadWindow

				public static GUIContent identification { get { return Words (new GUIContent ("Identification"), new GUIContent ("Identification")); } }

				public static GUIContent message { get { return Words (new GUIContent ("メッセージ"), new GUIContent ("Message")); } }

				public static GUIContent buildType { get { return Words (new GUIContent ("ビルドタイプ"), new GUIContent ("BuildType")); } }

				public static GUIContent delete { get { return Words (new GUIContent ("削除"), new GUIContent ("Delete")); } }

				#endregion DeployGateUploadWindow

				#region DeployGateMembersWindow

				public static GUIContent developer { get { return Words (new GUIContent ("開発者"), new GUIContent ("Developer")); } }

				public static GUIContent tester { get { return Words (new GUIContent ("テスター"), new GUIContent ("Tester")); } }

				public static GUIContent refresh { get { return Words (new GUIContent ("更新"), new GUIContent ("Refresh")); } }

				public static GUIContent addMember { get { return Words (new GUIContent ("メンバーの追加"), new GUIContent ("Add Member")); } }

				public static GUIContent add { get { return Words (new GUIContent ("追加"), new GUIContent ("Add")); } }

				#endregion DeployGateMembersWindow

				#region DeployGatePreferenceWindow

				public static GUIContent account { get { return Words ("アカウント", "Account"); } }

				public static GUIContent username { get { return Words ("ユーザー名", "Username"); } }

				public static GUIContent apiKey { get { return Words (new GUIContent ("APIキー", "apkのアップロードやメンバーの取得に使用します"), new GUIContent ("APIKey")); } }

				public static GUIContent permissions { get { return Words ("パーミッション", "Permissions"); } }

				public static GUIContent language { get { return Words ("言語", "Language"); } }

				public static GUIContent permissions_readlog { get { return Words ("android.permission.READ_LOGS を含める", "Include android.permission.READ_LOGS"); } }

				#endregion DeployGatePreferenceWindow

				#region DeployGateHelpWindow

				public static GUIContent profile { get { return Words ("プロフィール", "Profile"); } }

				public static GUIContent howTo { get { return Words ("使い方", "How to"); } }

				public static GUIContent show { get { return Words ("表示する", "Show"); } }

				public static GUIContent welcomeWindow { get { return Words ("Welcome Window", "Welcome Window"); } }

				public static GUIContent goToPage { get { return Words ("ページヘ移動する", "Go to Page"); } }

				public static GUIContent howToUseDeployGate { get { return Words ("DeployGateの使い方", "How to use DeployGate"); } }

				public static GUIContent howToUseAssets { get { return Words ("このアセットの使い方", "How to use Assets"); } }

				#endregion DeployGateHelpWindow

				public static GUIContent Words (string jp, string en)
				{
						return Words (new GUIContent (jp), new GUIContent (en));
				}

				public static GUIContent Words (GUIContent jp, GUIContent en)
				{
						GUIContent word;
						switch (Asset.Load<DeployGatePreference> ().language) {
						case DeployGatePreference.Language.English:
								word = en;
								break;
						case DeployGatePreference.Language.Japanese:
								word = jp;
								break;
						default:
								throw new ArgumentOutOfRangeException ();
						}
						return word;
				}
		}
}