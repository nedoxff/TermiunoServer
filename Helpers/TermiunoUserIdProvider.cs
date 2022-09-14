using Microsoft.AspNetCore.SignalR;

namespace TermiunoServer.Helpers;

public class TermiunoUserIdProvider: IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return Guid.NewGuid().ToString("N");
    }
}