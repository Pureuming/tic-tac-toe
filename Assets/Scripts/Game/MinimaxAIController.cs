using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MinimaxAIController
{
    public static (int row, int col)? GetBestMove(GameManager.PlayerType[,] board)
    {
        for (var row = 0; row < board.GetLength(0); row++)
        {
            for (var col = 0; col < board.GetLength(1); col++)
            {
                if (board[row, col] == GameManager.PlayerType.None)
                {
                    board[row, col] = GameManager.PlayerType.PlayerB;
                    var score = DoMinimax(board, 0, false);
                    board[row, col] = GameManager.PlayerType.None;
                }
            }
        }

        return null;
    }
    
    private static float DoMinimax(GameManager.PlayerType[,] board, int depth, bool isAITurn)
    {
        if (CheckGameWin(GameManager.PlayerType.PlayerA, board))
            return -10 + depth;
        if (CheckGameWin(GameManager.PlayerType.PlayerB, board))
            return 10 - depth;
        if (IsAllBlocksPlaced(board))
            return 0;

        if (isAITurn)
        {
            for (var row = 0; row < board.GetLength(0); row++)
            {
                for (var col = 0; col < board.GetLength(1); col++)
                {
                    if (board[row, col] == GameManager.PlayerType.None)
                    {
                        board[row, col] = GameManager.PlayerType.PlayerB;
                        DoMinimax(board, depth + 1, false); // 재귀함수
                        board[row, col] = GameManager.PlayerType.None;
                    }
                }
            }
        }
        else
        {
            for (var row = 0; row < board.GetLength(0); row++)
            {
                for (var col = 0; col < board.GetLength(1); col++)
                {
                    if (board[row, col] == GameManager.PlayerType.None)
                    {
                        board[row, col] = GameManager.PlayerType.PlayerA;
                        DoMinimax(board, depth + 1, true); // 재귀함수
                        board[row, col] = GameManager.PlayerType.None;
                    }
                }
            }
        }
        
        return 0; // 임시 코드
    }
    
    /// <summary>
    /// 모든 마커가 보드에 배치 되었는지 확인하는 함수
    /// </summary>
    /// <returns>True : 모두 배치</returns>
    public static bool IsAllBlocksPlaced(GameManager.PlayerType[,] board)
    {
        for (var row = 0; row < board.GetLength(0); row++)
        {
            for (var col = 0; col < board.GetLength(1); col++)
            {
                if (board[row, col] == GameManager.PlayerType.None)
                    return false;
            }
        }
        return true;
    }
    
    /// <summary>
    /// 게임의 승패를 판단하는 함수
    /// </summary>
    /// <param name="playerType"></param>
    /// <param name="board"></param>
    /// <returns></returns>
    private static bool CheckGameWin(GameManager.PlayerType playerType, GameManager.PlayerType[,] board)
    {
        // 가로로 마커가 일치하는지 확인 
        for (var row = 0; row < board.GetLength(0); row++)
        {
            if (board[row, 0] == playerType && board[row, 1] == playerType && board[row, 2] == playerType)
                return true;
        }
        
        // 세로로 마커가 일치하는지 확인
        for (var col = 0; col < board.GetLength(1); col++)
        {
            if (board[0, col] == playerType && board[1, col] == playerType && board[2, col] == playerType)
                return true;
        }
        
        // 대각선으로 마커가 일치하는지 확인
        if (board[0, 0] == playerType && board[1, 1] == playerType && board[2, 2] == playerType)
            return true;

        if (board[0, 2] == playerType && board[1, 1] == playerType && board[2, 0] == playerType)
            return true;
        
        return false;
    }
}