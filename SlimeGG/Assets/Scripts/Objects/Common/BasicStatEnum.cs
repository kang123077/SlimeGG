using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

[JsonConverter(typeof(StringEnumConverter))]
public enum BasicStatEnum
{
    hp, def, atk, spd,

    hp_multiple, atk_multiple, spd_multiple,

    // 관통력
    penetration_sum, penetration_multiple, penetration_abs,

    // 방어력 관련
    def_sum, def_multiple, invincible,

    position, timeCoolCycle, timeCastingCycle,
}