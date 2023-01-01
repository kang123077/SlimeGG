using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq;

[System.Serializable]
public class MonsterInfo
{
    // 저장을 위한 구성
    public List<MonsterVariableStat> basic { get; set; }
    public List<ElementStat> element { get; set; }
    public string nickName { get; set; }
    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
    public List<MonsterSpeciesEnum> accuSpecies { get; set; }
    public int installedPosition { get; set; }

    // 실제로 사용하는 값
    public Dictionary<MonsterVariableEnum, MonsterVariableStat> basicDict { get; set; }
    public Dictionary<ElementEnum, ElementStat> elementDict { get; set; }

    public MonsterInfo Clone()
    {
        MonsterInfo res = new MonsterInfo();
        res.basic = basic.ToList();
        res.element = element.ToList();
        res.nickName = nickName;
        res.accuSpecies = accuSpecies;
        res.installedPosition = installedPosition;
        res.basicDict = basicDict;
        res.elementDict = elementDict;

        return res;
    }

    public MonsterInfo()
    {
        basic = new List<MonsterVariableStat>();
        element = new List<ElementStat>();
        nickName = "";
        accuSpecies = new List<MonsterSpeciesEnum>();
        installedPosition = 0;
        basicDict = new Dictionary<MonsterVariableEnum, MonsterVariableStat>();
        elementDict = new Dictionary<ElementEnum, ElementStat>();
    }

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
