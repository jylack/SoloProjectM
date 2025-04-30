using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum UIType
{
    MainMenu,
    InGameHUD,
    InGameVFX,
    PauseMenu,
    GameOver
}

[Serializable]
public class UIElement : MonoBehaviour
{
    public UIType type;
    public List<GameObject> uiE;

    public void View()
    {
        foreach (GameObject go in uiE)
        {
            if (go != null)
            {
                go.SetActive(true);
            }
        }
    }
}

public class UIManager : MonoBehaviour
{
    [Header("UI ¸®½ºÆ®")]
    [SerializeField]
    Dictionary<UIType,UIElement> uiDic;

    void Start()
    {

    }

    void Update()
    {

    }
}
