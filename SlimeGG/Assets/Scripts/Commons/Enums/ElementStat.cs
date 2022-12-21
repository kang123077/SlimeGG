using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[System.Serializable]
public class ElementStat
{
    [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
    public List<ElementEnum> elements { get; set; }
    public List<float> stats { get; set; }
}