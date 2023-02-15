using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class BasicStat
{
    [JsonConverter(typeof(StringEnumConverter))]
    public BasicStatEnum name;
    public float amount;
    public bool isMultiple;
    [System.NonSerialized]
    public Vector3 directionWithPower = Vector3.zero;

    public BasicStat()
    {
        amount = 0f;
        isMultiple = false;
    }

    public BasicStat(float amount)
    {
        this.amount = amount;
        isMultiple = false;
    }

    public BasicStat(BasicStat basicStat)
    {
        name = basicStat.name;
        amount = basicStat.amount;
        isMultiple = basicStat.isMultiple;
    }
}
