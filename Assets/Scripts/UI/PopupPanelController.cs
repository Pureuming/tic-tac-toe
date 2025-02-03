using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // DOTween을 사용하기 위해 선언

[RequireComponent(typeof(CanvasGroup))]
public class PopupPanelController : Singleton<PopupPanelController>
{
    [SerializeField] private TMP_Text contentText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private TMP_Text confirmButtonText;

    [SerializeField] private RectTransform panelRectTransform;

    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        Hide(true);
    }

    public void Show(string content, string confirmButtonText, bool animation, Action confirmAction)
    {
        gameObject.SetActive(true);

        // animation을 위한 초기화
        _canvasGroup.alpha = 0;
        panelRectTransform.localScale = Vector3.zero;

        if (animation)
        {
            // DOTween 사용
            panelRectTransform.DOScale(1f, 0.2f);
            _canvasGroup.DOFade(1f, 0.2f).SetEase(Ease.OutBack);
            // SetEase() : animation을 빠르게 시작했다가 점차 느려지도록, / OutBack : 효과
        }
        else
        {
            panelRectTransform.localScale = Vector3.one;
            _canvasGroup.alpha = 1f;
        }
        
        contentText.text = content;
        this.confirmButtonText.text = confirmButtonText;
        confirmButton.onClick.AddListener(() =>
        {
            confirmAction();
            Hide(true);
        });
    }

    public void Hide(bool animation)
    {
        if (animation)
        {
            // OnComplete() : DOScale() 함수가 끝나고 난 뒤 실행
            panelRectTransform.DOScale(0f, 0.2f).OnComplete(() =>
            {
                contentText.text = "";
                confirmButtonText.text = "";
                confirmButton.onClick.RemoveAllListeners();

                gameObject.SetActive(false);
            });
            _canvasGroup.DOFade(0f, 0.2f).SetEase(Ease.InBack);
        }
        else
        {
            contentText.text = "";
            confirmButtonText.text = "";
            confirmButton.onClick.RemoveAllListeners();

            gameObject.SetActive(false);
        }
    }
}