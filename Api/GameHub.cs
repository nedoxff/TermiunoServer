using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace TermiunoServer.Api;

public partial class GameHub: Hub
{
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        if (!string.IsNullOrEmpty(Context.UserIdentifier) && ConnectedUsers.ContainsKey(Context.UserIdentifier))
        {
            Log.Information("Disconnected user {Nickname} ({Id})", ConnectedUsers[Context.UserIdentifier], Context.UserIdentifier);
            ConnectedUsers.Remove(Context.UserIdentifier);
        }

        return Task.CompletedTask;
    }
}