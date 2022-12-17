using System.Collections.Generic;

public class SpeciesInfo
{
    /**
     * 
     * name : Insect,
     * basictype :
     * { insect, 20 }
     * { glass, 15 },
     * 
     */
    public MonsterSpecies speciesName;
    public Dictionary<MonsterType, int> basicType = new Dictionary<MonsterType, int>(); 

    public SpeciesInfo(MonsterSpecies speciesName)
    {

    }
}