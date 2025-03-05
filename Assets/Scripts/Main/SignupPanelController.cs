using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public struct SignupData
{
    public string username;
    public string nickname;
    public string password;
}

public class SignupPanelController : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField nicknameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TMP_InputField confirmPasswordInputField;
    
    private const string ServerURL = "http://localhost:3000";
    
    public void OnClickConfirmButton()
    {
        var username = usernameInputField.text;
        var nickname = nicknameInputField.text;
        var password = passwordInputField.text;
        var confirmPassword = confirmPasswordInputField.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(nickname) || 
            string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword)) 
        {
            // TODO: 입력값이 비어있음을 알리는 팝업창 표시
            return;
        }

        if (password.Equals(confirmPassword))
        {
            SignupData signupData = new SignupData();
            signupData.username = username;
            signupData.nickname = nickname;
            signupData.password = password;
            
            // 서버로 SignupData 전달하면서 회원가입 진행
            StartCoroutine(Signup(signupData));
        }
    }

    IEnumerator Signup(SignupData signupData)
    {
        string jsonString = JsonUtility.ToJson(signupData);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);

        // () 안의 객체가 {}를 벗어나면 Disposing되는 문법
        using (UnityWebRequest www =
               new UnityWebRequest(ServerURL + "/users/signup", UnityWebRequest.kHttpVerbPOST))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || 
                www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Error : " + www.error);

                if (www.responseCode == 409) // 중복 사용자
                {
                    // TODO: 중복 사용자 생성 팝업 표시
                    Debug.Log("중복 사용자");
                    GameManager.Instance.OpenConfirmPanel("이미 존재하는 사용자입니다.", () =>
                    {
                        usernameInputField.text = "";
                        nicknameInputField.text = "";
                        passwordInputField.text = "";
                        confirmPasswordInputField.text = "";
                    });
                }
            }
            else
            {
                var result = www.downloadHandler.text;
                Debug.Log("Result : " + result);
                
                // 회원가입 성공 팝업 표시
                GameManager.Instance.OpenConfirmPanel("회원가입이 완료 되었습니다.", () =>
                {
                    Destroy(gameObject);
                });
            }
        }
    }
    
    public void OnClickCancelButton()
    {
        Destroy(gameObject);
    }
}