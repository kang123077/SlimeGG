using System.Collections.Generic;

[System.Serializable]
public class MonsterInfo
{
    public string nickName;
    public GrowthState curGrowthState; // generator�� �ӽ�?
    public string monsterName; // generator�� �ӽ�?
    // public int curGrowthStateIdx�� growthStateInfo�� idx�� ���°� ��������..?
    public GrowthStateInfo curGrowthStateInfo;
    public GrowthStateInfo[] growthStateInfo;
    public StatInfo growValue; // ����ġ
    public StatInfo stat; // ���� ����? growthStateInfo.speciesInfo.stat + growValue
    // ���� �ö� Ÿ�ϼ��� ������ ��� ������ ��������?

    // Generator�� Constructor
    public MonsterInfo(string nickName, GrowthState curGrowthState, string monsterName)
    {
        this.nickName = nickName;
        this.curGrowthState = curGrowthState;
        this.monsterName = monsterName;
    }

    // speciesName�� �޾Ƽ� �ʱ�ȭ �ϴ� Constructor ����
    public MonsterInfo(string nickName, SpeciesName speciesName)
    {
        this.nickName = nickName;
        this.curGrowthStateInfo.speciesInfo.Name = speciesName;
    }
}