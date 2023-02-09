using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

[JsonConverter(typeof(StringEnumConverter))]
public enum BasicStatEnum
{
    hp, def, atk, spd,
    hp_multiple, def_multiple, atk_multiple, spd_multiple,
    position, timeCoolCycle, invincible, timeCastingCycle,

}