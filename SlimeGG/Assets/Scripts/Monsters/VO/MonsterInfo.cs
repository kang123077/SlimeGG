[System.Serializable]
public class MonsterInfo
{
    public GrowthState curGrowthState;
    public string monsterName;

    public MonsterInfo(GrowthState curGrowthState, string monsterName)
    {
        this.curGrowthState = curGrowthState;
        this.monsterName = monsterName;
    }
}