using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseAuthMgr : MonoBehaviour
{
    public Button LoginBtn;    //로그인 버튼
    public Button RegisterBtn; //희원가입UI 오픈
    public Button CreateIDBtn; //아이디 생성 버튼

    public FirebaseUser user;  //인증된 유저 정보. 웹개발로 치면 토큰같은 느낌
    public FirebaseAuth auth;  //인증 진행을 위한 정보
    private DatabaseReference _databaseRoot; // 실시간 DB 루트

    public TMP_InputField emailField; //유저가 입력한 이메일
    public TMP_InputField pwField; //유저가 입력한 비밀번호
    public TMP_InputField nickField; //희원 가입시 입력할 닉네임

    public GameObject RegisterUI; //회원가입 UI

    public Text warningText;
    public Text confirmText;

    [Header("Auth Emulator (optional)")]
    [SerializeField] private bool useAuthEmulator;
    [SerializeField] private string authEmulatorHost = "127.0.0.1";
    [SerializeField] private int authEmulatorPort = 9099;

    [Header("Profile Load")]
    [SerializeField] private int profileLoadRetryCount = 2;
    [SerializeField] private float profileRetryDelaySeconds = 0.25f;

    private bool _isFirebaseReady;

    private void Awake()
    {
        SetAuthButtonsInteractable(false);

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                var app = FirebaseApp.DefaultInstance;
                app.Options.DatabaseUrl = new Uri("https://soloprojm-default-rtdb.firebaseio.com/");

                auth = FirebaseAuth.DefaultInstance;
                ConfigureAuthEmulator();

                _databaseRoot = FirebaseDatabase.GetInstance(app).RootReference;
                _isFirebaseReady = true;
                SetAuthButtonsInteractable(true);

                Debug.Log("[FirebaseAuthMgr] Firebase init + DB URL set");
                TryLoadCurrentUserProfile();
            }
            else
            {
                Debug.LogError($"[FirebaseAuthMgr] Firebase deps error: {dependencyStatus}");
                warningText.text = "Firebase 초기화 실패";
            }
        });

        LoginBtn.onClick.AddListener(() => { Login(); });
        RegisterBtn.onClick.AddListener(() => { Register(); });
        CreateIDBtn.onClick.AddListener(() => { CreateID(); });
    }

    private void ConfigureAuthEmulator()
    {
        if (auth == null)
        {
            return;
        }

        string useEmulatorEnv = Environment.GetEnvironmentVariable("USE_AUTH_EMULATOR");
        bool hasEnvOptIn = useEmulatorEnv == "1" || string.Equals(useEmulatorEnv, "true", StringComparison.OrdinalIgnoreCase);

        if (!useAuthEmulator && !hasEnvOptIn)
        {
            return;
        }

        auth.UseEmulator(authEmulatorHost, authEmulatorPort);
        Debug.Log($"[FirebaseAuthMgr] Auth emulator enabled: {authEmulatorHost}:{authEmulatorPort}");
    }

    private void Start()
    {
        RegisterUI.SetActive(false); //회원가입 UI 비활성화
        warningText.text = "";
        confirmText.text = "";
    }

    public void Login()
    {
        if (!_isFirebaseReady || auth == null)
        {
            warningText.text = "Firebase 초기화 중입니다. 잠시 후 다시 시도해주세요.";
            return;
        }

        StartCoroutine(LoginCor(emailField.text, pwField.text));
    }

    public void Register()
    {
        if (!_isFirebaseReady || auth == null)
        {
            warningText.text = "Firebase 초기화 중입니다. 잠시 후 다시 시도해주세요.";
            return;
        }

        RegisterUI.SetActive(true); //회원가입 UI 활성화
        RegisterUI.GetComponent<RegisterUI>().Setting(user, auth, warningText, confirmText, _databaseRoot);
    }

    public void CreateID()
    {
        RegisterUI.GetComponent<RegisterUI>().StartRegister();
    }

    private IEnumerator LoginCor(string email, string password)
    {
        if (auth == null)
        {
            warningText.text = "로그인 준비가 완료되지 않았습니다.";
            yield break;
        }

        Task<AuthResult> loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            Debug.LogWarning("다음과 같은 이유로 로그인 실패:" + loginTask.Exception);

            FirebaseException firebaseEx = loginTask.Exception.GetBaseException() as FirebaseException;
            string message;

            if (firebaseEx == null)
            {
                message = "로그인 실패. 네트워크 상태를 확인해주세요.";
            }
            else
            {
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "이메일 누락";
                        break;
                    case AuthError.MissingPassword:
                        message = "패스워드 누락";
                        break;
                    case AuthError.WrongPassword:
                        message = "패스워드 틀림";
                        break;
                    case AuthError.InvalidEmail:
                        message = "이메일 형식이 옳지 않음";
                        break;
                    case AuthError.UserNotFound:
                        message = "아이디가 존재하지 않음";
                        break;
                    default:
                        message = "관리자에게 문의 바랍니다";
                        break;
                }
            }

            warningText.text = message;
            yield break;
        }

        user = loginTask.Result.User;
        warningText.text = "";
        nickField.text = GetSafeNickname(user);
        confirmText.text = "로그인 완료, 반갑습니다 " + nickField.text + "님";

        yield return StartCoroutine(LoadProfileCor(user));

        GameManager.Instance.SceneLoad(SceneName.RoomScene);
    }

    private void TryLoadCurrentUserProfile()
    {
        if (auth == null || auth.CurrentUser == null)
        {
            return;
        }

        user = auth.CurrentUser;
        StartCoroutine(LoadProfileCor(user));
    }

    private IEnumerator LoadProfileCor(FirebaseUser currentUser)
    {
        if (_databaseRoot == null || currentUser == null)
        {
            yield break;
        }

        PlayerProfileData profileData = null;
        int maxAttempt = Mathf.Max(1, profileLoadRetryCount);

        for (int attempt = 1; attempt <= maxAttempt; attempt++)
        {
            var profileTask = _databaseRoot.Child("users").Child(currentUser.UserId).GetValueAsync();
            yield return new WaitUntil(() => profileTask.IsCompleted);

            if (profileTask.Exception != null)
            {
                Debug.LogWarning($"Realtime DB 로드 실패 (시도 {attempt}/{maxAttempt}) : {profileTask.Exception}");
                if (attempt < maxAttempt)
                {
                    yield return new WaitForSeconds(profileRetryDelaySeconds);
                }
                continue;
            }

            if (!profileTask.Result.Exists)
            {
                break;
            }

            string rawJson = profileTask.Result.GetRawJsonValue();
            if (string.IsNullOrEmpty(rawJson) || rawJson == "null")
            {
                Debug.LogWarning("프로필 JSON이 비어있습니다. 기본값으로 복구합니다.");
                break;
            }

            try
            {
                profileData = JsonUtility.FromJson<PlayerProfileData>(rawJson);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("프로필 데이터 역직렬화 실패 : " + ex);
            }

            break;
        }

        if (profileData == null)
        {
            profileData = PlayerProfileData.CreateDefault(currentUser.UserId, GetSafeNickname(currentUser));
            string json = JsonUtility.ToJson(profileData);
            var createTask = _databaseRoot.Child("users").Child(currentUser.UserId).SetRawJsonValueAsync(json);
            yield return new WaitUntil(() => createTask.IsCompleted);
            if (createTask.Exception != null)
            {
                Debug.LogWarning("Realtime DB 기본 데이터 저장 실패 : " + createTask.Exception);
            }
        }
        else
        {
            bool wasUpdated;
            profileData = NormalizeProfile(profileData, currentUser, out wasUpdated);
            if (wasUpdated)
            {
                string normalizedJson = JsonUtility.ToJson(profileData);
                var updateTask = _databaseRoot.Child("users").Child(currentUser.UserId).SetRawJsonValueAsync(normalizedJson);
                yield return new WaitUntil(() => updateTask.IsCompleted);
                if (updateTask.Exception != null)
                {
                    Debug.LogWarning("프로필 정규화 데이터 저장 실패 : " + updateTask.Exception);
                }
            }
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetPlayerProfile(profileData);
        }
    }

    private PlayerProfileData NormalizeProfile(PlayerProfileData profileData, FirebaseUser currentUser, out bool wasUpdated)
    {
        wasUpdated = false;

        if (profileData == null)
        {
            wasUpdated = true;
            return PlayerProfileData.CreateDefault(currentUser.UserId, GetSafeNickname(currentUser));
        }

        if (string.IsNullOrEmpty(profileData.uid))
        {
            profileData.uid = currentUser.UserId;
            wasUpdated = true;
        }

        if (string.IsNullOrEmpty(profileData.nickname))
        {
            profileData.nickname = GetSafeNickname(currentUser);
            wasUpdated = true;
        }

        if (profileData.stats == null)
        {
            profileData.stats = PlayerStatsData.CreateDefault();
            wasUpdated = true;
        }

        if (profileData.items == null)
        {
            profileData.items = new List<PlayerItemData>();
            wasUpdated = true;
        }

        if (profileData.skills == null)
        {
            profileData.skills = new List<PlayerSkillData>();
            wasUpdated = true;
        }

        return profileData;
    }

    private string GetSafeNickname(FirebaseUser currentUser)
    {
        if (currentUser == null)
        {
            return "Player";
        }

        if (!string.IsNullOrEmpty(currentUser.DisplayName))
        {
            return currentUser.DisplayName;
        }

        if (!string.IsNullOrEmpty(currentUser.Email))
        {
            int atIndex = currentUser.Email.IndexOf('@');
            if (atIndex > 0)
            {
                return currentUser.Email.Substring(0, atIndex);
            }

            return currentUser.Email;
        }

        return "Player";
    }

    private void SetAuthButtonsInteractable(bool interactable)
    {
        if (LoginBtn != null)
        {
            LoginBtn.interactable = interactable;
        }

        if (RegisterBtn != null)
        {
            RegisterBtn.interactable = interactable;
        }

        if (CreateIDBtn != null)
        {
            CreateIDBtn.interactable = interactable;
        }
    }

    public void TestLogin()
    {
        GameManager.Instance.SceneLoad(SceneName.RoomScene);
    }
}
