using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePanelController : PanelController
{
    [SerializeField] private Image player1Marker;
    [SerializeField] private Image player2Marker;

    private Color _defaultColor;
    
    public void SetTurnColor(GameManager.TurnType turnType)
    {
        _defaultColor = player1Marker.color;

        if (turnType == GameManager.TurnType.PlayerA)
        {
            player1Marker.color = new Color32(0, 166, 255, 255);
            player2Marker.color = _defaultColor;
        }
            
        else if (turnType == GameManager.TurnType.PlayerB)
        {
            player2Marker.color = new Color32(255, 0, 94, 255);
            player1Marker.color = _defaultColor;
        }
    }
}