using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace VMCS.API.Hubs;

public class ChatHub : Hub
{
    private static Dictionary<string, List<string>> _chats = new();

    public async Task JoinChat(string meetingId)
    {
        if (!_chats.ContainsKey(meetingId))
            _chats.Add(meetingId, new List<string>());

        _chats[meetingId].Add(Context.ConnectionId);
        
        await Groups.AddToGroupAsync(Context.ConnectionId, meetingId);
    }
        
    public async Task LeaveChat(string meetingId)
    {
        if (!_chats.ContainsKey(meetingId))
            throw new ArgumentException(meetingId);
        
        _chats[meetingId].Remove(Context.ConnectionId);
        
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, meetingId);
    }
    
    public async Task SendMessage(string text, string login, string chatId)
    {
        throw new NotImplementedException();
    }
}