using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
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

    private bool _isFirebaseReady;


    private void Awake()
    {
        SetAuthButtonsInteractable(false);

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                // 1) Firebase 기본 인스턴스
                var app = FirebaseApp.DefaultInstance;

                // 2) 여기에 네 콘솔에서 본 URL 박기
                app.Options.DatabaseUrl = new Uri("https://soloprojm-default-rtdb.firebaseio.com/");

                // 3) 인증 인스턴스도 여기서 꺼내기
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

        // 버튼 리스너는 그대로
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
        //StartCoroutine(RegisterCor(emailField.text, pwField.text, nickField.text));
        RegisterUI.GetComponent<RegisterUI>().StartRegister();
    }

    private IEnumerator LoginCor(string email, string password)
    {
        if (auth == null)
        {
            warningText.text = "로그인 준비가 완료되지 않았습니다.";
            yield break;
        }

        Task<AuthResult> LoginTask = auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            Debug.LogWarning(message: "다음과 같은 이유로 로그인 실패:" + LoginTask.Exception);

            //파이어베이스에선 에러를 분석할 수 있는 형식을 제공
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "";
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
            warningText.text = message;
        }
        else// 그렇지 않다면 로그인
        {
            user = LoginTask.Result.User; //유저 정보 기억
            warningText.text = "";
            nickField.text = user.DisplayName;
            confirmText.text = "로그인 완료, 반갑습니다 " + user.DisplayName + "님";

            yield return StartCoroutine(LoadProfileCor(user));

            GameManager.Instance.SceneLoad(SceneName.RoomScene);
        }
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

        var profileTask = _databaseRoot.Child("users").Child(currentUser.UserId).GetValueAsync();
        yield return new WaitUntil(() => profileTask.IsCompleted);
        if (profileTask.Exception != null)
        {
            Debug.LogWarning("Realtime DB 로드 실패 : " + profileTask.Exception);
            yield break;
        }

        PlayerProfileData profileData = null;
        if (profileTask.Result.Exists)
        {
            try
            {
                profileData = JsonUtility.FromJson<PlayerProfileData>(profileTask.Result.GetRawJsonValue());
            }
            catch (Exception ex)
            {
                Debug.LogWarning("프로필 데이터 역직렬화 실패 : " + ex);
            }
        }

        if (profileData == null)
        {
            profileData = PlayerProfileData.CreateDefault(currentUser.UserId, currentUser.DisplayName);
            string json = JsonUtility.ToJson(profileData);
            var createTask = _databaseRoot.Child("users").Child(currentUser.UserId).SetRawJsonValueAsync(json);
            yield return new WaitUntil(() => createTask.IsCompleted);
            if (createTask.Exception != null)
            {
                Debug.LogWarning("Realtime DB 기본 데이터 저장 실패 : " + createTask.Exception);
            }
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetPlayerProfile(profileData);
        }
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
    // 간단/안전한 이메일 형식 검사 (System.Net.Mail 사용)

    

    public void TestLogin()
    {

        GameManager.Instance.SceneLoad(SceneName.RoomScene);
    }
}
