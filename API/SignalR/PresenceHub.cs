namespace API.SignalR;

using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

[Authorize]
public class PresenceHub(PresenceTracker presenceTracker) : Hub
{
    public override async Task OnConnectedAsync()
       {
        if (Context.User == null)
        {
            throw new HubException("Cannot get the current user claim");
        }

        await presenceTracker.UserConnected(Context.User.GetUsername(), Context.ConnectionId);
        await Clients.Others.SendAsync("UserIsOnline", Context.User?.GetUsername());
        
        await GetOnlineUsers();;
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Context.User == null)
        {
            throw new HubException("Cannot get the current user claim");
        }

        await presenceTracker.UserDisconnected(Context.User.GetUsername(), Context.ConnectionId);
        await Clients.Others.SendAsync("UserIsOffline", Context.User?.GetUsername());
        await GetOnlineUsers();
        await base.OnDisconnectedAsync(exception);
    }

    private async Task GetOnlineUsers()
    {
        var onlineUsers = await presenceTracker.GetOnlineUsers();
        await Clients.All.SendAsync("GetOnlineUsers", onlineUsers);
    }
}