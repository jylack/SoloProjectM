using Firebase;
using Firebase.Auth;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class FirebaseAuthMgr : MonoBehaviour
{
    public Button LoginBtn;    //�α��� ��ư
    public Button RegisterBtn; //�������UI ����
    public Button CreateIDBtn; //���̵� ���� ��ư

    public FirebaseUser user;  //������ ���� ����. �����߷� ġ�� ��ū���� ����
    public FirebaseAuth auth;  //���� ������ ���� ����

    public TMP_InputField emailField; //������ �Է��� �̸���
    public TMP_InputField pwField; //������ �Է��� ��й�ȣ
    public TMP_InputField nickField; //��� ���Խ� �Է��� �г���

    public GameObject RegisterUI; //ȸ������ UI

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
                // Firebase Unity SDK�� ����� �� �����ϴ�.
            }
        });

        LoginBtn.onClick.AddListener(() => { Login(); });
        RegisterBtn.onClick.AddListener(() => { Register(); });
        CreateIDBtn.onClick.AddListener(() => { CreateID(); });
    }

    private void Start()
    {
        RegisterUI.SetActive(false); //ȸ������ UI ��Ȱ��ȭ
        warningText.text = "";
        confirmText.text = "";
    }
    public void Login()
    {
        StartCoroutine(LoginCor(emailField.text, pwField.text));
    }

    public void Register()
    {
        RegisterUI.SetActive(true); //ȸ������ UI Ȱ��ȭ
    }

    public void CreateID()
    {
        StartCoroutine(RegisterCor(emailField.text, pwField.text, nickField.text));
    }

    private IEnumerator LoginCor(string email, string password)
    {
        Task<AuthResult> LoginTask = auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            Debug.LogWarning(message: "������ ���� ������ �α��� ����:" + LoginTask.Exception);

            //���̾�̽����� ������ �м��� �� �ִ� ������ ����
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "�̸��� ����";
                    break;
                case AuthError.MissingPassword:
                    message = "�н����� ����";
                    break;
                case AuthError.WrongPassword:
                    message = "�н����� Ʋ��";
                    break;
                case AuthError.InvalidEmail:
                    message = "�̸��� ������ ���� ����";
                    break;
                case AuthError.UserNotFound:
                    message = "���̵� �������� ����";
                    break;
                default:
                    message = "�����ڿ��� ���� �ٶ��ϴ�";
                    break;
            }
            warningText.text = message;
        }
        else// �׷��� �ʴٸ� �α���
        {
            user = LoginTask.Result.User; //���� ���� ���
            warningText.text = "";
            nickField.text = user.DisplayName;
            confirmText.text = "�α��� �Ϸ�, �ݰ����ϴ� " + user.DisplayName + "��";
            
            GameManager.instance.SceneLoad(SceneName.RoomScene);
        }
    }

    private IEnumerator RegisterCor(string email, string password, string username)
    {
        if (username == "")
        {
            warningText.text = "�г��� �̱���";
        }
        else
        {
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                Debug.LogWarning(message: "���� ����" + RegisterTask.Exception);
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "ȸ������ ����";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "�̸��� ����";
                        break;
                    case AuthError.MissingPassword:
                        message = "�н����� ����";
                        break;
                    case AuthError.WeakPassword:
                        message = "�н����� ����";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "�ߺ� �̸���";
                        break;
                    default:
                        message = "��Ÿ ����. ������ ���� �ٶ�";
                        break;
                }
                warningText.text = message;
            }
            else //���� �Ϸ�
            {
                user = RegisterTask.Result.User;

                if (user != null)
                {
                    UserProfile profile = new UserProfile { DisplayName = username };

                    //���̾�̽��� �г��� ���� �ø�
                    Task ProfileTask = user.UpdateUserProfileAsync(profile);
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        Debug.LogWarning(message: "�г��� ���� ����" + ProfileTask.Exception);
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningText.text = "�г��� ���� ����";
                    }
                    else
                    {
                        warningText.text = "";
                        confirmText.text = "���� �Ϸ�, �ݰ����ϴ� " + user.DisplayName + "��";
                        //StartBtn.interactable = true;
                        RegisterUI.SetActive(false); //ȸ������ UI ��Ȱ��ȭ
                    }
                }
            }
        }
    }
}