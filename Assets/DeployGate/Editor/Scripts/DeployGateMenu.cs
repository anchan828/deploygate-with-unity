using System;
using UnityEngine;
using UnityEditor;
using DeployGate;
using DeployGate.Resources;

namespace DeployGate
{
    public class DeployGateMenu
    {
        [MenuItem("Window/DeployGate %1")]
        static void OpenDeployGatePreferenceWindow()
        {
            if (DeployGateUtility.showWelcomeWindow)
            {
                DeployGateWelcomeWindow.GetWindow();
            }
            else
            {
                DeployGateWindow.GetWindow();
            }
        }
    }
}

