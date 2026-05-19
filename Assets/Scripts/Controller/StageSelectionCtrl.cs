using System.Collections.Generic;
using UnityEngine;

public class StageSelectionCtrl : MonoBehaviour
{
    [SerializeField] private RectTransform stageButtonRoot;
    [SerializeField] private StageButton stageButtonPrefab;
    [SerializeField] private List<StageDefinition> stages;

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
        if (stageButtonRoot == null)
        {
            Debug.LogError("StageSelectionCtrl: Stage button root is not assigned.");
            return;
        }

        if (stageButtonPrefab == null)
        {
            Debug.LogError("StageSelectionCtrl: Stage button prefab is not assigned.");
            return;
        }

        ClearStageButtons();

        foreach (StageDefinition def in stages)
        {
            if (def == null)
            {
                continue;
            }

            StageButton button = Instantiate(stageButtonPrefab, stageButtonRoot);
            button.Initialize(def);
        }
    }

    private void ClearStageButtons()
    {
        for (int i = stageButtonRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(stageButtonRoot.GetChild(i).gameObject);
        }
    }
}