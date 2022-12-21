using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[System.Serializable]
public class TileSetInfo : ElementStat
{
    public string name { get; set; }
    public string nickName { get; set; }
    public int tier { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public TileSetShapeEnum tileSetShape { get; set; }
}
