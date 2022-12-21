using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[System.Serializable]
public class MonsterInfo : MonsterAbilityStat
{
    public string nickName { get; set; }
    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
    public List<MonsterSpeciesEnum> accuSpecies { get; set; }
    override public string ToString()
    {
        string elementToString = string.Empty;
        string statsToString = string.Empty;
        string treeToString = string.Empty;

        elements.ForEach((str) =>
        {
            elementToString += str + ", ";
        });
        stats.ForEach((str) =>
        {
            statsToString += str + ", ";
        });
        accuSpecies.ForEach((str) =>
        {
            treeToString += str + ", ";
        });

        return $"\n" +
            $"{nickName.ToString()}\n" +
            $"{elementToString}\n" +
            $"{statsToString}\n" +
            $"{treeToString}\n"
            ;
    }
}
