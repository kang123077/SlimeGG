using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[System.Serializable]
public class MonsterSpeciesInfo
{
    // 저장을 위한 구성
    public List<MonsterVariableStat> basic { get; set; }
    public List<ElementStat> element { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public MonsterSpeciesEnum species { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public MonsterStageEnum stage { get; set; }
    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
    public List<MonsterSkillEnum> skills { get; set; }
    public string resourcePath { get; set; }

    // 실제로 사용하는 값
    public Dictionary<MonsterVariableEnum, MonsterVariableStat> basicDict { get; set; }
    public Dictionary<ElementEnum, ElementStat> elementDict { get; set; }

    public void extractInfo()
    {
        basicDict = new Dictionary<MonsterVariableEnum, MonsterVariableStat>();
        elementDict = new Dictionary<ElementEnum, ElementStat>();
        foreach (MonsterVariableStat stat in basic)
        {
            basicDict[stat.name] = stat;
        }
        foreach (ElementStat stat in element)
        {
            elementDict[stat.name] = stat;
        }
    }
}