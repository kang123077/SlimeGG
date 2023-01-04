using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaController : MonoBehaviour
{
    private MonsterBattleController caster { get; set; }
    private MonsterBattleController target { get; set; }
    private List<EffectStat> effectOnSustain { get; set; }
    private bool isTarget { get; set; }
    private int targetSide { get; set; }
    private float spd { get; set; }
    private float range { get; set; }
    private float duration { get; set; }

    private static string resourcePath = "Effects/Areas/";
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (duration > 0f)
        {
            // 현재 장판 위 타겟들 효과 적용
            // 이동 가능 시 이동
            // 시간 감기
            passTime();
            handleEffects();
        } else
        {
            Destroy(gameObject);
        }
    }

    public void initInfo(
        EffectAreaStat areaStat,
        MonsterBattleController caster,
        int targetSide)
    {
        this.caster = caster;
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(PathInfo.SPRITE + resourcePath + (areaStat.src != null ? areaStat.src : "Normal"));
        if (isTarget = areaStat.isTarget)
        {
            target = BattleManager.monsterBattleControllerList[targetSide][BattleManager.getClosestIndex(caster.getSide())[targetSide]];
            spd = areaStat.spd;
        }
        range = areaStat.range;
        duration = areaStat.duration;
        this.targetSide = targetSide;
        effectOnSustain = areaStat.effects.sustain;
        foreach (EffectStat effect in effectOnSustain)
        {
            effect.tickTimer = effect.tickTime;
        }
        transform.localScale = Vector3.one * range;
    }

    private void passTime()
    {
        duration -= Time.deltaTime;

        foreach (EffectStat effect in effectOnSustain)
        {
            effect.tickTimer -= Time.deltaTime;
        }
    }

    private void handleEffects()
    {
        List<MonsterBattleController> targetList = new List<MonsterBattleController>();
        MonsterBattleController[] temp = BattleManager.monsterBattleControllerList[targetSide];
        foreach (int idx in BattleManager.getTargetIdxListByDistance(transform.position, targetSide, range))
        {
            targetList.Add(temp[idx]);
        }
        foreach (EffectStat effect in effectOnSustain)
        {
            if (effect.tickTimer <= 0f)
            {
                effect.tickTimer = effect.tickTime;
                applyEffectToTargetList(effect, targetList);
            }
        }
    }

    private void applyEffectToTargetList(EffectStat effect, List<MonsterBattleController> targetList)
    {
        foreach (MonsterBattleController targetToApply in targetList)
        {
            applyEffectOnTarget(effect, targetToApply);
        }
    }

    // 타겟 컨트롤러에 효과 적용
    private void applyEffectOnTarget(EffectStat effectToApply, MonsterBattleController targetToApply)
    {
        if (effectToApply.name == BasicStatEnum.position)
        {
            effectToApply.directionWithPower =
                MonsterCommonFunction.translatePositionPowerToVector3(
                    Vector3.Normalize(targetToApply.transform.position - transform.position),
                    effectToApply.amount
                    );
        }
        targetToApply.effects.Add(new EffectStat(effectToApply, true));
    }
}
