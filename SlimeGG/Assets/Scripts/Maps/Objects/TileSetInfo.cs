using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[System.Serializable]
public class TileSetInfo : ElementStat
{
    [JsonConverter(typeof(StringEnumConverter))]
    public TileSetNameEnum tileName { get; set; }
    public string nickName { get; set; }
    public int tier { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public TileSetShapeEnum tileSetShape { get; set; }
}
