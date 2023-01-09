using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace VMCS.API.Hubs;

public class MeetingHub : Hub
{
    private static Dictionary<string, List<string>> _meetings = new ();

    public async Task JoinMeeting(string meetingId)
    {
        var username = Context.User.FindFirstValue(ClaimTypes.GivenName) ?? "Anonymous";
        
        // TODO: Получать встречу (id) из базы данных
        
        if (!_meetings.ContainsKey(meetingId))
            _meetings.Add(meetingId, new List<string>());

        _meetings[meetingId].Add(Context.ConnectionId);
        
        await Groups.AddToGroupAsync(Context.ConnectionId, meetingId);
        
        await Clients.OthersInGroup(meetingId).SendAsync("JoinClient", Context.ConnectionId, username);
    }
        
    public async Task LeaveMeeting(string meetingId)
    {
        if (!_meetings.ContainsKey(meetingId))
            throw new ArgumentException();
        
        _meetings[meetingId].Remove(Context.ConnectionId);
        
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, meetingId);
        
        await Clients.OthersInGroup(meetingId).SendAsync("LeaveClient", Context.ConnectionId);
    }

    public async Task SendOffer(string clientId, object offer)
    {
        var username = Context.User.FindFirstValue(ClaimTypes.GivenName) ?? "Anonymous";
        
        await Clients.Client(clientId).SendAsync("ReceiveOffer", Context.ConnectionId, offer, username);
    }
        
    public async Task SendAnswer(string clientId, object answer)
    {
        await Clients.Client(clientId).SendAsync("ReceiveAnswer", Context.ConnectionId, answer);
    }

    public async Task AddIceCandidate(string clientId, object iceCandidate)
    {
        await Clients.Client(clientId).SendAsync("ReceiveIceCandidate", Context.ConnectionId, iceCandidate);
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        foreach (var meeting in _meetings.Values.Where(meeting => meeting.Contains(Context.ConnectionId)))
            meeting.Remove(Context.ConnectionId);

        return base.OnDisconnectedAsync(exception);
    }
}