using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFarmInfo
{
    public string nickName { get; set; }
    public Dictionary<BasicStatEnum, PlainStatVO> basic { get; set; }
    public Dictionary<ElementEnum, PlainStatVO> element { get; set; }
    public Dictionary<string, SkillStat> skills { get; set; }
    public List<string> accuSpecies { get; set; }
    public string src { get; set; }
    public int installedPosition { get; set; }

    public MonsterFarmInfo()
    {
        nickName = string.Empty;
        basic = new Dictionary<BasicStatEnum, PlainStatVO>();
        element = new Dictionary<ElementEnum, PlainStatVO>();
        skills = new Dictionary<string, SkillStat>();
        accuSpecies = new List<string>();
        src = string.Empty;
        installedPosition = 0;
    }
}
