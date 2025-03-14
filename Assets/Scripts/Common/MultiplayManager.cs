using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using SocketIOClient;

// game.js 에서 보내는 값을 받음
public class RoomData
{
    [JsonProperty("roomId")]
    public string roomId { get; set; }
}

public class UserData
{
    [JsonProperty("userId")]
    public string userId { get; set; }
}

public class MoveData
{
    [JsonProperty("position")]
    public int position { get; set; }
}

public class MessageData
{
    [JsonProperty("nickName")]
    public string nickName { get; set; }
    [JsonProperty("message")]
    public string message { get; set; }
}

public class MultiplayManager : IDisposable
{
    private SocketIOUnity _socket;
    private event Action<Constants.MultiplayManagerState, string> _onMultiplayStateChanged;
    public Action<MoveData> OnOpponentMove;
    public Action<MessageData> OnReceivedMessage;

    public MultiplayManager(Action<Constants.MultiplayManagerState, string> onMultiplayStateChanged)
    {
        _onMultiplayStateChanged = onMultiplayStateChanged;
        
        var uri = new Uri(Constants.GameServerURL);
        _socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });
        
        // 서버가 보낼 event(message)에 따라 작동
        // "createRoom"이라는 event가 왔을 때 CreateRoom이 작동하도록 CreateRoom()이라고 적지 않는다.
        _socket.On("createRoom", CreateRoom);
        _socket.On("joinRoom", JoinRoom);
        _socket.On("exitRoom", ExitRoom);
        _socket.On("startGame", StartGame);
        _socket.On("endGame", EndGame);
        _socket.On("doOpponent", DoOpponent);
        _socket.On("receiveMessage", ReceiveMessage);
        
        _socket.Connect();
    }

    private void CreateRoom(SocketIOResponse response)  // 매개변수로 SocketIOResponse가 필수로 있어야 한다.
    {
        var data = response.GetValue<RoomData>();
        _onMultiplayStateChanged?.Invoke(Constants.MultiplayManagerState.CreateRoom, data.roomId);
    }

    private void JoinRoom(SocketIOResponse response)
    {
        var data = response.GetValue<RoomData>();
        _onMultiplayStateChanged?.Invoke(Constants.MultiplayManagerState.JoinRoom, data.roomId);
    }

    private void ExitRoom(SocketIOResponse response)
    {
        _onMultiplayStateChanged?.Invoke(Constants.MultiplayManagerState.ExitRoom, null);
    }

    private void StartGame(SocketIOResponse response)
    {
        var data = response.GetValue<RoomData>();
        _onMultiplayStateChanged?.Invoke(Constants.MultiplayManagerState.StartGame, data.roomId);
    }

    private void EndGame(SocketIOResponse response)
    {
        _onMultiplayStateChanged?.Invoke(Constants.MultiplayManagerState.EndGame, null);
    }

    // 서버로부터 상대방의 마커 정보를 받기 위한 Method
    private void DoOpponent(SocketIOResponse response)
    {
        var data = response.GetValue<MoveData>();
        OnOpponentMove?.Invoke(data);
    }

    // Player의 마커 위치를 서버로 전달하기 위한 Method
    public void SendPlayerMove(string roomId, int position)
    {
        _socket.Emit("doPlayer", new { roomId, position });
    }

    // 서버로부터 Message를 받음
    private void ReceiveMessage(SocketIOResponse response)
    {
        var data = response.GetValue<MessageData>();
        OnReceivedMessage?.Invoke(data);
    }

    // 서버로 Message를 보냄
    public void SendMessage(string roomId , string nickName, string message)
    {
        _socket.Emit("sendMessage", new { roomId, nickName, message });
    }

    public void LeaveRoom(string roomId)
    {
        _socket.Emit("leaveRoom", new { roomId });
    }

    public void Dispose()
    {
        if (_socket != null)
        {
            _socket.Disconnect();
            _socket.Dispose();
            _socket = null;
        }
    }
}