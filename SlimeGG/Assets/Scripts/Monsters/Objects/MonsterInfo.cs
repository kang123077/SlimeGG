using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[System.Serializable]
public class MonsterInfo : MonsterAbilityStat
{
    public string nickName { get; set; }
    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
    public List<MonsterSpeciesEnum> accuSpecies { get; set; }
    public int installedPosition { get; set; }
}
