using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;

[System.Serializable]
public class TileSetBriefInfo
{
    [JsonConverter(typeof(StringEnumConverter))]
    public TileSetNameEnum name { get; set; }
    public List<float> installedPosition { get; set; }
}