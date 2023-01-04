using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class BasicStatVO : PlainStatVO
{
    [JsonConverter(typeof(StringEnumConverter))]
    public BasicStatEnum name;

    public Vector3 directionWithPower = Vector3.zero;

    public BasicStatVO(): base()
    {
    }

    public BasicStatVO(float amount) : base(amount)
    {
    }
}
