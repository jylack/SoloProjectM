
using System.Collections.Generic;
using UnityEngine;

public class StageSelectionCtrl : MonoBehaviour
{
    //private RectTransform contentRoot;     // ScrollView > Content
    [SerializeField] private RectTransform contentRoot;     // ScrollView > Content
    [SerializeField] private StageButton stageButtonPrefab;  // StageButton 프리팹
    [SerializeField] private List<StageDefinition> stages;  // 1스테이지, 2스테이지 SO 리스트

    //private void Awake()
    //{
    //    contentRoot = GetComponent<RectTransform>();
    //}

    private void Start()
    {

        if (StageManager.Instance != null)
        {
            StageManager.Instance.ConfigureStages(stages, true);
        }
        else
        {
            Debug.LogWarning("StageSelectionCtrl: StageManager instance is missing.");
        }

        PopulateStageList();
    }

    private void PopulateStageList()
    {
        if (contentRoot == null)
        {
            contentRoot = GetComponent<RectTransform>();
            Debug.Log("NOOOooo");

            if (contentRoot == null)
            {
                Debug.LogError("StageSelectionCtrl: Content root is not assigned.");
                return;
            }

        }

        if (stageButtonPrefab == null)
        {
            Debug.LogError("StageSelectionCtrl: Stage button prefab is not assigned.");
            return;
        }

        for (int i = contentRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(contentRoot.GetChild(i).gameObject);
        }

        foreach (var def in stages)
        {
            if (def == null)
                continue;

            var button = Instantiate(stageButtonPrefab, contentRoot);
            button.Initialize(def);
        }
    }
}
