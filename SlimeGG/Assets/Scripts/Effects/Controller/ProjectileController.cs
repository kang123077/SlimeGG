using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private float spd;
    private MonsterBattleController caster { get; set; }
    private MonsterBattleController target { get; set; }
    private EffectBundleStat effectOnHit { get; set; }
    private EffectAreaStat effectAura { get; set; }
    private Vector3 targetPos { get; set; }
    private bool isTargeting = true;
    private float delayTime = -1f;
    private int targetSide { get; set; }

    private Vector3 directiontoward { get; set; }

    private Animator anim { get; set; }
    private static string resourcePath = "Effects/Attacks/";

    // Update is called once per frame
    void Update()
    {
        // 전투 정지인지 아닌지
        if (!LocalStorage.IS_GAME_PAUSE && !LocalStorage.IS_BATTLE_FINISH)
        {
            // 전투 종료인지
            if (LocalStorage.IS_BATTLE_FINISH)
            {
                Destroy(transform.gameObject);
            }
            // 데이터 로딩이 다 되었는지
            if (delayTime <= 0f)
            {
                // 도착했는지
                if (isArrived())
                {
                    // 피격 이벤트 발동 후 투사체 파괴
                    handleArrival();
                    Destroy(transform.gameObject);
                    return;
                }
                else
                {
                    // 이동
                    moveTo();
                }
            }
            else
            {
                delayTime -= Time.deltaTime;
            }
        }
    }

    private bool isArrived()
    {
        return Vector3.Distance((isTargeting ? target.transform.position : targetPos), transform.position) < 0.25f;
    }

    private void moveTo()
    {
        directiontoward = Vector3.Normalize((isTargeting ? target.transform.position : targetPos) - transform.position);
        transform.Translate(
            directiontoward * spd * Time.deltaTime
            );
        if (anim != null)
        {
            anim.SetFloat("DirectionX",
                directiontoward.x > 0f ? 1f : -1f
            );
        }
    }

    public void initInfo(
        ProjectileStat projectileStat,
        float delay,
        MonsterBattleController casterController,
        MonsterBattleController targetController,
        int targetSide
        )
    {
        caster = casterController;
        if (projectileStat.src != null)
        {
            anim = GetComponent<Animator>();
            anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(
                PathInfo.ANIMATION + resourcePath + projectileStat.src + "/Controller");
        }

        if (projectileStat.rgb != null)
        {
            Color temp;
            ColorUtility.TryParseHtmlString(projectileStat.rgb, out temp);
            GetComponent<SpriteRenderer>().color = temp;
        }

        spd = projectileStat.spd;
        isTargeting = projectileStat.isTarget;
        delayTime = Mathf.Max(delay, 0);
        if (!(isTargeting = projectileStat.isTarget))
        {
            targetPos = targetController.transform.position;
        }
        else
        {
            target = targetController;
        }
        effectOnHit = projectileStat.effects;
        effectAura = projectileStat.area;
        this.targetSide = targetSide;
    }

    private void handleArrival()
    {
        handleArrivalEffects();
        handleAruaEffects();
    }

    // 도착과 동시 발생 효과 처리
    private void handleArrivalEffects()
    {
        if (effectOnHit != null)
        {
            if (effectOnHit.instant != null)
                applyEffectListOnTarget(effectOnHit.instant, target, true);
            if (effectOnHit.sustain != null)
                applyEffectListOnTarget(effectOnHit.sustain, target, true);
        }
    }

    // 도착과 동시 장판 생성
    private void handleAruaEffects()
    {
        if (effectAura != null)
        {
            // 현재 기준 장판 영역 내 타겟들에게 즉발 효과 적용
            int[] targetIdxList = BattleManager.getTargetIdxListByDistance(transform.position, targetSide, effectAura.range);
            MonsterBattleController[] temp = BattleManager.monsterBattleControllerList[targetSide];
            foreach (int targetIdx in targetIdxList)
            {
                applyEffectListOnTarget(effectAura.effects.instant, temp[targetIdx], false);
            }
            // 장판 생성
            BattleManager.executeArea(transform.position, caster, effectAura, targetSide);
        }
    }

    // 타겟 컨트롤러에 효과 적용
    private void applyEffectOnTarget(EffectStat effectToApply, MonsterBattleController targetToApply, bool isDirectionSustain)
    {
        EffectStat temp = new EffectStat(effectToApply);
        if (temp.name == BasicStatEnum.hp)
        {
            temp.amount = BattleManager.calculateDamage(temp.amount, caster.liveBattleInfo, targetToApply.liveBattleInfo);
        }
        if (temp.name == BasicStatEnum.position)
        {
            temp.directionWithPower =
                MonsterCommonFunction.translatePositionPowerToVector3(
                    isDirectionSustain ? directiontoward : Vector3.Normalize(targetToApply.transform.position - transform.position),
                    temp.amount
                    );
        }
        targetToApply.effects.Add(temp);
    }

    private void applyEffectListOnTarget(List<EffectStat> effectListToApply, MonsterBattleController targetToApply, bool isDirectionSustain)
    {
        foreach (EffectStat effect in effectListToApply)
        {
            applyEffectOnTarget(effect, targetToApply, isDirectionSustain);
        }
    }



    //private void calculateDamage()
    //{
    //    float res = skillStat.amount;
    //    res *= isCrit ? 1.5f : 1f;
    //    res *= 100f / (100f + target.def);

    //    List<ElementEnum> elementsRelated = new List<ElementEnum>();
    //    elementsRelated.AddRange(caster.speciesInfo.elements);
    //    elementsRelated.AddRange(target.speciesInfo.elements);
    //    elementsRelated = elementsRelated.Distinct().ToList();
    //    float targetSum = 10f;
    //    float casterAccu = 0f, targetAccu = 0f;
    //    foreach (ElementEnum element in elementsRelated)
    //    {
    //        int idx;
    //        if ((idx = caster.speciesInfo.elements.IndexOf(element)) != -1)
    //        {
    //            casterAccu += caster.monsterInfo.stats[idx];
    //            casterAccu += caster.speciesInfo.stats[idx];
    //        }
    //        if ((idx = target.speciesInfo.elements.IndexOf(element)) != -1)
    //        {
    //            targetAccu += target.monsterInfo.stats[idx];
    //            targetAccu += target.speciesInfo.stats[idx];
    //        }
    //    }
    //    if (targetAccu > casterAccu)
    //    {
    //        targetSum += targetAccu - casterAccu;
    //    }

    //    res *= Mathf.Pow(Mathf.Log10(targetSum), 0.5f);
    //    res = Mathf.Floor(res);
    //    target.calcHpDamage((int)res);
    //    target.activateHitEffect(skillStat.skillType, isCrit);
    //    if ((target.curHp -= res) <= 0f)
    //    {
    //        target.makeDead();
    //    }
    //}
}
