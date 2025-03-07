using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChattingPanelController : MonoBehaviour
{
    [SerializeField] private TMP_InputField messageInputField;
    [SerializeField] private GameObject messageTextPrefab;
    [SerializeField] private Transform messageTextParent;

    private MultiplayManager _multiplayManager;
    private string _roomId;

    public void OnEndEditInputField(string messageText)
    {
        var messageTextObject = Instantiate(messageTextPrefab, messageTextParent);
        messageTextObject.GetComponent<TMP_Text>().text = messageText;
        messageInputField.text = "";    // message를 보내고 나면 InputField를 비워서 다음 message를 받을 수 있도록

        if (_roomId != null && _multiplayManager != null)
        {
            // TODO: 임의로 넣은 "홍길동" 대신 로그인할 때 받아온 User의 nickName을 넣어주기
            _multiplayManager.SendMessage(_roomId, "홍길동", messageText);
        }
    }

    private void Start()
    {
        messageInputField.interactable = false;
        _multiplayManager = new MultiplayManager((state, id) =>
        {
            switch (state)
            {
                case Constants.MultiplayManagerState.CreateRoom:
                    Debug.Log("## Create Room ##");
                    _roomId = id;
                    break;
                case Constants.MultiplayManagerState.JoinRoom:
                    Debug.Log("## Join Room ##");
                    _roomId = id;
                    messageInputField.interactable = true;
                    break;
                case Constants.MultiplayManagerState.StartGame:
                    Debug.Log("## Start Game ##");
                    messageInputField.interactable = true;
                    break;
                case Constants.MultiplayManagerState.EndGame:
                    Debug.Log("## End Game ##");
                    break;
            }
        });
        _multiplayManager.OnReceivedMessage = OnReceiveMessage;
    }

    private void OnReceiveMessage(MessageData messageData)
    {
        UnityThread.executeInUpdate(() =>
        {
            var messageTextObject = Instantiate(messageTextPrefab, messageTextParent);
            messageTextObject.GetComponent<TMP_Text>().text = messageData.nickName + " : " + messageData.message;
        });
    }
}