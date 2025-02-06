using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConfirmPanelController : PanelController
{
    [SerializeField] private TMP_Text messageText;

    public delegate void OnConfirmButtonClick();
    private OnConfirmButtonClick onConfirmButtonClick;

    // Confirm Button에 띄울 message도 받아서 사용하기 위해 함수 오버로딩을 활용
    public void Show(string message, OnConfirmButtonClick onConfirmButtonClick)
    {
        messageText.text = message;
        this.onConfirmButtonClick = onConfirmButtonClick;
    }

    /// <summary>
    /// Confirm 버튼 클릭 시 호출되는 함수
    /// </summary>
    public void OnClickConfirmButton()
    {
        onConfirmButtonClick?.Invoke();
        Hide();
    }
    
    /// <summary>
    /// X 버튼 클릭 시 호출되는 함수
    /// </summary>
    public void OnClickCloseButton()
    {
        Hide();
    }
}