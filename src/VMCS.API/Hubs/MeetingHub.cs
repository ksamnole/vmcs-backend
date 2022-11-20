using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace VMCS.API.Hubs;

public class MeetingHub : Hub
{
    private static Dictionary<string, List<string>> _meetings;

    static MeetingHub()
    {
        _meetings = new Dictionary<string, List<string>>();
    }

    public async Task JoinMeeting(string meetingId)
    {
        if (!_meetings.ContainsKey(meetingId))
            _meetings.Add(meetingId, new List<string>());

        _meetings[meetingId].Add(Context.ConnectionId);
        
        await Groups.AddToGroupAsync(Context.ConnectionId, meetingId);
    }
        
    public async Task LeaveMeeting(string meetingId)
    {
        if (!_meetings.ContainsKey(meetingId))
            throw new ArgumentException();
        
        _meetings[meetingId].Remove(Context.ConnectionId);
        
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, meetingId);
    }

    public async Task SendMessageToMeeting(string text, string login, string meetingId)
    {
        if (!_meetings.ContainsKey(meetingId))
            throw new ArgumentException();

        if (IsMeetingParticipant(Context.ConnectionId, meetingId))
        {
            await Clients.Group(meetingId).SendAsync("ReceiveMessage", login, text);
        }
    }

    public async Task SendOffer(string clientId, object offer)
    {
        await Clients.Client(clientId).SendAsync("ReceiveOffer", Context.ConnectionId, offer);
    }
        
    public async Task SendAnswer(string clientId, object answer)
    {
        await Clients.Client(clientId).SendAsync("ReceiveAnswer", Context.ConnectionId, answer);
    }

    public async Task AddIceCandidate(string roomId, object obj)
    {
        await Clients.OthersInGroup(roomId).SendAsync("ReceiveIceCandidate", Context.ConnectionId, obj);
    }
    
    private static bool IsMeetingParticipant(string connectionId, string meetingId)
    {
        return _meetings[meetingId].FirstOrDefault(id => id == connectionId) != null;
    }
}