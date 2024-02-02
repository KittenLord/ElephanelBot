using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Elephanel.Pterodactyl;

public enum WSEvent
{
    Auth,
    SendStats,
    SendLogs,
    SetState,
    SendCommand,
    AuthSuccess,
    Status,
    ConsoleOutput,
    Stats,
    TokenExpiring,
    TokenExpired
}

public class WebSocketMessage
{
    private static string[] Events = new string[] 
    {"auth", "send stats", "send logs", "set state", "send command", "auth success", "status", "console output", "stats", "token expiring", "token expired"};

    [JsonProperty("event")]
    public string EventName { get; set; }

    [JsonIgnore]
    public WSEvent Event => (WSEvent)Array.IndexOf(Events, EventName);
    public void SetEvent(WSEvent eventCode) { EventName = Events[(int)eventCode]; }

    [JsonProperty("args")]
    public List<string> Arguments { get; set; }
}


public class ServerCollection
{
    [JsonProperty("data")] public List<Server> Data { get; set; }
}

public class SftpDetails
{
    [JsonProperty("ip")] public string Ip { get; set; }
    [JsonProperty("port")] public int Port { get; set; }
}

public class Limits
{
    [JsonProperty("memory")] public int Memory { get; set; }
    [JsonProperty("swap")] public int Swap { get; set; }
    [JsonProperty("disk")] public int Disk { get; set; }
    [JsonProperty("io")] public int Io { get; set; }
    [JsonProperty("cpu")] public int Cpu { get; set; }
    [JsonProperty("threads")] public object Threads { get; set; }
    [JsonProperty("oom_disabled")] public bool OomDisabled { get; set; }
}

public class AllocationAttributes
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("ip")] public string Ip { get; set; }
    [JsonProperty("ip_alias")] public string IpAlias { get; set; }
    [JsonProperty("port")] public int Port { get; set; }
    [JsonProperty("notes")] public object Notes { get; set; }
    [JsonProperty("is_default")] public bool IsDefault { get; set; }
}

public class AllocationsHolder
{
    [JsonProperty("data")] public List<Allocation> Allocations { get; set; }
}

public class Allocation
{
    [JsonProperty("object")] public string Object { get; set; }
    [JsonProperty("attributes")] public AllocationAttributes Attributes { get; set; }
}

public class EggVariableAttributes
{
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("description")] public string Description { get; set; }
    [JsonProperty("env_variable")] public string EnvVariable { get; set; }
    [JsonProperty("default_value")] public string DefaultValue { get; set; }
    [JsonProperty("server_value")] public object ServerValue { get; set; }
    [JsonProperty("is_editable")] public bool IsEditable { get; set; }
    [JsonProperty("rules")] public string Rules { get; set; }
}

public class EggVariablesHolder
{
    [JsonProperty("data")] public List<EggVariable> EggVariables { get; set; }
}

public class EggVariable
{
    [JsonProperty("object")] public string Object { get; set; }
    [JsonProperty("attributes")] public EggVariableAttributes Attributes { get; set; }
}

public class Relationships
{
    [JsonProperty("allocations")] public AllocationsHolder Allocations { get; set; }
    [JsonProperty("variables")] public EggVariablesHolder Variables { get; set; }
}

public class ServerAttributes
{
    [JsonProperty("server_owner")] public bool ServerOwner { get; set; }
    [JsonProperty("identifier")] public string Identifier { get; set; }
    [JsonProperty("internal_id")] public int InternalId { get; set; }
    [JsonProperty("uuid")] public string Uuid { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("node")] public string Node { get; set; }
    [JsonProperty("is_node_under_maintenance")] public bool IsNodeUnderMaintenance { get; set; }
    [JsonProperty("sftp_details")] public SftpDetails SftpDetails { get; set; }
    [JsonProperty("description")] public string Description { get; set; }
    [JsonProperty("limits")] public Limits Limits { get; set; }
    [JsonProperty("invocation")] public string Invocation { get; set; }
    [JsonProperty("docker_image")] public string DockerImage { get; set; }
    [JsonProperty("egg_features")] public List<string> EggFeatures { get; set; }
    [JsonProperty("feature_limits")] public Dictionary<string, int> FeatureLimits { get; set; }
    [JsonProperty("status")] public object Status { get; set; }
    [JsonProperty("is_suspended")] public bool IsSuspended { get; set; }
    [JsonProperty("is_installing")] public bool IsInstalling { get; set; }
    [JsonProperty("is_transferring")] public bool IsTransferring { get; set; }
    [JsonProperty("relationships")] public Relationships Relationships { get; set; }
}

public class Server
{
    [JsonProperty("object")] public string Object { get; set; }
    [JsonProperty("attributes")] public ServerAttributes Attributes { get; set; }
}

public class Account
{
    [JsonProperty("attributes")] public AccountAttributes Attributes { get; set; }
}

public class AccountAttributes
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("admin")] public bool IsAdmin { get; set; }
    [JsonProperty("username")] public string Username { get; set; }
    [JsonProperty("email")] public string Email { get; set; }
    [JsonProperty("first_name")] public string FirstName { get; set; }
    [JsonProperty("last_name")] public string LastName { get; set; }
    [JsonProperty("language")] public string Language { get; set; }
    
}