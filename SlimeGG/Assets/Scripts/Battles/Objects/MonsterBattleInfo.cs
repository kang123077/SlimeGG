using System.Collections.Generic;

public class MonsterBattleInfo
{
    public string nickName { get; set; }
    public Dictionary<BasicStatEnum, PlainStatVO> basic { get; set; }
    public Dictionary<ElementEnum, PlainStatVO> element { get; set; }
    public Dictionary<string, SkillStat> skills { get; set; }
    public int[] entryPos { get; set; }
    public string src { get; set; }

    public MonsterBattleInfo()
    {
        nickName = string.Empty;
        basic = new Dictionary<BasicStatEnum, PlainStatVO>();
        element = new Dictionary<ElementEnum, PlainStatVO>();
        skills = new Dictionary<string, SkillStat>();
        src = string.Empty;
    }

    public MonsterBattleInfo(MonsterBattleInfo monsterBattleInfo)
    {
        nickName = monsterBattleInfo.nickName;
        element = monsterBattleInfo.element;
        skills = monsterBattleInfo.skills;
        basic = monsterBattleInfo.basic;
        element = monsterBattleInfo.element;
        basic[BasicStatEnum.timeCastingCycle] = new PlainStatVO();
        basic[BasicStatEnum.timeCoolCycle] = new PlainStatVO();
        basic[BasicStatEnum.invincible] = new PlainStatVO();
        basic[BasicStatEnum.position] = new PlainStatVO();
    }
}
