using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace VMCS.API.Hubs;

public class MeetingHub : Hub
{
    private static Dictionary<string, List<string>> _meetings = new ();

    public async Task JoinMeeting(string meetingId)
    {
        // TODO: Получать встречу (id) из базы данных
        
        if (!_meetings.ContainsKey(meetingId))
            _meetings[meetingId] = new List<string>();

        _meetings[meetingId].Add(Context.ConnectionId);
        
        await Groups.AddToGroupAsync(Context.ConnectionId, meetingId);
        
        await Clients.OthersInGroup(meetingId).SendAsync("JoinedNewClient", Context.ConnectionId);
    }
        
    public async Task LeaveMeeting(string meetingId)
    {
        if (!_meetings.ContainsKey(meetingId))
            throw new ArgumentException();
        
        _meetings[meetingId].Remove(Context.ConnectionId);
        
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, meetingId);
    }

    public async Task SendOffer(string clientId, object offer)
    {
        await Clients.Client(clientId).SendAsync("ReceiveOffer", Context.ConnectionId, offer);
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