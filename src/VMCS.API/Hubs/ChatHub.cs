using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using VMCS.Core.Domains.Messages;
using VMCS.Core.Domains.Messages.Services;

namespace VMCS.API.Hubs;

public class ChatHub : Hub
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMessageService _messageService;
    private static Dictionary<string, List<string>> _chats = new();

    public ChatHub(IHttpContextAccessor httpContextAccessor, IMessageService messageService)
    {
        _httpContextAccessor = httpContextAccessor;
        _messageService = messageService;
    }

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
    
    public async Task SendMessage(string text, string chatId)
    {
        if (_httpContextAccessor.HttpContext == null)
            throw new Exception();
        
        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var username = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.GivenName);

        await _messageService.Create(new Message()
        {
            ChatId = chatId,
            Text = text,
            UserId = userId,
            Username = username
        }, CancellationToken.None);
        
        await Clients.Group(chatId).SendAsync("ReceiveMessage", username, text);
    }
}