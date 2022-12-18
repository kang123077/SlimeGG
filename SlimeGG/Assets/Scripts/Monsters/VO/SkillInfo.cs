using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillInfo
{
    public SkillName name;
    public int damage;
    public int cooltime;
    Dictionary<Type, float> type;

    public void printData()
    {
        Debug.Log("SkillName : " + name);
        Debug.Log("Damage : " + damage);
        Debug.Log("Cooltime : " + cooltime);
        Debug.Log("Type : " + damage);
    }
}

/*
public class DictionaryByGrowState
{
    [SerializeField] public GrowthState growthState;

    [SerializeField] public List<MonsterInfo> monsterInfos;
}
*/