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
    public static GameManager Instance { get; private set; }

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
            Debug.LogError("[GameManager] SetPlayerProfile 실패: profile이 null입니다.");
            return;
        }

        PlayerProfile = profile;

        Debug.Log($"[GameManager] PlayerProfile 저장 완료 / uid={profile.uid}, nickname={profile.nickname}");
    }
    public void SceneLoad(SceneName sceneName)
    {
        SceneManager.LoadScene(sceneName.ToString());
    }
}
