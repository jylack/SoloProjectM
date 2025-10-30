using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterUI : MonoBehaviour
{
    public TMP_InputField emailField; //유저가 입력한 이메일
    public TMP_InputField pwField; //유저가 입력한 비밀번호
    public TMP_InputField nickField; //희원 가입시 입력할 닉네임

    Text warningText;
    Text confirmText;


    FirebaseUser _user;  //인증된 유저 정보. 웹개발로 치면 토큰같은 느낌
    FirebaseAuth _auth;  //인증 진행을 위한 정보
    DatabaseReference _databaseRoot; // 실시간 DB 루트 참조

    public void Setting(FirebaseUser user, FirebaseAuth auth, Text waring, Text confirm, DatabaseReference databaseRoot)
    {
        _user = user;
        _auth = auth;
        warningText = waring;
        confirmText = confirm;
        _databaseRoot = databaseRoot;
    }

    public void StartRegister()
    {

        StartCoroutine(RegisterCor(emailField.text, pwField.text, nickField.text));
    }
    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
    private IEnumerator RegisterCor(string email, string password, string username)
    {
        email = email.Trim();
        password = password.Trim();
        username = username.Trim();

        if (string.IsNullOrEmpty(username))
        {
            warningText.text = "닉네임 미기입";
            yield break;
        }
        if (string.IsNullOrEmpty(email))
        {
            warningText.text = "이메일 미기입";
            yield break;
        }
        if (!IsValidEmail(email))
        {
            warningText.text = "이메일 형식 오류";
            yield break;
        }
        if (string.IsNullOrEmpty(password))
        {
            warningText.text = "패스워드 미기입";
            yield break;
        }
        if (password.Length < 6)
        {
            warningText.text = "패스워드 6자리 이상이어야 합니다.";
            yield break;
        }

        var registerTask = _auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.Exception != null)
        {
            Debug.LogWarning("실패 사유 : " + registerTask.Exception);

            var baseEx = registerTask.Exception.GetBaseException();
            var firebaseEx = baseEx as FirebaseException;

            string message = "회원가입 실패";
            if (firebaseEx != null)
            {
                var errorCode = (AuthError)firebaseEx.ErrorCode;
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "이메일 누락";
                        break;
                    case AuthError.MissingPassword:
                        message = "패스워드 누락";
                        break;
                    case AuthError.WeakPassword:
                        message = "패스워드 약함";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "중복 이메일";
                        break;
                    case AuthError.InvalidEmail:
                        message = "이메일 형식이 옳지 않음";
                        break;
                    default:
                        message = "기타 사유. 관리자 문의 바람";
                        break;
                }
            }
            else
            {
                message = "희원가입 실패 : " + baseEx.Message;
            }

            warningText.text = message;
            yield break;
        }

        _user = registerTask.Result.User;
        if (_user != null)
        {
            var profile = new UserProfile { DisplayName = username };
            var profileTask = _user.UpdateUserProfileAsync(profile);
            yield return new WaitUntil(() => profileTask.IsCompleted);
            if (profileTask.Exception != null)
            {
                Debug.LogWarning("닉네임 설정 실패 : " + profileTask.Exception);
                warningText.text = "닉네임 설정 실패";
                yield break;
            }

            if (_databaseRoot != null)
            {
                var defaultProfile = PlayerProfileData.CreateDefault(_user.UserId, username);
                string json = JsonUtility.ToJson(defaultProfile);
                var dbTask = _databaseRoot.Child("users").Child(_user.UserId).SetRawJsonValueAsync(json);
                yield return new WaitUntil(() => dbTask.IsCompleted);
                if (dbTask.Exception != null)
                {
                    Debug.LogWarning("Realtime DB 저장 실패 : " + dbTask.Exception);
                    warningText.text = "데이터 저장 실패";
                    yield break;
                }
                else if (GameManager.Instance != null)
                {
                    GameManager.Instance.SetPlayerProfile(defaultProfile);
                }
            }
            else
            {
                Debug.LogWarning("RegisterUI - Database reference is null. 기본 프로필을 저장하지 못했습니다.");
            }

            warningText.text = "";
            confirmText.text = "생성 완료, 반갑습니다 " + _user.DisplayName + "님";
            gameObject.SetActive(false); //회원가입 UI 비활성화
        }

    }
}
