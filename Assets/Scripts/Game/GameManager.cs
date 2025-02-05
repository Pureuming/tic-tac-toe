using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private BlockController blockController;
    [SerializeField] private PanelManager panelManager;
    [SerializeField] private GameUIController gameUIController;

    public enum PlayerType { None, PlayerA, PlayerB }
    private PlayerType[,] _board;
    
    public enum TurnType { PlayerA, PlayerB }
    private enum GameResult { None, Win, Lose, Draw }

    private void Start()
    {
        // 게임 초기화
        InitGame();
    }

    /// <summary>
    /// 게임 초기화 함수 
    /// </summary>
    public void InitGame()
    {
        // _board 초기화
        _board = new PlayerType[3, 3];
        
        // Block 초기화
        blockController.InitBlocks();
        
        // StartPanel 표시
        panelManager.ShowPanel(PanelManager.PanelType.StartPanel);
        
        // Game UI 초기화
        gameUIController.SetGameUIMode(GameUIController.GameUIMode.Init);
    }

    /// <summary>
    /// 게임 시작
    /// </summary>
    public void StartGame()
    {
        panelManager.ShowPanel(PanelManager.PanelType.BattlePanel);
        SetTurn(TurnType.PlayerA);
    }
    
    /// <summary>
    /// 게임 오버 시, 호출되는 함수
    /// gameResult에 따라 결과 출력
    /// </summary>
    /// <param name="gameResult">win, lose, draw</param>
    private void EndGame(GameResult gameResult)
    {
        // 게임오버 표시
        gameUIController.SetGameUIMode(GameUIController.GameUIMode.GameOver);
        
        // TODO: 나중에 구현!
        switch (gameResult)
        {
            case GameResult.Win:
                break;
            case GameResult.Lose:
                break;
            case GameResult.Draw:
                break;
        }
    }

    /// <summary>
    /// _board에 새로운 값을 할당하는 함수
    /// </summary>
    /// <param name="playerType">할당하고자 하는 플레이어 타입</param>
    /// <param name="row">Row</param>
    /// <param name="col">Col</param>
    /// <returns>False : 할당할 수 없음, True : 할당이 완료됨</returns>
    private bool SetNewBoardValue(PlayerType playerType, int row, int col)
    {
        if (playerType == PlayerType.PlayerA)
        {
            _board[row, col] = playerType;
            blockController.PlaceMarker(Block.MarkerType.O, row, col);
            return true;
        }
        else if (playerType == PlayerType.PlayerB)
        {
            _board[row, col] = playerType;
            blockController.PlaceMarker(Block.MarkerType.X, row, col);
            return true;
        }
        return false;
    }

    private void SetTurn(TurnType turnType)
    {
        switch (turnType)
        {
            case TurnType.PlayerA:
                gameUIController.SetGameUIMode(GameUIController.GameUIMode.TurnA);
                blockController.OnBlockClickedDelegate = (row, col) =>
                {
                    if (SetNewBoardValue(PlayerType.PlayerA, row, col))
                    {
                        var gameResult = CheckGameResult();
                        if (gameResult == GameResult.None)
                            SetTurn(TurnType.PlayerB);
                        else
                            EndGame(gameResult);
                    }
                    else
                    {
                        // TODO: 이미 있는 곳을 터치 했을 때 처리
                    }
                };
                break;
            case TurnType.PlayerB:
                gameUIController.SetGameUIMode(GameUIController.GameUIMode.TurnB);
                blockController.OnBlockClickedDelegate = (row, col) =>
                {
                    if (SetNewBoardValue(PlayerType.PlayerB, row, col))
                    {
                        var gameResult = CheckGameResult();
                        if (gameResult == GameResult.None)
                            SetTurn(TurnType.PlayerA);
                        else
                            EndGame(gameResult);
                    }
                    else
                    {
                        // TODO: 이미 있는 곳을 터치 했을 때 처리
                    }
                };
                //TODO: AI에게 입력 받기
                
                break;
        }
    }

    /// <summary>
    /// 게임 결과 확인 함수
    /// </summary>
    /// <returns>플레이어 기준 게임 결과</returns>
    private GameResult CheckGameResult()
    {
        if (CheckGameWin(PlayerType.PlayerA)) return GameResult.Win;
        if (CheckGameWin(PlayerType.PlayerB)) return GameResult.Lose;
        if (IsAllBlocksPlaced()) return GameResult.Draw;
        
        return GameResult.None;
    }
    
    // 모든 마커가 보드에 배치 되었는지 확인하는 함수
    private bool IsAllBlocksPlaced()
    {
        for (var row = 0; row < _board.GetLength(0); row++)
        {
            for (var col = 0; col < _board.GetLength(1); col++)
            {
                if (_board[row, col] == PlayerType.None)
                    return false;
            }
        }
        return true;
    }
    
    // 게임의 승패를 판단하는 함수
    private bool CheckGameWin(PlayerType playerType)
    {
        // 가로로 마커가 일치하는지 확인 
        for (var row = 0; row < _board.GetLength(0); row++)
        {
            if (_board[row, 0] == playerType && _board[row, 1] == playerType && _board[row, 2] == playerType)
            {
                (int, int)[] blocks = { (row, 0), (row, 1), (row, 2) };
                blockController.SetBlockColor(playerType, blocks);
                return true;
            }
        }
        
        // 세로로 마커가 일치하는지 확인
        for (var col = 0; col < _board.GetLength(1); col++)
        {
            if (_board[0, col] == playerType && _board[1, col] == playerType && _board[2, col] == playerType)
            {
                (int, int)[] blocks = { (0, col), (1, col), (2, col) };
                blockController.SetBlockColor(playerType, blocks);
                return true;
            }
        }
        
        // 대각선으로 마커가 일치하는지 확인
        if (_board[0, 0] == playerType && _board[1, 1] == playerType && _board[2, 2] == playerType)
        {
            (int, int)[] blocks = { (0, 0), (1, 1), (2, 2) };
            blockController.SetBlockColor(playerType, blocks);
            return true;
        }

        if (_board[0, 2] == playerType && _board[1, 1] == playerType && _board[2, 0] == playerType)
        {
            (int, int)[] blocks = { (0, 2), (1, 1), (2, 0) };
            blockController.SetBlockColor(playerType, blocks);
            return true;
        }
        
        return false;
    }
}