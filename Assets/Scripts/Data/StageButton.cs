using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageButton : MonoBehaviour
{
    [SerializeField] private TMP_Text stageNameText;
    [SerializeField] private TMP_Text daysText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Button button;
    [SerializeField] private SceneName gameScene = SceneName.InGameScene;

    private StageDefinition _def;

    public void Initialize(StageDefinition def)
    {
        _def = def;
        stageNameText.text = def.stageName;
        daysText.text = $"{def.maxDays}일";
        
        if(def.backgroundSprite != null) 
            backgroundImage.sprite = def.backgroundSprite;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        // 1) 선택한 스테이지로 강제 설정
        StageManager.Instance.ForceStage(_def.stageId);

        // 2) GameManager를 통해 InGameScene 로드
        GameManager.Instance.SceneLoad(gameScene);
    }
}
