using System.Collections.Generic;
using UnityEngine;

public class StageSelectionCtrl : MonoBehaviour
{
    private RectTransform contentRoot;     // ScrollView > Content
    [SerializeField] private StageButton stageButtonPrefab;  // StageButton 프리팹
    [SerializeField] private List<StageDefinition> stages;  // 1스테이지, 2스테이지 SO 리스트

    private void Awake()
    {
        contentRoot = GetComponent<RectTransform>();    
    }

    private void Start()
    {
        stageButtonPrefab.Initialize(stages[0]);
        //PopulateStageList();
    }

    private void PopulateStageList()
    {
        foreach (var def in stages)
        {
            stageButtonPrefab.Initialize(def);
        }
    }
}
