using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[System.Serializable]
public class SkillStat : ElementStat
{
    [JsonConverter(typeof(StringEnumConverter))]
    public MonsterSkillEnum skillName { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public MonsterSkillTypeEnum skillType { get; set; }
    public int numberOfTarget { get; set; }
    public float range { get; set; }
    public float amount { get; set; }
    public float coolTime { get; set; }
    public float delayTime { get; set; }
    public float speed { get; set; }
    public string resourcePath { get; set; }
}