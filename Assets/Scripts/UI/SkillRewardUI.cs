using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public  class SkillRewardUI : MonoBehaviour
{
    public GameObject SkillRewardPanel; // 스킬 보상 패널
    public List< GameObject> SkillRewardItemPrefab = new List<GameObject>(3); // 스킬 보상 아이템 프리팹
    public Text SkillNameText; // 스킬 이름 텍스트
    public Text SkillDescriptionText; // 스킬 설명 텍스트
    public Image SkillIconImage; // 스킬 아이콘 이미지
    public Button ClaimButton; // 보상 수령 버튼
    private Reward reward; // 보상 데이터

    private void Start()
    {
        // 패널 비활성화
        SkillRewardPanel.SetActive(false);
    }

    public void Initialize(Reward reward)
    {
        this.reward = reward;
        SkillNameText.text = reward.name;
        SkillDescriptionText.text = reward.description;
        SkillIconImage.sprite = Resources.Load<Sprite>("SkillIcons/" + reward.name); // 스킬 아이콘 로드
        ClaimButton.onClick.AddListener(OnClaimButtonClicked);
    }
    private void OnClaimButtonClicked()
    {
        // 보상 수령 처리
        reward.isClaimed = true;
        SkillRewardPanel.SetActive(false);
    }

}