using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkillExecutor
{
    public static void execute(SkillStat skillToUse, MonsterBattleController caster)
    {
        switch (skillToUse.skilType)
        {
            case MonsterSkillTypeEnum.ATTACK_NORMAL:
                attackNormal(skillToUse, caster);
                break;
            case MonsterSkillTypeEnum.ATTACK_DASH:
                attackDash(skillToUse, caster);
                break;
            case MonsterSkillTypeEnum.ATTACK_ASSASSIN:
                attackAssassin(skillToUse, caster);
                break;
            case MonsterSkillTypeEnum.HEAL:
                healNormal(skillToUse, caster);
                break;
        }
    }

    private static void attackNormal(SkillStat skillStat, MonsterBattleController caster)
    {

    }

    private static void attackDash(SkillStat skillStat, MonsterBattleController caster)
    {

    }

    private static void attackAssassin(SkillStat skillStat, MonsterBattleController caster)
    {

    }

    private static void healNormal(SkillStat skillStat, MonsterBattleController caster)
    {

    }
}