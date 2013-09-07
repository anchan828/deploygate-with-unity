using UnityEngine;
using System.Collections;
using System;

public class DeployGateSDK
{
#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG

    public delegate void DeployGateCallbackFunction(bool initialized);
    public delegate void DeployGateStatusChangedFunction(bool isManaged, bool isAuthorized, AndroidJavaObject loginUsername, bool isStopped);
    public delegate void DeployGateUpdateAvailableFunction(int revision, AndroidJavaObject versionName, int versionCode);

    public static DeployGateCallbackFunction initializedChanged;
    public static DeployGateStatusChangedFunction statusChanged;
    public static DeployGateUpdateAvailableFunction updateAvailableChanged;

    private static AndroidJavaClass deployGate = null;
    /// <summary>
    ///  Install DeployGate on your application instance.
    /// </summary>
    public static void Install()
    {
        Install(string.Empty);
    }

    /// <summary>
    ///  Install DeployGate on your application instance.
    /// </summary>
    public static void Install(string deployGateUserName)
    {

        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject app = activity.Call<AndroidJavaObject>("getApplicationContext");

        deployGate = new AndroidJavaClass("com.deploygate.sdk.DeployGate");

        activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {

            if (string.IsNullOrEmpty(deployGateUserName))
                deployGate.CallStatic("install", app, new DeployGateCallback(), true);
            else
                deployGate.CallStatic("install", app, deployGateUserName, new DeployGateCallback(), true);
        }));

    }

    /// <summary>
    ///  Get whether DeployGate client service is available on this device.
    /// </summary>
    public static bool IsDeployGateAvailable()
    {
        return deployGate.CallStatic<bool>("isDeployGateAvaliable");
    }

    /// <suIsInstalled = true;mmary>
    /// Get whether SDK is completed its intialization process and ready after install(Application).
    /// </summary>
    public static bool IsInitialized()
    {
        return deployGate.CallStatic<bool>("isInitialized");
    }

    /// <summary>
    ///  Get whether current DeployGate user has this application in his/her available list.
    /// </summary>
    public static bool IsAuthorized()
    {
        return deployGate.CallStatic<bool>("isAuthorized");
    }

    public static bool IsManaged()
    {
        return deployGate.CallStatic<bool>("isManaged");
    }

    /// <summary>
    ///  Get current DeployGate username. This function only available when isAuthorized() is true.
    /// </summary>
    public static string GetLoginUsername()
    {
        return deployGate.CallStatic<string>("getLoginUsername");
    }

    /// <summary>
    ///  Get current app's author (i.e. distributor) username on DeployGate. 
    /// </summary>
    public static string GetAuthorUsername()
    {
        return deployGate.CallStatic<string>("getAuthorUsername");
    }

    /// <summary>
    ///  Record ERROR level event on DeployGate.
    /// </summary>
    public static void LogError(string text)
    {
        deployGate.CallStatic("logError", text);
    }

    /// <summary>
    ///  Record WARN level event on DeployGate.
    /// </summary>
    public static void LogWarn(string text)
    {
        deployGate.CallStatic("logWarn", text);
    }

    /// <summary>
    ///  Record DEBUG level event on DeployGate.
    /// </summary>
    public static void LogDebug(string text)
    {
        deployGate.CallStatic("logDebug", text);
    }

    /// <summary>
    ///  Record INFO level event on DeployGate.
    /// </summary>
    public static void LogInfo(string text)
    {
        deployGate.CallStatic("logInfo", text);
    }

    /// <summary>
    ///  Record VERBOSE level event on DeployGate.
    /// </summary>
    public static void LogVerbose(string text)
    {
        deployGate.CallStatic("logVerbose", text);
    }

    /// <summary>
    /// Request refreshing cached session values (e.g., isAuthorized, etc.) to the DeployGate service.
    /// </summary>
    public static void Refresh()
    {
        deployGate.CallStatic("refresh");
    }

    class DeployGateCallback : AndroidJavaProxy
    {
        public DeployGateCallback() : base("com/deploygate/sdk/DeployGateCallback") { }

        void onInitialized(bool isServiceAvailable)
        {
            LogDebug("onStatusChanged");
            initializedChanged(isServiceAvailable);
        }

        void onStatusChanged(bool isManaged, bool isAuthorized, AndroidJavaObject loginUsername, bool isStopped)
        {
            LogDebug("onStatusChanged");
            statusChanged(isManaged, isAuthorized, loginUsername, isStopped);
        }

        void onUpdateAvailable(int revision, AndroidJavaObject versionName, int versionCode)
        {
            LogDebug("onUpdateAvailable");
            updateAvailableChanged(revision, versionName, versionCode);
        }

        int hashCode()
        {
            return GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
#else
    public static void Install ()
	{
	}

	public static void Install (string deployGateUserName)
	{
	}

	public static bool IsDeployGateAvailable ()
	{
		return false;
	}

	public static bool IsInitialized ()
	{
		return false;
	}

	public static bool IsAuthorized ()
	{
		return false;
	}
	
	public static bool IsManaged ()
	{
		return false;
	}
	
	public static string GetLoginUsername ()
	{
		return string.Empty;
	}

	public static string GetAuthorUsername ()
	{
		return string.Empty;
	}

	public static void LogError (string text)
	{
	}

	public static void LogWarn (string text)
	{
	}

	public static void LogDebug (string text)
	{
	}

	public static void LogInfo (string text)
	{
	}

	public static void LogVerbose (string text)
	{
	}

	public static void Refresh ()
	{
	}
#endif
}
