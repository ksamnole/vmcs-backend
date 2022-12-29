using System;
using System.Collections.Generic;
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

    public async Task JoinChats(IEnumerable<string> chatsId)
    {
        foreach (var chatId in chatsId)
            await JoinChat(chatId);
    }

    public async Task JoinChat(string chatId)
    {
        if (!_chats.ContainsKey(chatId))
            _chats.Add(chatId, new List<string>());

        _chats[chatId].Add(Context.ConnectionId);
        
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
    }
        
    public async Task LeaveChat(string chatId)
    {
        if (!_chats.ContainsKey(chatId))
            throw new ArgumentException(chatId);
        
        _chats[chatId].Remove(Context.ConnectionId);
        
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
    }
    
    public async Task SendMessage(string text, string chatId)
    {
        if (_httpContextAccessor.HttpContext == null)
            throw new Exception();
        
        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var username = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.GivenName);

        var message = await _messageService.Create(new Message()
        {
            ChatId = chatId,
            Text = text,
            UserId = userId,
            Username = username
        }, CancellationToken.None);
        
        await Clients.Group(chatId).SendAsync("ReceiveMessage", message.Id ,message.Username, message.Text, message.ChatId);
    }
}