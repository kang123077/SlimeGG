using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

[System.Serializable]
public class MonsterDictionaryStat
{
    public string displayName;
    public int tier;
    public string desc;
    public List<BasicStat> basic;
    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
    public List<ElementEnum> element;
    public List<ElementStat> exp;
    public List<ElementStat> elementEvol;
    public List<string> nextMonster;
    public List<string> skills { get; set; }
}
