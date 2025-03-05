﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class NetworkManager : Singleton<NetworkManager>
{
    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }
    
    public IEnumerator Signup(SignupData signupData, Action success, Action failure)
    {
        string jsonString = JsonUtility.ToJson(signupData);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);

        // () 안의 객체가 {}를 벗어나면 Disposing되는 문법
        using (UnityWebRequest www =
               new UnityWebRequest(Constants.ServerURL + "/users/signup", UnityWebRequest.kHttpVerbPOST))
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
                        failure?.Invoke();
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
                    success?.Invoke();
                });
            }
        }
    }
    
    public IEnumerator Signin(SigninData signinData, Action success, Action<int> failure)
    {
        string jsonString = JsonUtility.ToJson(signinData);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);

        using (UnityWebRequest www =
               new UnityWebRequest(Constants.ServerURL + "/users/signin", UnityWebRequest.kHttpVerbPOST))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError)
            {
                
            }
            else
            {
                var resultString = www.downloadHandler.text;
                var result = JsonUtility.FromJson<SigninResult>(resultString);

                if (result.result == 0)
                {
                    // username이 유효하지 않음
                    GameManager.Instance.OpenConfirmPanel("Username이 유효하지 않습니다.", () =>
                    {
                        failure?.Invoke(0);
                    });
                }
                else if (result.result == 1)
                {
                    // password가 유효하지 않음
                    GameManager.Instance.OpenConfirmPanel("Password가 유효하지 않습니다.", () =>
                    {
                        failure?.Invoke(1);
                    });
                }
                else if (result.result == 2)
                {
                    // 성공
                    GameManager.Instance.OpenConfirmPanel("로그인에 성공하였습니다.", () =>
                    {
                        success?.Invoke();
                    });
                }
            }
        }
    }
}