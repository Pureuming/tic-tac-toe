using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardPanelController : PanelController
{
    [SerializeField] private GameObject scoreCell;
    [SerializeField] private Transform content;

    public void CreateScoreCell(ScoreInfo scoreInfo)
    {
        var scoreCellObject = Instantiate(scoreCell, content);
        var scoreCellController = scoreCellObject.GetComponent<ScoreCellController>();
        scoreCellController.SetCellInfo(scoreInfo);
    }
    
    /// <summary>
    /// X 버튼 클릭 시 호출되는 함수
    /// </summary>
    public void OnClickCloseButton()
    {
        Hide();
    }
}