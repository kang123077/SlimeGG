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
    override public string ToString()
    {
        string elementToString = string.Empty;
        string statsToString = string.Empty;

        elements.ForEach((str) =>
        {
            elementToString += str + ", ";
        });
        stats.ForEach((str) =>
        {
            statsToString += str + ", ";
        });

        return $"\n" +
            $"{skillName.ToString()}\n" +
            $"{skillType.ToString()}\n" +
            $"{numberOfTarget}\n" +
            $"{range}\n" +
            $"{amount}\n" +
            $"{coolTime}\n" +
            $"{delayTime}\n" +
            $"{statsToString}\n"
            ;
    }
}