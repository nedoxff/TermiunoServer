using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using TermiunoServer.Models;

namespace TermiunoServer.Api;

public partial class GameHub
{
    private static JsonSerializerSettings _camelCaseSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };
    
    public static List<Room> Rooms = new();
    public async Task CreateRoom(string name, int maxPlayers)
    {
        var room = new Room
        {
            Id = Guid.NewGuid().ToString("N"),
            MaxPlayers = maxPlayers,
            Playing = false,
            Name = name,
            CreatorId = Context.UserIdentifier!
        };
        Log.Information("Created new room ({Id})", room.Id);
        Rooms.Add(room);
        await Clients.All.SendAsync("RefreshRoomsList", JsonConvert.SerializeObject(Rooms, _camelCaseSettings));
    }

    public async Task GetAndRefreshRooms() => await Clients.Caller.SendAsync("RefreshRoomsList", JsonConvert.SerializeObject(Rooms, _camelCaseSettings));
}