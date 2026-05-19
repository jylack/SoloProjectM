using UnityEngine;

public class CharacterPrefabPreview : MonoBehaviour
{
    [SerializeField] private Transform characterParent;
    [SerializeField] private string defaultPrefabPath = "SPUM/Resources/Units/SPUM_202504171";
    [SerializeField] private bool initializeSpumAnimation = true;

    private GameObject currentCharacter;

    public void Apply(PlayerAppearanceData appearance)
    {
        string prefabPath = GetPrefabPath(appearance);
        LoadPrefab(prefabPath);
    }

    public void Clear()
    {
        if (currentCharacter != null)
        {
            Destroy(currentCharacter);
            currentCharacter = null;
        }
    }

    private string GetPrefabPath(PlayerAppearanceData appearance)
    {
        if (appearance == null || string.IsNullOrEmpty(appearance.characterPrefabId))
        {
            return defaultPrefabPath;
        }

        return appearance.characterPrefabId;
    }

    private void LoadPrefab(string resourcesPath)
    {
        if (string.IsNullOrEmpty(resourcesPath))
        {
            Debug.LogError("[CharacterPrefabPreview] Prefab path is empty.");
            return;
        }

        Transform parent = characterParent != null ? characterParent : transform;

        Clear();

        GameObject prefab = Resources.Load<GameObject>(resourcesPath);
        if (prefab == null)
        {
            Debug.LogError("[CharacterPrefabPreview] Prefab not found: " + resourcesPath);
            return;
        }

        currentCharacter = Instantiate(prefab, parent);
        currentCharacter.transform.localPosition = Vector3.zero;
        currentCharacter.transform.localRotation = Quaternion.identity;
        currentCharacter.transform.localScale = Vector3.one;

        if (initializeSpumAnimation)
        {
            InitSpum(currentCharacter);
        }
    }

    private void InitSpum(GameObject characterObject)
    {
        SPUM_Prefabs spumPrefab = characterObject.GetComponentInChildren<SPUM_Prefabs>(true);
        if (spumPrefab == null)
        {
            return;
        }

        if (spumPrefab._anim == null)
        {
            spumPrefab._anim = spumPrefab.GetComponentInChildren<Animator>(true);
        }

        if (spumPrefab._anim == null)
        {
            Debug.LogWarning("[CharacterPrefabPreview] Animator not found in SPUM prefab.");
            return;
        }

        spumPrefab.PopulateAnimationLists();
        spumPrefab.OverrideControllerInit();
        spumPrefab.PlayAnimation(PlayerState.IDLE, 0);
    }
}
