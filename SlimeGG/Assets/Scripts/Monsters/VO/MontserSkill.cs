using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkill
{
    public MonsterSkillName name;
    public int damage;
    public int cooltime;
    public int multiplier;
    Dictionary<MonsterType, bool> Type;
}
