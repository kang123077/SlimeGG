using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[System.Serializable]
public class SkillStat
{
    // 스킬 타입
    [JsonConverter(typeof(StringEnumConverter))]
    public MonsterSkillEnum skillName { get; set; }

    // 이동 타입:: 대상 판별, 넉백, 대쉬 계산 시 사용
    // 스킬 효과 (발동 타이밍:: 스킬 사용 시)
    [JsonConverter(typeof(StringEnumConverter))]
    public MonsterSkillTypeEnum skillType { get; set; }
    // 스킬 대상 수
    public int numberOfTarget { get; set; }
    // 사거리
    public float range { get; set; }
    // 스킬 쿨타임
    public float coolTime { get; set; }
    // 스킬 영창 시간
    public float castingTime { get; set; }
    public string resourcePath { get; set; }

    // 투사체 타입
    [JsonConverter(typeof(StringEnumConverter))]
    public ProjectileTypeEnum projectileType { get; set; }
    // 투사체 개수
    public int count = 1;
    // 투사체 간 사이 시간
    public float delayTime { get; set; }
    // 투사체 속도
    public float speed { get; set; }
    // 투사체 반지름 (필요시)
    public float radius { get; set; }
    // 투사체 지속시간 (필요시)
    public float durationTime { get; set; }
    // 스킬 효과 (발동 타이밍:: 투사체 피격 시)
    public ConditionHandlingStat effect { get; set; }
}