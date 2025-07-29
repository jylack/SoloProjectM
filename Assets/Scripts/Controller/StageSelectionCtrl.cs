using System.Collections.Generic;
using UnityEngine;

public class StageSelectionCtrl : MonoBehaviour
{
    [SerializeField] private RectTransform contentRoot;     // ScrollView > Content
    [SerializeField] private GameObject stageButtonPrefab;  // StageButton 프리팹
    [SerializeField] private List<StageDefinition> stages;  // 1스테이지, 2스테이지 SO 리스트

    private void Start()
    {
        PopulateStageList();
    }

    private void PopulateStageList()
    {
        foreach (var def in stages)
        {
            var go = Instantiate(stageButtonPrefab, contentRoot);
            var btn = go.GetComponent<StageButton>();
            btn.Initialize(def);
        }
    }
}
