using System.Collections;
using System.Collections.Generic;
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

    public static GameManager instance;

    public Player PlayerState;

    public int currentDay = 1; // 현재 날짜

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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


    public void SceneLoad(SceneName sceneName)
    {
        SceneManager.LoadScene(sceneName.ToString());
    }
}
