using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[System.Serializable]
public class MonsterVO : MonsterSpeciesVO
{
    public string nickName { get; set; }
    public int installedPosition { get; set; }
    public List<string> accuSpecies { get; set; }
}
