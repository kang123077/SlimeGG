using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkillExecutor
{
    public static List<int> checkAvailable(SkillStat skillToUse, MonsterBattleController caster)
    {
        switch (skillToUse.skilType)
        {
            case MonsterSkillTypeEnum.ATTACK_NORMAL:
                return checkEnemyInRangeClosest(skillToUse, caster);
            case MonsterSkillTypeEnum.ATTACK_DASH:
                return checkEnemyInRangeClosest(skillToUse, caster);
            case MonsterSkillTypeEnum.ATTACK_ASSASSIN:
                return checkEnemyInRangeFarthest(skillToUse, caster);
            case MonsterSkillTypeEnum.HEAL:
                return checkAllyClosest(skillToUse, caster);
        }
        return false;
    }

    /** 0. 적들
     *  1. 사정거리 내
     *  2. 가장 가까운 순서
     */
    public static List<int> checkEnemyInRangeClosest(SkillStat skillToUse, MonsterBattleController caster)
    {
        List<int> res = new List<int>();
        for (int j = 0; j < caster.distanceEnemies.Length; j++)
        {
            if (res.Count == skillToUse.numberOfTarget)
                return res;
            if (skillToUse.range >= caster.distanceEnemies[j])
            {
                res.Add(j);
            }
        }
        return res;
    }

    /** 0. 적들
     *  1. 사정거리 내
     *  2. 가장 먼 순서
     */
    public static List<int> checkEnemyInRangeFarthest(SkillStat skillToUse, MonsterBattleController caster)
    {
        List<int> res = new List<int>();
        for (int j = 0; j < caster.distanceEnemies.Length; j++)
        {
            if (res.Count == skillToUse.numberOfTarget)
                return res;
            if (skillToUse.range >= caster.distanceEnemies[j])
            {
                res.Add(j);
            }
        }
        return res;
    }
    public static int[] checkEnemyFarthest(SkillStat skillToUse, MonsterBattleController caster)
    {
        bool res = false;
        for (int i = 0; i < skillToUse.numberOfTarget; i++)
        {
            if (skillToUse.range >= caster.distanceEnemies[i])
            {
                res = true;
            }
        }
        return res;
    }
    public static int[] checkAllyClosest(SkillStat skillToUse, MonsterBattleController caster)
    {
        return false;
    }

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