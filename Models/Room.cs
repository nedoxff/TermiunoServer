namespace TermiunoServer.Models;

public struct Room
{
    public string Name = "";
    public string CreatorId = "";
    public string Id;
    public int MaxPlayers = 0;
    public bool Playing;
    public Dictionary<string, string> Players = new();

    public Room()
    {
    }
}