using System.Collections.Generic;

[System.Serializable]
public class MonsterSpeciesVO
{
    public List<BasicStatVO> basic { get; set; }
    public List<ElementStatVO> element { get; set; }
    public List<string> skills { get; set; }
    public string src { get; set; }
}