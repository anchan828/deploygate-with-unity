# DeployGate with Unity v1.0

![](https://raw.github.com/anchan828/file-place/master/DeployGate/deploygate-icon.png)

##1.これは何？
Unityから[DeployGate](https://deploygate.com)へ楽にUpdate & SDKの使用ができるアセットです。

##2.インストール
`DeployGateSDK.unitypackage`があるのでUnityプロジェクトにインポートして下さい。

##3.ユーザー名とAPIキーの設定
Uploadするのに必要なユーザー名とAPIキーを設定します。
「Window」→「DeployGate」からDeployGateWindowを開き、**Settings** の`Your Name`と`API Key`を[DeployGateのアカウントページ](https://deploygate.com/settings)から取得し、設定して下さい。


![](https://raw.github.com/anchan828/file-place/master/DeployGate/Settings.png)

##4.リモートLogCatを有効にするには
DeploygateSDKの機能リモートLogCatを有効にするには **Settings** の「Include android.permission.READ_LOGS」にチェックを入れて下さい


##5.ビルド & アップロードを行う
**Build & Upload** の「Build & Upload」ボタンをおすことでDeploygateにアップロードすることができます。

![](https://raw.github.com/anchan828/file-place/master/DeployGate/BuildUpload.png)

###5-1.Identification
アプリのパージョンとバージョンコードを指定することができます。

###5-2.Message
Upload時に付与するメッセージを入力して下さい。必須ではありません。
前回のMessageがログとして保存されています。引用したい場合はコピーしてnew Messageにペーストして使用して下さい。


###5-3 BuildType
apkかEclipseProjectを選択して下さい。apkであればDeployGateにUploadします。



##6.DeployGateSDK.csの説明

**必須**　最初にDeployGateSDK.Install ()を行なって下さい。

```
void Start ()
{
	DeployGateSDK.Install ();
}
```

##7.APIの概要

|型|メソッド|説明|
|---|---|---|
|`static void`|Install()|DeployGateのインストール（セットアップ）を行います|
|`static void`|Install(string author)|DeployGateのインストール（セットアップ）を行います。管理者名を入力することでアプリの管理者のみがこのアプリを使用出来るようにします|
|`static bool`|IsDeployGateAvailable()|DeployGateクライアントがインストールされているかどうかを返します|
|`static bool`|IsAuthorized()|このアプリが現在のユーザーのアプリ一覧にあるかどうかを返します。IsDeployGateAvailableがtrueを返した時のみ動作します|
|`static string`|GetLoginUsername()|現在のログインユーザー名を返します。IsAuthorizedとIsDeployGateAvailableがtrueを返した時のみ動作します|
|`static string`|GetAuthorUsername()|アプリケーション提供者のユーザー名を返します|
|`static bool`|IsInitialized()|Installが行われた場合trueを返します|
|`static bool`|IsManaged()|このアプリがDeployGateで管理されている場合trueを返します|
|`static void`|Refresh()|キャシュされたDeployGateの情報をリフレッシュします|
|`static void`|LogVerbose(string text)|Verboseログを送信します|
|`static void`|LogDebug(string text)|Debugログを送信します|
|`static void`|LogInfo(string text)|Infoログを送信します|
|`static void`|LogWarn(string text)|Warnログを送信します|
|`static void`|LogError(string text)|Errorログを送信します|


##8.DeployGateSDK.csのみを使う方法
「Assets/DeployGate/Plugins/DeployGateSDK」にあるファイルを「Assets/Plugins」配下に移動させます。

###8.1 セットアップ
![](https://raw.github.com/anchan828/file-place/master/DeployGate/8.1.png)

|ファイル名|移動先|
|---|---|
|DeployGateSDK.cs|Assets/Plugins|
|deploygatesdk.jar|Assets/Plugins/Android|

###8.2 リモートLogCatを有効にする
「Assets/Plugins/Android」にAndroidManifest.xmlを作成し**android.permission.READ_LOGS**のPermissionを追加して下さい

#更新履歴

2013/3/29 v1.0 githubに公開

#License

```
Copyright (c) 2013 Keigo Ando

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
```