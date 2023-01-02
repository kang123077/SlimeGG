using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[System.Serializable]
public class ElementStatVO : PlainStatVO
{
    [JsonConverter(typeof(StringEnumConverter))]
    public ElementEnum name;

    public ElementStatVO(float amount) : base(amount)
    {
    }
}
