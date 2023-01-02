using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SkillExecutor
{
    public static int[] selectTargetIndexList(SkillStat skillToUse, Vector2 entryNum)
    {
        switch (skillToUse.skillType)
        {
            case MonsterSkillTypeEnum.ATTACK_NORMAL:
                return checkTargetsInRangeClosest(skillToUse, entryNum, true);
            case MonsterSkillTypeEnum.ATTACK_DASH:
                return checkTargetsInRangeClosest(skillToUse, entryNum, true);
            case MonsterSkillTypeEnum.ATTACK_ASSASSIN:
                return checkTargetsInRangeFarthest(skillToUse, entryNum, true);
            case MonsterSkillTypeEnum.HEAL:
                return checkTargetsInRangeClosest(skillToUse, entryNum, false);
        }
        return new int[] { };
    }

    /** 0. 타겟들
     *  1. 사정거리 내
     *  2. 가장 가까운 순서
     *  3. 타겟 최대수:: 스킬 최대 타겟 수
     *  4. return:: 해당 타겟들 인덱스 List
     */
    public static int[] checkTargetsInRangeClosest(
        SkillStat skillToUse,
        Vector2 entryNum,
        bool isTargetEnemy
        )
    {
        return FieldController.getIndexListByDistance(entryNum, isTargetEnemy, skillToUse.range);
    }


    /** 0. 타겟들
     *  1. 사정거리 내
     *  2. 가장 먼 순서
     *  3. 타겟 최대수:: 스킬 최대 타겟 수
     *  4. return:: 해당 타겟들 인덱스 List
     */
    public static int[] checkTargetsInRangeFarthest(
        SkillStat skillToUse,
        Vector2 entryNum,
        bool isTargetEnemy
        )
    {
        int[] res = FieldController.getIndexListByDistance(entryNum, isTargetEnemy, skillToUse.range);
        res.Reverse();
        return res;
    }

    public static void execute(SkillStat skillToUse, Vector2 entryNum, List<int> targetIndexList)
    {
        switch (skillToUse.skillType)
        {
            case MonsterSkillTypeEnum.ATTACK_NORMAL:
                break;
            case MonsterSkillTypeEnum.ATTACK_DASH:
                controllPosition(5f, entryNum, targetIndexList);
                break;
            case MonsterSkillTypeEnum.ATTACK_ASSASSIN:
                break;
            case MonsterSkillTypeEnum.HEAL:
                break;
        }
        createProjectile(skillToUse, entryNum, targetIndexList);
    }

    private static void controllPosition(float amount, Vector2 entryNum, List<int> targetIndexList)
    {
        Vector3 movPos = LocalStorage.monsterBattleControllerList[(int)entryNum.x][(int)entryNum.y].transform.position;
        Vector3 tarPos = LocalStorage.monsterBattleControllerList[1 - (int)entryNum.x][targetIndexList[0]].transform.position;
        LocalStorage.monsterBattleControllerList[(int)entryNum.x][(int)entryNum.y].extraMovement += 
            Vector3.Normalize(tarPos - movPos) * amount * 3f;
    }

    private static void createProjectile(SkillStat skillStat, Vector2 entryNum, List<int> targetIndexList)
    {
        for (int i = 0; i < skillStat.count; i++)
        {
            if (i < targetIndexList.Count)
            {
                FieldController.createProjectile(skillStat, entryNum, targetIndexList[i]);
            } else
            {
                return;
            }
        }
    }
}