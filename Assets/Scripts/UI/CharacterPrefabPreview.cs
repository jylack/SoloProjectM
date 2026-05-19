using UnityEngine;

public class CharacterPrefabPreview : MonoBehaviour
{
    [SerializeField] private Transform characterParent;
    [SerializeField] private string defaultPrefabPath = "SPUM_20250417115258389";
    [SerializeField] private PlayerState initialState = PlayerState.IDLE;
    [SerializeField] private int initialAnimationIndex = 0;
    [SerializeField] private bool initializeSpumAnimation = true;
    [SerializeField] private string previewLayerName = "PreviewCharacter";
    
    private SPUM_Prefabs spumPrefab;
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
    public void PlayIdle()
    {
        Play(PlayerState.IDLE, 0);
    }

    public void PlayMove()
    {
        Play(PlayerState.MOVE, 0);
    }

    public void Play(PlayerState state, int index)
    {
        if (spumPrefab == null)
        {
            Debug.LogWarning("[CharacterPrefabPreview] SPUM prefab is not ready.");
            return;
        }

        spumPrefab.PlayAnimation(state, index);
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

        SetLayerRecursively(currentCharacter, LayerMask.NameToLayer(previewLayerName));

        if (initializeSpumAnimation)
        {
            InitSpum(currentCharacter);
        }
    }

    private void InitSpum(GameObject characterObject)
    {        
        spumPrefab = characterObject.GetComponentInChildren<SPUM_Prefabs>(true);
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

    public void RefreshFromGameManager()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("[CharacterPrefabPreview] GameManager.Instance is null.");
            return;
        }

        PlayerProfileData profile = GameManager.Instance.PlayerProfile;

        if (profile == null)
        {
            Debug.LogError("[CharacterPrefabPreview] PlayerProfile is null.");
            return;
        }

        Apply(profile.appearance);
    }

    private void SetLayerRecursively(GameObject target, int layer)
{
    if (target == null)
    {
        return;
    }

    if (layer < 0)
    {
        Debug.LogWarning("[CharacterPrefabPreview] Preview layer not found.");
        return;
    }

    target.layer = layer;

    foreach (Transform child in target.transform)
    {
        SetLayerRecursively(child.gameObject, layer);
    }
}
}
