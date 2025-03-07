using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChattingPanelController : MonoBehaviour
{
    [SerializeField] private TMP_InputField messageInputField;
    [SerializeField] private GameObject messageTextPrefab;
    [SerializeField] private Transform messageTextParent;

    public void OnEndEditInputField(string messageText)
    {
        var messageTextObject = Instantiate(messageTextPrefab, messageTextParent);
        messageTextObject.GetComponent<TMP_Text>().text = messageText;
        messageInputField.text = "";    // message를 보내고 나면 InputField를 비워서 다음 message를 받을 수 있도록
    }
}