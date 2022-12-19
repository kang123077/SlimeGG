using System.Collections.Generic;

[System.Serializable]
public class MonsterInfo
{
    public string nickName;
    public GrowthState curGrowthState; // generator용 임시?
    public string monsterName; // generator용 임시?
    // public int curGrowthStateIdx를 growthStateInfo의 idx로 쓰는게 나을지도..?
    public GrowthStateInfo curGrowthStateInfo;
    public GrowthStateInfo[] growthStateInfo;
    public StatInfo growValue; // 성장치
    public StatInfo stat; // 최종 스탯? growthStateInfo.speciesInfo.stat + growValue
    // 현재 올라간 타일셋의 정보를 담는 공간이 있을지도?

    // Generator용 Constructor
    public MonsterInfo(string nickName, GrowthState curGrowthState, string monsterName)
    {
        this.nickName = nickName;
        this.curGrowthState = curGrowthState;
        this.monsterName = monsterName;
    }

    // speciesName만 받아서 초기화 하는 Constructor 예상
    public MonsterInfo(string nickName, SpeciesName speciesName)
    {
        this.nickName = nickName;
        this.curGrowthStateInfo.speciesInfo.Name = speciesName;
    }
}