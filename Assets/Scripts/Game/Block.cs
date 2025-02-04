﻿using System;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private Sprite oSprite;
    [SerializeField] private Sprite xSprite;
    [SerializeField] private SpriteRenderer markerSpriteRenderer;

    public enum MarkerType { None, O, X }

    public delegate void OnBlockClicked(int index); // void를 반환하고 int를 매개변수로 삼는 형태의 함수를 받는다.
    public OnBlockClicked onBlockClicked;
    
    private int _blockIndex;

    /// <summary>
    /// Block 초기화 함수
    /// </summary>
    /// <param name="blockIndex">Block 인덱스</param>
    /// <param name="onBlockClicked">Block 터치 이벤트</param>
    public void InitMarker(int blockIndex, OnBlockClicked onBlockClicked)
    {
        _blockIndex = blockIndex;
        SetMarker(MarkerType.None);
        this.onBlockClicked = onBlockClicked;
    }

    /// <summary>
    /// 어떤 마커를 표시할지 전달하는 함수
    /// </summary>
    /// <param name="markerType">마커 타입</param>
    public void SetMarker(MarkerType markerType) // 외부에서 마크 표시
    {
        switch (markerType)
        {
            case MarkerType.O:
                markerSpriteRenderer.sprite = oSprite;
                break;
            case MarkerType.X:
                markerSpriteRenderer.sprite = xSprite;
                break;
            case MarkerType.None:
                markerSpriteRenderer.sprite = null;
                break;
        }
    }

    private void OnMouseUpAsButton()
    {
        onBlockClicked?.Invoke(_blockIndex);
    }
}