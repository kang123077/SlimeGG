using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFarmInfo
{
    public string nickName { get; set; }
    public Dictionary<BasicStatEnum, PlainStatVO> basic { get; set; }
    public Dictionary<ElementEnum, PlainStatVO> element { get; set; }
    public List<string> accuSpecies { get; set; }
    public MonsterSpeciesVO specieInfo { get; set; }
    public string src { get; set; }
    public int installedPosition { get; set; }

    public MonsterFarmInfo()
    {
        nickName = string.Empty;
        basic = new Dictionary<BasicStatEnum, PlainStatVO>();
        element = new Dictionary<ElementEnum, PlainStatVO>();
        accuSpecies = new List<string>();
        src = string.Empty;
        installedPosition = 0;
    }
}
