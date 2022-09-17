using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace TermiunoServer.Api;

public partial class GameHub: Hub
{
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (!string.IsNullOrEmpty(Context.UserIdentifier) && ConnectedUsers.ContainsKey(Context.UserIdentifier))
        {
            Log.Information("Disconnected user {Nickname} ({Id})", ConnectedUsers[Context.UserIdentifier], Context.UserIdentifier);
            ConnectedUsers.Remove(Context.UserIdentifier);
            var emptyRoom =
                Rooms.FirstOrDefault(x => x.Players.Count == 1 && x.Players.Last().Key == Context.UserIdentifier);
            if (emptyRoom != null)
                await RemoveRoom(emptyRoom);
        }
    }
}