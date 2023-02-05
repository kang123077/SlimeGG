using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

[JsonConverter(typeof(StringEnumConverter))]
public enum BasicStatEnum
{
    hp, def, atk, spd,
    position, timeCoolCycle, invincible, timeCastingCycle
}