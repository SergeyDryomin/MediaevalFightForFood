using System.Text.Json.Serialization;

namespace MediaevalFightForFood.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TeamType
{
    Green,
    Red
}

public static class TeamTypeExtension
{
    public static TeamType First()
    {
        return TeamType.Green;
    }
}
