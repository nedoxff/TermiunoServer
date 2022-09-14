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
    public async Task Login(string nickname)
    {
        if (ConnectedUsers.ContainsKey(nickname))
        {
            await Clients.Caller.SendAsync("LoginResult",
                JsonConvert.SerializeObject(new LoginResponse
                {
                    ErrorCode = "ALREADY_LOGGED_IN",
                    Success = false
                }));
        }
        else
        {
            Log.Information("New player connected ({Nickname}, {Id})", nickname, Context.UserIdentifier);
            ConnectedUsers[Context.UserIdentifier!] = nickname;
            await Clients.Caller.SendAsync("LoginResult", JsonConvert.SerializeObject(new LoginResponse
            {
                Success = true
            }));
        }
    }
}