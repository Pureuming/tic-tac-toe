using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class PanelController : MonoBehaviour
{
    public bool IsShow { get; private set; }

    public delegate void OnHide();
    private OnHide _onHideDelegate;

    private RectTransform _rectTransform;
    private Vector2 _hideAnchorPosition;

    private void Awake() // GameManager에서 Start에 InitGame()을 해주기 때문에 그 전에 Awake에서 세팅 
    {
        _rectTransform = GetComponent<RectTransform>();
        _hideAnchorPosition = _rectTransform.anchoredPosition; // 숨겨둔 StartPanel의 Anchor 위치 저장
        IsShow = false;
    }

    /// <summary>
    /// Panel 표시 함수
    /// </summary>
    public void Show(OnHide onHideDelegate)
    {
        _onHideDelegate = onHideDelegate;
        _rectTransform.anchoredPosition = Vector2.zero;
        IsShow = true;
    }
    
    /// <summary>
    /// Panel 숨기기 함수
    /// </summary>
    public void Hide()
    {
        _rectTransform.anchoredPosition = _hideAnchorPosition;
        IsShow = false;
        _onHideDelegate?.Invoke();
    }
}