﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using VMCS.Core.Domains.Meetings.Repositories;

namespace VMCS.API.Hubs;

public class MeetingHub : Hub
{
    private static readonly Dictionary<string, List<string>> _meetings = new();
    private readonly IMeetingRepository _meetingRepository;

    public MeetingHub(IMeetingRepository meetingRepository)
    {
        _meetingRepository = meetingRepository;
    }

    public async Task JoinMeeting(string meetingId)
    {
        if (!_meetingRepository.ContainsById(meetingId))
            throw new ObjectNotFoundException($"Meeting with id={meetingId} not found.");

        var username = Context.User.FindFirstValue(ClaimTypes.GivenName) ?? "Anonymous";

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

    public async Task AddTrack(string meetingId, object track)
    {
        await Clients.Caller.SendAsync("ReceiveTrack", track);
    }

    public async Task ToggleMic(string meetingId)
    {
        await Clients.Groups(meetingId).SendAsync("ToggleMic", Context.ConnectionId);
    }

    public async Task ToggleWebCamera(string meetingId, bool isActive)
    {
        await Clients.Groups(meetingId).SendAsync("ToggleWebCamera", Context.ConnectionId,isActive);
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
        foreach (var kvp in _meetings.Where(kvp => kvp.Value.Contains(Context.ConnectionId)))
        {
            kvp.Value.Remove(Context.ConnectionId);
            Groups.RemoveFromGroupAsync(Context.ConnectionId, kvp.Key);
        }

        return base.OnDisconnectedAsync(exception);
    }
}