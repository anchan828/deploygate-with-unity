using UnityEngine;

namespace DeployGate
{
	public class I18n
	{
		public  static string local{ get { return Words ("ja-JP", "en-US"); } }
		
		public static string profileError{ get{ return Words("UsernameまたはAPIKeyを入力してください","Please Enter Username or APIKey"); } }		
		public static string networkError{ get{ return Words("ネットワークに接続してください","Not connecting to network"); } }

		public static string Words (string jp, string en)
		{
			return Application.systemLanguage == SystemLanguage.Japanese ? jp : en;
		}
	}
}