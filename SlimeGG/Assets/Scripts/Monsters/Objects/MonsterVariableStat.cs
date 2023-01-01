using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


[System.Serializable]
public class MonsterVariableStat
{
    [JsonConverter(typeof(StringEnumConverter))]
    public MonsterVariableEnum name;
    public float amount;
    public bool isMultiple;

    public MonsterVariableStat(MonsterVariableEnum name, float amount, bool isMultiple)
    {
        this.name = name;
        this.amount = amount;
        this.isMultiple = isMultiple;
    }
}