using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConditionHandlingExecutor
{
    public static void execute(SkillStat skillStat, MonsterBattleController caster, MonsterBattleController[] targetList)
    {
        switch (skillStat.effect.type)
        {
            case ConditionHandlingTypeEnum.None:
                controllBasicStat(
                    new MonsterVariableStat(
                        MonsterVariableEnum.hp,
                        skillStat.effect.amount,
                        skillStat.effect.isMultiple
                        ),
                    caster,
                    targetList);
                controllBasicStat(
                    new MonsterVariableStat(
                        MonsterVariableEnum.spd,
                        -0.5f,
                        false
                        ),
                    caster,
                    targetList);
                break;
            case ConditionHandlingTypeEnum.Stun:
                controllBasicStat(
                    new MonsterVariableStat(
                        MonsterVariableEnum.hp,
                        skillStat.effect.amount,
                        skillStat.effect.isMultiple
                        ),
                    caster,
                    targetList);
                controllPosition(
                    -3f,
                    targetList,
                    caster
                    );
                break;
            case ConditionHandlingTypeEnum.Poison:
                break;
            case ConditionHandlingTypeEnum.Burn:
                break;
            case ConditionHandlingTypeEnum.Paralysis:
                break;
            case ConditionHandlingTypeEnum.Slow:
                break;
            case ConditionHandlingTypeEnum.Dash:
                break;
            case ConditionHandlingTypeEnum.Blink:
                break;
        }
        activateHitEffect(skillStat.skillType, targetList);
    }

    private static void controllBasicStat(MonsterVariableStat targetStat, MonsterBattleController caster, MonsterBattleController[] targetList)
    {
        foreach (MonsterBattleController target in targetList)
        {
            float res =
                targetStat.name != MonsterVariableEnum.hp ?
                targetStat.amount :
                calculateDemage(targetStat.amount, caster.monsterInfo, target.monsterInfo);
            if (!targetStat.isMultiple)
            {
                target.monsterInfo.basicDict[targetStat.name].amount += res;
            }
            else
            {
                target.monsterInfo.basicDict[targetStat.name].amount *= res;
            }
        }
    }

    private static void controllPosition(float amount, MonsterBattleController[] oneToMoveList, MonsterBattleController target)
    {
        foreach (MonsterBattleController oneToMove in oneToMoveList)
        {
            Vector3 movPos = oneToMove.transform.position;
            Vector3 tarPos = target.transform.position;
            oneToMove.extraMovement += Vector3.Normalize(tarPos - movPos) * amount;
        }
    }

    private static float calculateDemage(float baseAmount, MonsterInfo casterInfo, MonsterInfo targetInfo)
    {
        if (baseAmount == 0f) return 0f;
        float res = baseAmount * casterInfo.basicDict[MonsterVariableEnum.atk].amount;
        res *= 100f / (100f + targetInfo.basicDict[MonsterVariableEnum.def].amount);
        // 속성 계산 필요
        return res;
    }

    private static void activateHitEffect(MonsterSkillTypeEnum skillType, MonsterBattleController[] targetList)
    {
        foreach (MonsterBattleController target in targetList)
        {
            target.activateHitEffect(skillType, false);
        }
    }
}