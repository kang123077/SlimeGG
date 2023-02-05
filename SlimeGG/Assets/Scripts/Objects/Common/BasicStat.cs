using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class BasicStat
{
    public BasicStatEnum name;
    public float amount;
    public bool isMultiple;

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
}
