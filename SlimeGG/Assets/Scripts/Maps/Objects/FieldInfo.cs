using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;

[System.Serializable]
public class FieldInfo
{
    [JsonConverter(typeof(StringEnumConverter))]
    public FieldNameEnum name { get; set; }
    public List<int> numberRestrictPerSide { get; set; }
    public List<float> size { get; set; }
    public List<List<List<float>>> initPosList { get; set; }
}