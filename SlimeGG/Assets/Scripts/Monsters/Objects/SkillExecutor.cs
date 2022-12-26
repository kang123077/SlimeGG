using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SkillExecutor
{

    public static void tryExecuteSkill(SkillStat skillToUse, MonsterBattleController caster)
    {
        if (selectTargetIndexList(skillToUse, caster).Count > 0)
        {

        }
    }
    public static List<int> selectTargetIndexList(SkillStat skillToUse, MonsterBattleController caster)
    {
        switch (skillToUse.skillType)
        {
            case MonsterSkillTypeEnum.ATTACK_NORMAL:
                return checkTargetsInRangeClosest(skillToUse, caster, true);
            case MonsterSkillTypeEnum.ATTACK_DASH:
                return checkTargetsInRangeClosest(skillToUse, caster, true);
            case MonsterSkillTypeEnum.ATTACK_ASSASSIN:
                return checkTargetsInRangeFarthest(skillToUse, caster, true);
            case MonsterSkillTypeEnum.HEAL:
                return checkTargetsInRangeClosest(skillToUse, caster, false);
        }
        return new List<int>();
    }

    /** 0. 타겟들
     *  1. 사정거리 내
     *  2. 가장 가까운 순서
     *  3. 타겟 최대수:: 스킬 최대 타겟 수
     *  4. return:: 해당 타겟들 인덱스 List
     */
    public static List<int> checkTargetsInRangeClosest(
        SkillStat skillToUse,
        MonsterBattleController caster,
        bool isTargetEnemy
        )
    {
        float[] distArr = isTargetEnemy ? caster.distanceEnemies : caster.distanceAllies;
        List<int> res = new List<int>();
        Dictionary<float, int> sort = new Dictionary<float, int>();
        for (int j = 0; j < distArr.Length; j++)
        {
            if (skillToUse.range >= distArr[j])
            {
                sort[distArr[j]] = j;
            }
        }
        foreach (KeyValuePair<float, int> index in sort.OrderBy((i) => i.Key))
        {
            if (res.Count == skillToUse.numberOfTarget) return res;
            res.Add(index.Value);
        }
        return res;
    }


    /** 0. 타겟들
     *  1. 사정거리 내
     *  2. 가장 먼 순서
     *  3. 타겟 최대수:: 스킬 최대 타겟 수
     *  4. return:: 해당 타겟들 인덱스 List
     */
    public static List<int> checkTargetsInRangeFarthest(
        SkillStat skillToUse,
        MonsterBattleController caster,
        bool isTargetEnemy
        )
    {
        List<int> res = checkTargetsInRangeClosest(skillToUse, caster, isTargetEnemy);
        res.Reverse();
        return res;
    }

    /** 0. 타겟들
     *  1. 사정거리 없음
     *  2. 가장 먼 순서
     *  3. 타겟 최대수:: 스킬 최대 타겟 수
     *  4. return:: 해당 타겟들 인덱스 List
     */
    public static List<int> checkTargetsOutRangeClosest(
        SkillStat skillToUse,
        MonsterBattleController caster,
        bool isTargetEnemy
        )
    {
        float[] arrToUse = isTargetEnemy ? caster.distanceEnemies : caster.distanceAllies;
        List<int> res = new List<int>();
        Dictionary<float, int> sort = new Dictionary<float, int>();
        for (int j = 0; j < arrToUse.Length; j++)
        {
            sort[arrToUse[j]] = j;
        }
        foreach (KeyValuePair<float, int> index in sort.OrderBy((i) => i.Key))
        {
            if (res.Count == skillToUse.numberOfTarget) return res;
            res.Add(index.Value);
        }
        return res;
    }

    /** 0. 타겟들
     *  1. 사정거리 없음
     *  2. 가장 가까운 순서
     *  3. 타겟 최대수:: 스킬 최대 타겟 수
     *  4. return:: 해당 타겟들 인덱스 List
     */
    public static List<int> checkTargetsOutRangeFarthest(
        SkillStat skillToUse,
        MonsterBattleController caster,
        bool isTargetEnemy
        )
    {
        List<int> res = checkTargetsOutRangeClosest(skillToUse, caster, isTargetEnemy);
        res.Reverse();
        return res;
    }

    public static void execute(SkillStat skillToUse, MonsterBattleController caster, List<int> targetIndexList)
    {
        switch (skillToUse.skillType)
        {
            case MonsterSkillTypeEnum.ATTACK_NORMAL:
                attackNormal(skillToUse, caster, targetIndexList);
                break;
            case MonsterSkillTypeEnum.ATTACK_DASH:
                attackDash(skillToUse, caster, targetIndexList);
                break;
            case MonsterSkillTypeEnum.ATTACK_ASSASSIN:
                attackAssassin(skillToUse, caster, targetIndexList);
                break;
            case MonsterSkillTypeEnum.HEAL:
                healNormal(skillToUse, caster, targetIndexList);
                break;
        }
    }

    private static void attackNormal(SkillStat skillStat, MonsterBattleController caster, List<int> targetIndexList)
    {
        float knockBackRate = skillStat.amount * Random.Range(1f, 3f);
        int targetCnt = 0;
        foreach (int i in targetIndexList)
        {
            if (targetCnt < skillStat.numberOfTarget)
            {
                targetCnt++;
                Transform targetTf = caster.enemies[i];
                targetTf.GetComponent<MonsterBattleController>().curKnockback =
                    Vector3.Normalize(
                        new Vector3(
                            targetTf.localPosition.x - caster.transform.localPosition.x,
                            targetTf.localPosition.y - caster.transform.localPosition.y,
                            0f
                            )
                        ) * knockBackRate
                    ;
            }
            else
            {
                break;
            }
        }
    }

    private static void attackDash(SkillStat skillStat, MonsterBattleController caster, List<int> targetIndexList)
    {
        float knockBackRate = skillStat.amount * Random.Range(3f, 5f);
        int targetCnt = 0;
        foreach (int i in targetIndexList)
        {
            if (targetCnt < skillStat.numberOfTarget)
            {
                targetCnt++;
                Transform targetTf = caster.enemies[i];
                targetTf.GetComponent<MonsterBattleController>().curKnockback =
                    Vector3.Normalize(
                        new Vector3(
                            targetTf.localPosition.x - caster.transform.localPosition.x,
                            targetTf.localPosition.y - caster.transform.localPosition.y,
                            0f
                            )
                        ) * knockBackRate;
                caster.curDash =
                    Vector3.Normalize(
                        new Vector3(
                            caster.transform.localPosition.x - targetTf.localPosition.x,
                            caster.transform.localPosition.y - targetTf.localPosition.y,
                            0f
                            )
                        ) * knockBackRate;
            }
            else
            {
                break;
            }
        }
    }

    private static void attackAssassin(SkillStat skillStat, MonsterBattleController caster, List<int> targetIndexList)
    {

    }

    private static void healNormal(SkillStat skillStat, MonsterBattleController caster, List<int> targetIndexList)
    {

    }
}