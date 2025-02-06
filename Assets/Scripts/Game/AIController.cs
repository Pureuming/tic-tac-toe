public static class AIController
{
    private static GameManager.PlayerType[,] _board;
    
    public static (int row, int col) FindNextMove(GameManager.PlayerType[,] board)
    {
        // TODO: board의 내용을 보고 다음 수를 계산 후 반환
        // 1. 중앙이 비었으면 무조건 중앙을 체크
        if (board[1, 1] == GameManager.PlayerType.None) { return (1, 1); }
        
        // 2. player가 두 칸이 이어져있고 남은 한 칸이 비어있으면 체크
        // 가로 체크
        for (var row = 0; row < board.GetLength(0); row++)
        {
            if (board[row, 0] == GameManager.PlayerType.PlayerA &&
                board[row, 1] == GameManager.PlayerType.PlayerA &&
                board[row, 2] == GameManager.PlayerType.None)
            {
                return (row, 2);
            }
            else if (board[row, 0] == GameManager.PlayerType.None &&
                     board[row, 1] == GameManager.PlayerType.PlayerA &&
                     board[row, 2] == GameManager.PlayerType.PlayerA)
            {
                return (row, 0);
            }
            else if (board[row, 0] == GameManager.PlayerType.PlayerA &&
                     board[row, 1] == GameManager.PlayerType.None &&
                     board[row, 2] == GameManager.PlayerType.PlayerA)
            {
                return (row, 1);
            }
        }
        
        // 세로 체크
        for (var col = 0; col < board.GetLength(1); col++)
        {
            if (board[0, col] == GameManager.PlayerType.PlayerA &&
                board[1, col] == GameManager.PlayerType.PlayerA &&
                board[2, col] == GameManager.PlayerType.None)
            {
                return (2, col);
            }
            else if (board[0, col] == GameManager.PlayerType.None &&
                     board[1, col] == GameManager.PlayerType.PlayerA &&
                     board[2, col] == GameManager.PlayerType.PlayerA)
            {
                return (0, col);
            }
            else if (board[0, col] == GameManager.PlayerType.PlayerA &&
                     board[1, col] == GameManager.PlayerType.None &&
                     board[2, col] == GameManager.PlayerType.PlayerA)
            {
                return (1, col);
            }
        }
        
        // 대각선 체크
        if (board[0, 0] == GameManager.PlayerType.PlayerA &&
            board[1, 1] == GameManager.PlayerType.PlayerA &&
            board[2, 2] == GameManager.PlayerType.None)
        {
            return (2, 2);
        }
        else if (board[0, 0] == GameManager.PlayerType.None &&
                 board[1, 1] == GameManager.PlayerType.PlayerA &&
                 board[2, 2] == GameManager.PlayerType.PlayerA)
        {
            return (0, 0);
        }
        
        if (board[0, 2] == GameManager.PlayerType.PlayerA &&
            board[1, 1] == GameManager.PlayerType.PlayerA &&
            board[2, 0] == GameManager.PlayerType.None)
        {
            return (2, 0);
        }
        else if (board[0, 2] == GameManager.PlayerType.None &&
                 board[1, 1] == GameManager.PlayerType.PlayerA &&
                 board[2, 0] == GameManager.PlayerType.PlayerA)
        {
            return (0, 2);
        }
        
        // 3. AI가 두 칸이 이어져있고 한 칸이 비어있으면 체크해서 승리
        // 가로 체크
        for (var row = 0; row < board.GetLength(0); row++)
        {
            if (board[row, 0] == GameManager.PlayerType.PlayerB &&
                board[row, 1] == GameManager.PlayerType.PlayerB &&
                board[row, 2] == GameManager.PlayerType.None)
            {
                return (row, 2);
            }
            else if (board[row, 0] == GameManager.PlayerType.None &&
                     board[row, 1] == GameManager.PlayerType.PlayerB &&
                     board[row, 2] == GameManager.PlayerType.PlayerB)
            {
                return (row, 0);
            }
            else if (board[row, 0] == GameManager.PlayerType.PlayerB &&
                     board[row, 1] == GameManager.PlayerType.None &&
                     board[row, 2] == GameManager.PlayerType.PlayerB)
            {
                return (row, 1);
            }
        }
        
        // 세로 체크
        for (var col = 0; col < board.GetLength(1); col++)
        {
            if (board[0, col] == GameManager.PlayerType.PlayerB &&
                board[1, col] == GameManager.PlayerType.PlayerB &&
                board[2, col] == GameManager.PlayerType.None)
            {
                return (2, col);
            }
            else if (board[0, col] == GameManager.PlayerType.None &&
                     board[1, col] == GameManager.PlayerType.PlayerB &&
                     board[2, col] == GameManager.PlayerType.PlayerB)
            {
                return (0, col);
            }
            else if (board[0, col] == GameManager.PlayerType.PlayerB &&
                     board[1, col] == GameManager.PlayerType.None &&
                     board[2, col] == GameManager.PlayerType.PlayerB)
            {
                return (1, col);
            }
        }
        
        // 대각선 체크
        if (board[0, 0] == GameManager.PlayerType.PlayerB &&
            board[1, 1] == GameManager.PlayerType.PlayerB &&
            board[2, 2] == GameManager.PlayerType.None)
        {
            return (2, 2);
        }
        else if (board[0, 0] == GameManager.PlayerType.None &&
                 board[1, 1] == GameManager.PlayerType.PlayerB &&
                 board[2, 2] == GameManager.PlayerType.PlayerB)
        {
            return (0, 0);
        }
        
        if (board[0, 2] == GameManager.PlayerType.PlayerB &&
            board[1, 1] == GameManager.PlayerType.PlayerB &&
            board[2, 0] == GameManager.PlayerType.None)
        {
            return (2, 0);
        }
        else if (board[0, 2] == GameManager.PlayerType.None &&
                 board[1, 1] == GameManager.PlayerType.PlayerB &&
                 board[2, 0] == GameManager.PlayerType.PlayerB)
        {
            return (0, 2);
        }
        
        // 4. 앞선 세 경우가 아니라면 남은 칸 중에 랜덤으로 체크 --> 일단 지금은 랜덤 없이 테스트
        for (var row = 0; row < board.GetLength(1); row++)
        {
            for (var col = 0; col < board.GetLength(0); col++)
            {
                if (board[row, col] == GameManager.PlayerType.None)
                {
                    return (row, col);
                }
            }
        }
        
        return (0, 0);
    }

    private static bool CheckTwoMarker(GameManager.PlayerType[,] board, GameManager.PlayerType playerType)
    {
        _board = board;
        
        // 가로 체크
        for (var row = 0; row < board.GetLength(0); row++)
        {
            if (board[row, 0] == playerType && board[row, 1] == playerType)
            {
                
            }
        }

        return false;
    }
}