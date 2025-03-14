using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject confirmPanel;
    [SerializeField] private GameObject signinPanel;
    [SerializeField] private GameObject signupPanel;
    
    private GameUIController _gameUIController;
    private Canvas _canvas;
    
    private Constants.GameType _gameType;
    private GameLogic _gameLogic;

    private void Start()
    {
        // 로그인
        StartCoroutine(NetworkManager.Instance.GetScore((userInfo) =>
        {
            Debug.Log("자동 로그인 성공" + "\n아이디 : " + userInfo.username + "\n로그인 닉네임 : " + userInfo.nickname);
            OpenConfirmPanel(userInfo.nickname + "님 로그인에 성공하였습니다.", () => { });
        }, () =>
        {
            Debug.Log("자동 로그인 실패");
            OpenSigninPanel();
        }));
    }

    public void ChangeToGameScene(Constants.GameType gameType)
    {
        _gameType = gameType;
        SceneManager.LoadScene("Game");
    }

    public void ChangeToMainScene()
    {
        _gameLogic?.Dispose();
        _gameLogic = null;
        SceneManager.LoadScene("Main");
    }

    public void OpenLeaderboardPanel()
    {
        if (_canvas != null)
        {
            var leaderboardPanelObject = Instantiate(leaderboardPanel, _canvas.transform);

            StartCoroutine(NetworkManager.Instance.GetLeaderboard(ranks =>
            {
                foreach (var rank in ranks.scores)
                {
                    var leaderboardController = leaderboardPanelObject.GetComponent<LeaderboardPanelController>();
                    leaderboardController.CreateScoreCell(rank);
                }
            }, () =>
            {
                Debug.Log("랭킹 가져오기 실패");
            }));
            
            leaderboardPanelObject.GetComponent<PanelController>().Show();
        }
    }

    public void OpenSettingsPanel()
    {
        if (_canvas != null)
        {
            var settingsPanelObject = Instantiate(settingsPanel, _canvas.transform);
            settingsPanelObject.GetComponent<PanelController>().Show();
        }
    }

    public void OpenConfirmPanel(string message, ConfirmPanelController.OnConfirmButtonClick onConfirmButtonClick)
    {
        if (_canvas != null)
        {
            var confirmPanelObject = Instantiate(confirmPanel, _canvas.transform);
            confirmPanelObject.GetComponent<ConfirmPanelController>()
                .Show(message, onConfirmButtonClick);
        }
    }

    public void OpenSigninPanel()
    {
        if (_canvas != null)
        {
            var signinPanelObject = Instantiate(signinPanel, _canvas.transform);
        }
    }

    public void OpenSignupPanel()
    {
        if (_canvas != null)
        {
            var signupPanelObject = Instantiate(signupPanel, _canvas.transform);
        }
    }

    public void OpenGameOverPanel()
    {
        _gameUIController.SetGameUIMode(GameUIController.GameUIMode.GameOver);
    }
    
    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            // Scene에 배치된 Object 찾기 (BlockController, GameUIController)
            var blockController = GameObject.FindObjectOfType<BlockController>();
            _gameUIController = GameObject.FindObjectOfType<GameUIController>();
            
            // BlockController 초기화
            blockController.InitBlocks();
            
            // Game UI 초기화
            _gameUIController.SetGameUIMode(GameUIController.GameUIMode.Init);
            
            // Game Logic 객체 생성 --> 생성자 호출
            if (_gameLogic != null) _gameLogic.Dispose();
            _gameLogic = new GameLogic(blockController, _gameType);
        }

        // Canvas는 Main과 Game 모두 필요하기 때문에 if 밖에서 찾음
        _canvas = GameObject.FindObjectOfType<Canvas>();
    }

    private void OnApplicationQuit()
    {
        _gameLogic?.Dispose();
        _gameLogic = null;
    }
}