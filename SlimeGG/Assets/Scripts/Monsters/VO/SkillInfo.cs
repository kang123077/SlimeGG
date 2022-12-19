using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillInfo
{
    public SkillName name;
    public int damage;
    public int cooltime;
    public int multiflier;
    public TypeSet[] typeList;
}

/*
public class DictionaryByGrowState
{
    [SerializeField] public GrowthState growthState;

    [SerializeField] public List<MonsterInfo> monsterInfos;
}
*/