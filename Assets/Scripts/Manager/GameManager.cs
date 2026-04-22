using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
    TitleScene,
    RoomScene,
    InGameScene,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public Player PlayerState;
    public PlayerProfileData PlayerProfile { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Player GetPlayer()
    {
        return PlayerState;
    }

    public void SetPlayerProfile(PlayerProfileData profile)
    {
        if (profile == null)
        {
            return;
        }

        PlayerProfile = profile;
    }

    public void SceneLoad(SceneName sceneName)
    {
        SceneManager.LoadScene(sceneName.ToString());
    }
}
