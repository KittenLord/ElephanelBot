using System;

namespace Elephanel;

public class ServerCategory
{
    public ServerCategory(string serverId, ulong categoryId, ulong consoleChannelId)
    {
        ServerId = serverId;
        CategoryId = categoryId;
        ConsoleChannelId = consoleChannelId;
    }

    public string ServerId { get; private set; }
    public ulong CategoryId { get; private set; }
    public ulong ConsoleChannelId { get; private set; }
}