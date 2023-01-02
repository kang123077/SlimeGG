using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

[System.Serializable]
public class BasicStatVO : PlainStatVO
{
    [JsonConverter(typeof(StringEnumConverter))]
    public BasicStatEnum name;

    public BasicStatVO(float amount) : base(amount)
    {
    }
}
