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
            Name = string.IsNullOrEmpty(name) ? Guid.NewGuid().ToString("N"): name,
            CreatorId = Context.UserIdentifier!,
            Players = new Dictionary<string, string>
            {
                {Context.UserIdentifier!, ConnectedUsers[Context.UserIdentifier!]}
            }
        };
        Log.Information("Created new room ({Id})", room.Id);
        Rooms.Add(room);
        await Clients.Caller.SendAsync("CreateRoomResponse",  JsonConvert.SerializeObject(room, _camelCaseSettings));
        await RefreshRoomsList();
    }

    private async Task RemoveRoom(Room room)
    {
        Rooms.Remove(room);
        await RefreshRoomsList();
    }

    private async Task RefreshRoomsList() =>
        await Clients.All.SendAsync("RefreshRoomsList", JsonConvert.SerializeObject(Rooms, _camelCaseSettings));

    public async Task GetRooms() => await Clients.Caller.SendAsync("GetRoomsResponse", JsonConvert.SerializeObject(Rooms, _camelCaseSettings));
}