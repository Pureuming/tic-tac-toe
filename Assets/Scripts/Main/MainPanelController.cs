using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainPanelController : MonoBehaviour
{
    public void OnClickSinglePlayButton()
    {
        GameManager.Instance.ChangeToGameScene(Constants.GameType.SinglePlayer);
    }
    
    public void OnClickDualPlayButton()
    {
        GameManager.Instance.ChangeToGameScene(Constants.GameType.DualPlayer);
    }

    public void OnClickMultiplayButton()
    {
        GameManager.Instance.ChangeToGameScene(Constants.GameType.MultiPlayer);
    }
    
    public void OnClickSettingsButton()
    {
        GameManager.Instance.OpenSettingsPanel();
    }

    // public void OnClickScoreButton()
    // {
    //     StartCoroutine(NetworkManager.Instance.GetScore((userInfo) =>
    //     {
    //         Debug.Log(userInfo);
    //     }, () =>
    //     {
    //         // 로그인 화면 띄우기
    //     }));
    // }
}