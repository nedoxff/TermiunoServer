namespace TermiunoServer.Models;

public class Room
{
    public string Name = null!;
    public string CreatorId = null!;
    public string Id = null!;
    public int MaxPlayers = -1;
    public bool Playing = false;
    public Dictionary<string, string> Players = new();
}