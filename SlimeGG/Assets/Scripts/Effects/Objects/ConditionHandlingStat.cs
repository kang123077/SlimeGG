using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[SerializeField]
public class ConditionHandlingStat
{
    [JsonConverter(typeof(StringEnumConverter))]
    public ConditionHandlingTypeEnum type;
    public bool isMultiple;
    public float amount;
}
