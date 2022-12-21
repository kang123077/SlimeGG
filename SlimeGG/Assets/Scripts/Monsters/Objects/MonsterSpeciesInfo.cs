using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[System.Serializable]
public class MonsterSpeciesInfo : MonsterAbilityStat
{
    [JsonConverter(typeof(StringEnumConverter))]
    public MonsterSpeciesEnum species { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public MonsterStageEnum stage { get; set; }
    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
    public List<MonsterSkillEnum> skills { get; set; }
    public string resourcePath { get; set; }

    override public string ToString()
    {
        string elementToString = string.Empty;
        string statsToString = string.Empty;
        string skillsToString = string.Empty;

        elements.ForEach((str) =>
        {
            elementToString += str + ", ";
        });
        stats.ForEach((str) =>
        {
            statsToString += str + ", ";
        });
        skills.ForEach((str) =>
        {
            skillsToString += str + ", ";
        });

        return $"\n" +
            $"{species.ToString()}\n" +
            $"{stage.ToString()}\n" +
            $"{elementToString}\n" +
            $"{statsToString}\n" +
            $"{skillsToString}\n"
            ;
    }

}