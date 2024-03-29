using System.Collections.Generic;

public class MonsterBattleInfo
{
    public Dictionary<BasicStatEnum, BasicStat> basic { get; set; }

    public List<ElementEnum> element { get; set; }
    public Dictionary<string, SkillStat> skills { get; set; }
    public int[] entryPos { get; set; }
    public string speicie { get; set; }

    public MonsterBattleInfo()
    {
        basic = new Dictionary<BasicStatEnum, BasicStat>();
        element = new List<ElementEnum>();
        skills = new Dictionary<string, SkillStat>();
        speicie = string.Empty;
    }

    public MonsterBattleInfo(MonsterBattleInfo monsterBattleInfo)
    {
        element = monsterBattleInfo.element;
        skills = monsterBattleInfo.skills;
        basic = monsterBattleInfo.basic;
        element = monsterBattleInfo.element;
        speicie = monsterBattleInfo.speicie;
        // basic[BasicStatEnum.timeCastingCycle] = new BasicStat();
        // basic[BasicStatEnum.timeCoolCycle] = new BasicStat();
        basic[BasicStatEnum.invincible] = new BasicStat();
        basic[BasicStatEnum.position] = new BasicStat();
    }
}
