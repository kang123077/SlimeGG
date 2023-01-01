using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[System.Serializable]
public class ElementStat
{
    [JsonConverter(typeof(StringEnumConverter))]
    public ElementEnum name;
    public float amount;
    public bool isMultiple;
}
