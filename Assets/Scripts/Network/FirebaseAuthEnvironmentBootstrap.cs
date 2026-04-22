using System;
using UnityEngine;

public static class FirebaseAuthEnvironmentBootstrap
{
    private const string UseAuthEmulatorKey = "USE_AUTH_EMULATOR";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void EnsureAuthEmulatorFlag()
    {
        string value = Environment.GetEnvironmentVariable(UseAuthEmulatorKey);
        if (!string.IsNullOrEmpty(value))
        {
            return;
        }

        Environment.SetEnvironmentVariable(UseAuthEmulatorKey, "0");
    }
}
