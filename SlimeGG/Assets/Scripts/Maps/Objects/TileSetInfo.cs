using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

[System.Serializable]
public class TileSetInfo : ElementStat
{
    [JsonConverter(typeof(StringEnumConverter))]
    public TileSetNameEnum tileName { get; set; }
    public string nickName { get; set; }
    public int tier { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public TileSetShapeEnum tileSetShape { get; set; }
    public string resourcePath { get; set; }
}
