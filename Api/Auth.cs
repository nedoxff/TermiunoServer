using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Serilog;

namespace TermiunoServer.Api;

public partial class GameHub
{
    private class LoginResponse
    {
        [JsonProperty("success")] public bool Success;
        [JsonProperty("message")] public string? ErrorCode;
    }
    public static Dictionary<string, string> ConnectedUsers = new();
    public async Task TryLogin(string nickname)
    {
        if (ConnectedUsers.ContainsValue(nickname))
        {
            await Clients.Caller.SendAsync("TryLoginResponse",
                JsonConvert.SerializeObject(new LoginResponse
                {
                    ErrorCode = "error.alreadyLoggedIn",
                    Success = false
                }));
        }
        else
        {
            Log.Information("New player connected ({Nickname}, {Id})", nickname, Context.UserIdentifier);
            ConnectedUsers[Context.UserIdentifier!] = nickname;
            await Clients.Caller.SendAsync("TryLoginResponse", JsonConvert.SerializeObject(new LoginResponse
            {
                Success = true
            }));
        }
    }
}