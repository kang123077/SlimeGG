using System.Collections.Generic;

public class MonsterSpeciesInfo : MonsterAbilityStat
{
    MonsterSpeiciesEnum species;
    MonsterStageEnum stage;
    List<ElementEnum> elements;
    List<MonsterSkillEnum> skills;
    string resopurcePath;

    public MonsterSpeciesInfo(MonsterSpeiciesEnum species, MonsterStageEnum stage, List<ElementEnum> elements, List<MonsterSkillEnum> skills)
    {
        this.species = species;
        this.stage = stage;
        this.elements = elements;
        this.skills = skills;
        resopurcePath = "Monsters/" + stage.ToString() + "/" + species.ToString();
    }
}