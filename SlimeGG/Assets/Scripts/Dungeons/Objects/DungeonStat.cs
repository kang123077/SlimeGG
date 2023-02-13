using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DungeonStat
{
    public string name { get; set; }
    public string displayName { get; set; }
    public List<StageSaveStat> stages { get; set; }
}
