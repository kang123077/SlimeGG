using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class MonsterSpeciesInfo : MonsterAbilityStat
{
    public MonsterSpeciesEnum species { get; set; }
    public MonsterStageEnum stage { get; set; }
    public List<ElementEnum> elements { get; set; }
    public List<MonsterSkillEnum> skills { get; set; }
    public string resourcePath { get; set; }

    public MonsterSpeciesInfo(
        string species, string stage, List<string> elements, List<string> skills,
        int hp, int def, int spd, List<float> stats)
    {
        this.species = CommonFunctions.convertEnumFromString<MonsterSpeciesEnum>(species);
        this.stage = CommonFunctions.convertEnumFromString<MonsterStageEnum>(stage);
        this.elements = CommonFunctions.convertEnumFromStringArr<ElementEnum>(elements);
        this.skills = CommonFunctions.convertEnumFromStringArr<MonsterSkillEnum>(skills);
        this.hp = hp;
        this.def = def;
        this.spd = spd;
        this.stats = stats;
        resourcePath = "Monsters/" + stage.ToString() + "/" + species.ToString();
    }
    override public string ToString()
    {
        return $"\n" +
            $"{species.ToString()}\n" +
            $"{stage.ToString()}\n" +
            $"{elements.ToString()}\n" +
            $"{stats.ToString()}\n" +
            $"{skills.ToString()}\n"
            ;
    }

}