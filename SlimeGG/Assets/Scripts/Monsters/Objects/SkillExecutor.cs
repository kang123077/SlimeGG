using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SkillExecutor
{
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
            if (distArr[j] != -1f && skillToUse.range >= distArr[j])
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
            if (arrToUse[j] != -1f) sort[arrToUse[j]] = j;
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
                break;
            case MonsterSkillTypeEnum.ATTACK_DASH:
                controllPosition(5f, caster, targetIndexList);
                break;
            case MonsterSkillTypeEnum.ATTACK_ASSASSIN:
                break;
            case MonsterSkillTypeEnum.HEAL:
                break;
        }
        createProjectile(skillToUse, caster, targetIndexList);
    }

    private static void controllPosition(float amount, MonsterBattleController caster, List<int> targetIndexList)
    {
        Vector3 movPos = caster.transform.position;
        Vector3 tarPos = caster.enemies[targetIndexList[0]].transform.position;
        caster.extraMovement += Vector3.Normalize(tarPos - movPos) * amount * 3f;
    }

    private static void createProjectile(SkillStat skillStat, MonsterBattleController caster, List<int> targetIndexList)
    {
        for (int i = 0; i < skillStat.count; i++)
        {
            if (i < targetIndexList.Count)
            {
                GameObject bullet = caster.generateBullet();
                bullet.GetComponent<ProjectileController>().initInfo(
                    skillStat, 
                    caster, 
                    caster.enemies[targetIndexList[i]].GetComponent<MonsterBattleController>()
                    );
            } else
            {
                return;
            }
        }
    }
}