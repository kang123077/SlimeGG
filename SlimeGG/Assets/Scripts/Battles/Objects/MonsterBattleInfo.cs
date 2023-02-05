using System.Collections.Generic;

public class MonsterBattleInfo
{
    public Dictionary<BasicStatEnum, PlainStat> basic { get; set; }

    public List<ElementEnum> element { get; set; }
    public Dictionary<string, SkillStat> skills { get; set; }
    public int[] entryPos { get; set; }
    public string src { get; set; }

    public MonsterBattleInfo()
    {
        basic = new Dictionary<BasicStatEnum, PlainStat>();
        element = new List<ElementEnum>();
        skills = new Dictionary<string, SkillStat>();
        src = string.Empty;
    }

    public MonsterBattleInfo(MonsterBattleInfo monsterBattleInfo)
    {
        element = monsterBattleInfo.element;
        skills = monsterBattleInfo.skills;
        basic = monsterBattleInfo.basic;
        element = monsterBattleInfo.element;
        basic[BasicStatEnum.timeCastingCycle] = new PlainStat();
        basic[BasicStatEnum.timeCoolCycle] = new PlainStat();
        basic[BasicStatEnum.invincible] = new PlainStat();
        basic[BasicStatEnum.position] = new PlainStat();
    }
}
