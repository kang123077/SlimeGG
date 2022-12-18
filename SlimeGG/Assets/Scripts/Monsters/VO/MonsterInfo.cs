using System.Collections.Generic;

[System.Serializable]
public class MonsterInfo
{
    public string nickName;
    public GrowthState curGrowthState;
    public Dictionary<GrowthState, SpeciesInfo> speciesInfo;
    public string monsterName; // юс╫ц

    public MonsterInfo(GrowthState curGrowthState, string monsterName)
    {
        this.curGrowthState = curGrowthState;
        this.monsterName = monsterName;
    }
}