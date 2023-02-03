using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[System.Serializable]
public class MonsterVO : MonsterSpeciesVO
{
    public string specie { get; set; }
    public int[] entryPos { get; set; }
}
