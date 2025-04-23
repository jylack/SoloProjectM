
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public SkillRewardUI SkillRewardUI;
    public SkillTooltipUI SkillTooltipUI;
    //public FairyEventUI FairyEventUI;
    //public DemonContractUI DemonContractUI;
    //public ANgel
    public StageFlowManager StageFlowManager;

    private void Awake()
    {
        
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        SkillRewardUI.gameObject.SetActive(false);
        SkillTooltipUI.gameObject.SetActive(false);
        //FairyEventUI.gameObject.SetActive(false);
        //DemonContractUI.gameObject.SetActive(false);
    }

    //public void ShowFairyEvent(StageInfoSO stage)
    //{
    //    fairyUI.gameObject.SetActive(true);
    //    fairyUI.Setup(stage);
    //}

    //public void ShowDemonContract(StageInfoSO stage)
    //{
    //    demonUI.gameObject.SetActive(true);
    //    demonUI.Setup(stage);
    //}

    //public void ShowAngelBlessing(StageInfoSO stage)
    //{
    //    angelUI.gameObject.SetActive(true);
    //    angelUI.Setup(stage);
    //}

    public void ShowReward(RewardCondition condition)
    {
        SkillRewardUI.gameObject.SetActive(true);
        //SkillRewardUI.Setup(condition);
    }





}