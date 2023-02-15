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

    // 사거리 관련
    range, range_multiple,

    // 쿨타임
    timeCoolCycle, timeCoolCycle_multiple,

    // 시전시간
    timeCastingCycle, timeCastingCycle_multiple,

    // 군중제어 관련
    stop_cooltime, stop_position,

    position,
}