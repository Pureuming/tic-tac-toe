public static class AIController
{
    private static GameManager.PlayerType[,] _board;
    
    public static (int row, int col)? FindNextMove(GameManager.PlayerType[,] board)
    {
        var result1 = FindTwoMarker(board);
        // HasValue : 값이 있는지 체크
        if (result1.HasValue) return result1.Value;
        var result2 = FindEmptyPosition(board, GameManager.PlayerType.PlayerA);
        if (result2.HasValue) return result2.Value;
        var result3 = FindEmptyPosition(board, GameManager.PlayerType.PlayerB);
        if (result3.HasValue) return result3.Value;
        var result4 = FindEmptyPosition(board, GameManager.PlayerType.None);
        if (result4.HasValue) return result4.Value;
        return null;
    }

    // 빈 칸을 찾은 뒤, 이미 있는 마커 근처를 반환하도록 
    private static (int row, int col)? FindEmptyPosition(GameManager.PlayerType[,] board,
        GameManager.PlayerType playerType)
    {
        for (var row = 0; row < board.GetLength(0); row++)
        {
            for (var col = 0; col < board.GetLength(1); col++)
            {
                if (board[row, col] == GameManager.PlayerType.None)
                {
                    if (playerType == GameManager.PlayerType.None) return (row, col);
                    
                    for (var i = -1; i <= 1; i++)
                    {
                        for (var j = -1; j <= 1; j++)
                        {
                            if (i == 0 && j == 0) continue;
                            if (row + i < 0 || row + i >= board.GetLength(0) ||
                                col + j < 0 || col + j >= board.GetLength(1)) continue;
                            if (board[row + i, col + j] == playerType) return (row, col);
                        }
                    }
                }
            }
        }

        return null;
    }

    // (int row, int col)'?' --> Nullable : null을 할당 할 수 없는 타입도 null로 할당 할 수 있도록, 사용법 : 변수 타입 뒤에 '?' 붙이기
    private static (int row, int col)? FindTwoMarker(GameManager.PlayerType[,] board)
    {
        // 1. 중앙이 비었으면 무조건 중앙을 체크
        if (board[1, 1] == GameManager.PlayerType.None) { return (1, 1); }
        
        // 2. 블록이 두 칸이 이어져있고 남은 한 칸이 비어있으면 체크 --> Player와 AI는 상관 없지만 None은 피해야 함
        // 가로 체크 (None을 피하는 더 아름다운 방법이 없을까..?)
        for (var row = 0; row < board.GetLength(0); row++)
        {
            if (board[row, 0] == board[row, 1] && board[row, 2] == GameManager.PlayerType.None
                                               && board[row, 0] != GameManager.PlayerType.None)
            {
                return (row, 2);
            }
            else if (board[row, 1] == board[row, 2] && board[row, 0] == GameManager.PlayerType.None 
                                                    && board[row, 1] != GameManager.PlayerType.None)
            {
                return (row, 0);
            }
            else if (board[row, 0] == board[row, 2] && board[row, 1] == GameManager.PlayerType.None
                                                    && board[row, 0] != GameManager.PlayerType.None)
            {
                return (row, 1);
            }
        }
        
        // 세로 체크
        for (var col = 0; col < board.GetLength(1); col++)
        {
            if (board[0, col] == board[1, col] && board[2, col] == GameManager.PlayerType.None 
                                               && board[0, col] != GameManager.PlayerType.None)
            {
                return (2, col);
            }
            else if (board[1, col] == board[2, col] && board[0, col] == GameManager.PlayerType.None
                                                    && board[1, col] != GameManager.PlayerType.None)
            {
                return (0, col);
            }
            else if (board[0, col] == board[2, col] && board[1, col] == GameManager.PlayerType.None
                                                    && board[0, col] != GameManager.PlayerType.None)
            {
                return (1, col);
            }
        }
        
        // 대각선 체크
        if (board[0, 0] == board[1, 1] && board[2, 2] == GameManager.PlayerType.None)
        {
            return (2, 2);
        }
        else if (board[1, 1] == board[2, 2] && board[0, 0] == GameManager.PlayerType.None)
        {
            return (0, 0);
        }
        
        if (board[0, 2] == board[1, 1] && board[2, 0] == GameManager.PlayerType.None)
        {
            return (2, 0);
        }
        else if (board[1, 1] == board[2, 0] && board[0, 2] == GameManager.PlayerType.None)
        {
            return (0, 2);
        }
        
        // 3. 앞선 두 경우가 아니라면 남은 칸 중에 랜덤으로 체크 --> 일단 지금은 랜덤 없이 테스트
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
        
        return null; // 이것을 위해 Nullable 사용
    }
}