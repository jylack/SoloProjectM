using UnityEngine;
using UnityEngine.SceneManagement;

public static class RunFlowBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        EnsureRunManager();
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EnsureRunManager();

        if (scene.name == "RoomScene")
        {
            if (Object.FindObjectOfType<RoomFlowController>() == null)
            {
                var go = new GameObject("RoomFlowController");
                go.AddComponent<RoomFlowController>();
            }

            if (Object.FindObjectOfType<RoomSceneUI>() == null)
            {
                var go = new GameObject("RoomSceneUI");
                go.AddComponent<RoomSceneUI>();
            }
        }

        if (scene.name == "InGameScene")
        {
            if (Object.FindObjectOfType<InGameSceneController>() == null)
            {
                var go = new GameObject("InGameSceneController");
                go.AddComponent<InGameSceneController>();
            }
        }
    }

    private static void EnsureRunManager()
    {
        if (RunManager.Instance != null)
        {
            return;
        }

        var go = new GameObject("RunManager");
        go.AddComponent<RunManager>();
    }
}
