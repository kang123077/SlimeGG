using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

[System.Serializable]
public class StageSaveStat
{
    public int id { get; set; }
    public List<float> locationPos { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public StageType type { get; set; }
    public List<int> next { get; set; }
}

