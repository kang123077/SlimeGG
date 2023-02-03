using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[System.Serializable]
public class MonsterSpeciesVO
{
    public List<BasicStatVO> basic { get; set; }
    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
    public List<ElementEnum> element { get; set; }
    public List<string> skills { get; set; }
    public string src { get; set; }
}