using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
        float knockBackRate = skillStat.amount * Random.Range(0.5f, 1f);
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
                            (targetTf.localPosition.x - caster.transform.localPosition.x) * Random.Range(1f, 10f),
                            (targetTf.localPosition.y - caster.transform.localPosition.y) * Random.Range(1f, 10f),
                            0f
                            )
                        ) * knockBackRate;
                calculateDamage(skillStat, caster, targetTf.GetComponent<MonsterBattleController>());
            }
            else
            {
                break;
            }
        }
    }

    private static void attackDash(SkillStat skillStat, MonsterBattleController caster, List<int> targetIndexList)
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
                            (targetTf.localPosition.x - caster.transform.localPosition.x) * Random.Range(1f, 10f),
                            (targetTf.localPosition.y - caster.transform.localPosition.y) * Random.Range(1f, 10f),
                            0f
                            )
                        ) * knockBackRate / 2f;
                caster.curDash =
                    Vector3.Normalize(
                        new Vector3(
                            (targetTf.localPosition.x - caster.transform.localPosition.x) * Random.Range(1f, 10f),
                            (targetTf.localPosition.y - caster.transform.localPosition.y) * Random.Range(1f, 10f),
                            0f
                            )
                        ) * knockBackRate * 2;
                calculateDamage(skillStat, caster, targetTf.GetComponent<MonsterBattleController>());
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

    private static void calculateDamage(SkillStat skillStat, MonsterBattleController caster, MonsterBattleController target)
    {
        bool isCrit = Random.Range(0f, 1f) > 0.9f;
        float res = skillStat.amount;
        res *= isCrit ? 1.5f : 1f;
        res *= 100f / (100f + target.def);

        List<ElementEnum> elementsRelated = new List<ElementEnum>();
        elementsRelated.AddRange(caster.speciesInfo.elements);
        elementsRelated.AddRange(target.speciesInfo.elements);
        elementsRelated = elementsRelated.Distinct().ToList();
        float targetSum = 10f;
        float casterAccu = 0f, targetAccu = 0f;
        foreach (ElementEnum element in elementsRelated)
        {
            int idx;
            if ((idx = caster.speciesInfo.elements.IndexOf(element)) != -1)
            {
                casterAccu += caster.monsterInfo.stats[idx];
                casterAccu += caster.speciesInfo.stats[idx];
            }
            if ((idx = target.speciesInfo.elements.IndexOf(element)) != -1)
            {
                targetAccu += target.monsterInfo.stats[idx];
                targetAccu += target.speciesInfo.stats[idx];
            }
        }
        if (targetAccu > casterAccu)
        {
            targetSum += targetAccu - casterAccu;
        }

        res *= Mathf.Pow(Mathf.Log10(targetSum), 0.5f);
        res = Mathf.Floor(res);
        target.calcHpDamage((int) res);
        target.anim.SetInteger("BehaviorState", 2);
        if ((target.curHp -= res) <= 0f)
        {
            target.makeDead();
        }
    }
}