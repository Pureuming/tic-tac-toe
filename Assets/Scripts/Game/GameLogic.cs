public class GameLogic
{
    private BlockController _blockController;
    private Constants.PlayerType[,] _board;
    
    private enum GameResult { None, Win, Lose, Draw }
    
    public GameLogic(BlockController blockController)
    {
        _blockController = blockController;
        
        // _board 초기화
        _board = new Constants.PlayerType[3, 3];
    }
    
    /// <summary>
    /// _board에 새로운 값을 할당하는 함수
    /// </summary>
    /// <param name="playerType">할당하고자 하는 플레이어 타입</param>
    /// <param name="row">Row</param>
    /// <param name="col">Col</param>
    /// <returns>False : 할당할 수 없음, True : 할당이 완료됨</returns>
    private bool SetNewBoardValue(Constants.PlayerType playerType, int row, int col)
    {
        if (_board[row, col] != Constants.PlayerType.None) return false; // 중복 체크 방지
        
        if (playerType == Constants.PlayerType.PlayerA)
        {
            _board[row, col] = playerType;
            _blockController.PlaceMarker(Block.MarkerType.O, row, col);
            return true;
        }
        else if (playerType == Constants.PlayerType.PlayerB)
        {
            _board[row, col] = playerType;
            _blockController.PlaceMarker(Block.MarkerType.X, row, col);
            return true;
        }
        return false;
    }
    
    /// <summary>
    /// 게임 결과 확인 함수
    /// </summary>
    /// <returns>플레이어 기준 게임 결과</returns>
    private GameResult CheckGameResult()
    {
        if (CheckGameWin(Constants.PlayerType.PlayerA)) return GameResult.Win;
        if (CheckGameWin(Constants.PlayerType.PlayerB)) return GameResult.Lose;
        if (MinimaxAIController.IsAllBlocksPlaced(_board)) return GameResult.Draw;
        
        return GameResult.None;
    }
    
    // 게임의 승패를 판단하는 함수
    private bool CheckGameWin(Constants.PlayerType playerType)
    {
        // 가로로 마커가 일치하는지 확인 
        for (var row = 0; row < _board.GetLength(0); row++)
        {
            if (_board[row, 0] == playerType && _board[row, 1] == playerType && _board[row, 2] == playerType)
            {
                (int, int)[] blocks = { (row, 0), (row, 1), (row, 2) };
                _blockController.SetBlockColor(playerType, blocks);
                return true;
            }
        }
        
        // 세로로 마커가 일치하는지 확인
        for (var col = 0; col < _board.GetLength(1); col++)
        {
            if (_board[0, col] == playerType && _board[1, col] == playerType && _board[2, col] == playerType)
            {
                (int, int)[] blocks = { (0, col), (1, col), (2, col) };
                _blockController.SetBlockColor(playerType, blocks);
                return true;
            }
        }
        
        // 대각선으로 마커가 일치하는지 확인
        if (_board[0, 0] == playerType && _board[1, 1] == playerType && _board[2, 2] == playerType)
        {
            (int, int)[] blocks = { (0, 0), (1, 1), (2, 2) };
            _blockController.SetBlockColor(playerType, blocks);
            return true;
        }

        if (_board[0, 2] == playerType && _board[1, 1] == playerType && _board[2, 0] == playerType)
        {
            (int, int)[] blocks = { (0, 2), (1, 1), (2, 0) };
            _blockController.SetBlockColor(playerType, blocks);
            return true;
        }
        
        return false;
    }
    
    /// <summary>
    /// 게임 오버 시, 호출되는 함수
    /// gameResult에 따라 결과 출력
    /// </summary>
    /// <param name="gameResult">win, lose, draw</param>
    private void EndGame(GameResult gameResult)
    {
        // // 게임오버 표시
        // _gameUIController.SetGameUIMode(GameUIController.GameUIMode.GameOver);
        // _blockController.OnBlockClickedDelegate = null; // 게임이 끝났을 때 board를 Click해도 바뀌지 않도록
    }
}