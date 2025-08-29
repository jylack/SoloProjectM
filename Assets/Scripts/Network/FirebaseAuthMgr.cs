using Firebase;
using Firebase.Auth;
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

    public TMP_InputField emailField; //유저가 입력한 이메일
    public TMP_InputField pwField; //유저가 입력한 비밀번호
    public TMP_InputField nickField; //희원 가입시 입력할 닉네임

    public GameObject RegisterUI; //회원가입 UI

    public Text warningText;
    public Text confirmText;


    private void Awake()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK를 사용할 수 없습니다.
            }
        });

        LoginBtn.onClick.AddListener(() => { Login(); });
        RegisterBtn.onClick.AddListener(() => { Register(); });
        CreateIDBtn.onClick.AddListener(() => { CreateID(); });
    }

    private void Start()
    {
        RegisterUI.SetActive(false); //회원가입 UI 비활성화
        warningText.text = "";
        confirmText.text = "";
    }
    public void Login()
    {
        StartCoroutine(LoginCor(emailField.text, pwField.text));
    }

    public void Register()
    {
        RegisterUI.SetActive(true); //회원가입 UI 활성화
        RegisterUI.GetComponent<RegisterUI>().Setting(user, auth, warningText, confirmText);
    }

    public void CreateID()
    {
        //StartCoroutine(RegisterCor(emailField.text, pwField.text, nickField.text));
        RegisterUI.GetComponent<RegisterUI>().StartRegister();
    }

    private IEnumerator LoginCor(string email, string password)
    {
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

            GameManager.Instance.SceneLoad(SceneName.RoomScene);
        }
    }
    // 간단/안전한 이메일 형식 검사 (System.Net.Mail 사용)

    

    public void TestLogin()
    {

        GameManager.Instance.SceneLoad(SceneName.RoomScene);
    }
}
