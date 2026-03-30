using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomFlowController : MonoBehaviour
{
    [SerializeField] private int stageId = 1;
    [SerializeField] private int totalDays = 5;
    [SerializeField] private int seed = 0;

    public EncounterData NextEncounter => RunManager.Instance != null ? RunManager.Instance.CurrentEncounter : null;

    private void Start()
    {
        if (RunManager.Instance == null)
        {
            Debug.LogError("RoomFlowController: RunManager is missing.");
            return;
        }

        if (!RunManager.Instance.HasActiveRun)
        {
            int runSeed = seed == 0 ? System.Environment.TickCount : seed;
            RunManager.Instance.StartNewRun(stageId, totalDays, runSeed);
        }
        else
        {
            RunManager.Instance.UnlockEncounter();
        }
    }

    public void EnterEncounter()
    {
        if (RunManager.Instance == null || RunManager.Instance.CurrentEncounter == null)
        {
            return;
        }

        SceneManager.LoadScene("InGameScene");
    }

    public void RegenerateNextEncounter()
    {
        Debug.Log("RoomFlowController: RegenerateNextEncounter disabled in pre-generated mode.");
    }
}
