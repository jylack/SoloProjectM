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

    private void Start()
    {
        RegisterUI.SetActive(false);
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

        RegisterUI.SetActive(true);
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

        string trimmedEmail = email == null ? string.Empty : email.Trim();
        string trimmedPassword = password == null ? string.Empty : password.Trim();

        if (string.IsNullOrEmpty(trimmedEmail) || string.IsNullOrEmpty(trimmedPassword))
        {
            warningText.text = "이메일/비밀번호를 입력해주세요.";
            yield break;
        }

        Task<AuthResult> loginTask = auth.SignInWithEmailAndPasswordAsync(trimmedEmail, trimmedPassword);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            warningText.text = ConvertLoginErrorMessage(loginTask.Exception);
            yield break;
        }

        user = loginTask.Result.User;
        bool profileValidated = false;
        string profileFailMessage = "프로필 검증 실패";

        yield return StartCoroutine(LoadAndValidateProfileCor(user, trimmedEmail, (ok, message) =>
        {
            profileValidated = ok;
            profileFailMessage = message;
        }));

        if (!profileValidated)
        {
            warningText.text = profileFailMessage;
            confirmText.text = "";
            auth.SignOut();
            user = null;
            yield break;
        }

        warningText.text = "";
        nickField.text = GetSafeNickname(user);
        confirmText.text = "로그인 완료, 반갑습니다 " + nickField.text + "님";

        GameManager.Instance.SceneLoad(SceneName.RoomScene);
    }

    private void TryLoadCurrentUserProfile()
    {
        if (auth == null || auth.CurrentUser == null)
        {
            return;
        }

        user = auth.CurrentUser;
        StartCoroutine(LoadAndValidateProfileCor(user, user.Email, null));
    }

    private IEnumerator LoadAndValidateProfileCor(FirebaseUser currentUser, string inputEmail, Action<bool, string> onCompleted)
    {
        if (_databaseRoot == null || currentUser == null)
        {
            onCompleted?.Invoke(false, "프로필 로드 준비가 완료되지 않았습니다.");
            yield break;
        }

        DataSnapshot snapshot = null;
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

            snapshot = profileTask.Result;
            break;
        }

        if (snapshot == null)
        {
            onCompleted?.Invoke(false, "프로필 정보를 불러오지 못했습니다.");
            yield break;
        }

        if (!snapshot.Exists)
        {
            onCompleted?.Invoke(false, "DB에 등록된 유저 데이터가 없습니다.");
            yield break;
        }

        string rawJson = snapshot.GetRawJsonValue();
        if (string.IsNullOrEmpty(rawJson) || rawJson == "null")
        {
            onCompleted?.Invoke(false, "DB 유저 데이터가 비어있습니다.");
            yield break;
        }

        PlayerProfileData profileData;
        try
        {
            profileData = JsonUtility.FromJson<PlayerProfileData>(rawJson);
        }
        catch (Exception ex)
        {
            Debug.LogWarning("프로필 데이터 역직렬화 실패 : " + ex);
            onCompleted?.Invoke(false, "DB 유저 데이터 형식이 올바르지 않습니다.");
            yield break;
        }

        if (profileData == null)
        {
            onCompleted?.Invoke(false, "DB 유저 데이터가 손상되었습니다.");
            yield break;
        }

        NormalizeProfile(profileData, currentUser);

        string authEmail = currentUser.Email == null ? string.Empty : currentUser.Email.Trim();
        string loginEmail = inputEmail == null ? string.Empty : inputEmail.Trim();

        if (!string.Equals(authEmail, loginEmail, StringComparison.OrdinalIgnoreCase))
        {
            onCompleted?.Invoke(false, "계정 이메일 검증에 실패했습니다.");
            yield break;
        }

        if (!string.IsNullOrEmpty(profileData.uid) && !string.Equals(profileData.uid, currentUser.UserId, StringComparison.Ordinal))
        {
            onCompleted?.Invoke(false, "DB 유저 정보와 인증 정보가 일치하지 않습니다.");
            yield break;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetPlayerProfile(profileData);
        }

        onCompleted?.Invoke(true, string.Empty);
    }

    private string ConvertLoginErrorMessage(AggregateException exception)
    {
        Debug.LogWarning("다음과 같은 이유로 로그인 실패:" + exception);

        FirebaseException firebaseEx = exception.GetBaseException() as FirebaseException;
        if (firebaseEx == null)
        {
            return "로그인 실패. 네트워크 상태를 확인해주세요.";
        }

        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
        switch (errorCode)
        {
            case AuthError.MissingEmail:
                return "이메일 누락";
            case AuthError.MissingPassword:
                return "패스워드 누락";
            case AuthError.WrongPassword:
                return "패스워드 틀림";
            case AuthError.InvalidEmail:
                return "이메일 형식이 옳지 않음";
            case AuthError.UserNotFound:
                return "아이디가 존재하지 않음";
            default:
                return "관리자에게 문의 바랍니다";
        }
    }

    private void NormalizeProfile(PlayerProfileData profileData, FirebaseUser currentUser)
    {
        if (string.IsNullOrEmpty(profileData.uid))
        {
            profileData.uid = currentUser.UserId;
        }

        if (string.IsNullOrEmpty(profileData.nickname))
        {
            profileData.nickname = GetSafeNickname(currentUser);
        }

        if (profileData.stats == null)
        {
            profileData.stats = PlayerStatsData.CreateDefault();
        }

        if (profileData.items == null)
        {
            profileData.items = new List<PlayerItemData>();
        }

        if (profileData.skills == null)
        {
            profileData.skills = new List<PlayerSkillData>();
        }
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
