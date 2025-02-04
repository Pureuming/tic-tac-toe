using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private BlockController blockController;
    
    public enum PlayerType { None, PlayerA, PlayerB }
    private PlayerType[,] _board;

    private void Start()
    {
        // 게임 초기화
        InitGame();
        
        // 테스트
        blockController.OnBlockClickedDelegate = (row, col) =>
        {
            Debug.Log($"Row : {row}, Col : {col}");
        };
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
    }
}