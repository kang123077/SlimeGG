using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private SkillStat skillStat;
    private ProjectileTypeEnum projectileType;
    private MonsterBattleController caster { get; set; }
    private MonsterBattleController target { get; set; }
    private Vector3 targetPos;
    private bool isTargeting;
    private float installTime = 0f;
    private float tickTime = 0f;

    private Animator anim { get; set; }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target != null && !LocalStorage.IS_GAME_PAUSE && !LocalStorage.IS_BATTLE_FINISH)
        {
            if (LocalStorage.IS_BATTLE_FINISH)
            {
                Destroy(transform.gameObject);
            }
            if (isArrived())
            {
                handleArrival();
                return;
            }
            else
            {
                moveTo();
            }
        }
    }

    private bool isArrived()
    {
        return Vector3.Distance((isTargeting ? transform.position : targetPos), target.transform.position) < 0.25f;
    }

    private void moveTo()
    {
        //transform.Translate(
        //    Vector3.Normalize((isTargeting ? target.transform.position : targetPos) - transform.position) * skillStat.speed * Time.deltaTime
        //    );
        //if (anim != null)
        //{
        //    anim.SetFloat("DirectionX",
        //        (target.transform.position.x - transform.position.x) > 0f ? 1f : -1f
        //    );
        //}
    }

    public void initInfo(
        SkillStat skillStat,
        Vector2 entryNum,
        int target
        )
    {
        //this.skillStat = skillStat;
        //this.caster = caster;
        //projectileType = skillStat.projectileType;
        //if (skillStat.resourcePath != null)
        //{
        //    anim = GetComponent<Animator>();
        //    anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(
        //        PathInfo.ANIMATION + skillStat.resourcePath + "/Controller");
        //}
        //switch (projectileType)
        //{
        //    case ProjectileTypeEnum.Bullet:
        //    case ProjectileTypeEnum.Explosive:
        //    case ProjectileTypeEnum.Aura:
        //        isTargeting = true;
        //        this.target = target;
        //        break;
        //    case ProjectileTypeEnum.Area:
        //        isTargeting = false;
        //        targetPos = target.transform.position;
        //        break;
        //}
    }

    private void handleArrival()
    {
        switch (projectileType)
        {
            case ProjectileTypeEnum.Bullet:
            case ProjectileTypeEnum.Explosive:
                // 소멸
                handleCondition();
                Destroy(transform.gameObject);
                break;
            case ProjectileTypeEnum.Aura:
                // 계속 추적
                handleCondition();
                break;
            case ProjectileTypeEnum.Area:
                // 설치
                isTargeting = false;
                handleCondition();
                break;
        }
    }

    private void handleCondition()
    {
        //switch (projectileType)
        //{
        //    case ProjectileTypeEnum.Bullet:
        //        ConditionHandlingExecutor.execute(skillStat, caster, new MonsterBattleController[] { target });
        //        Destroy(transform.gameObject);
        //        return;
        //    case ProjectileTypeEnum.Explosive:
        //        ConditionHandlingExecutor.execute(skillStat, caster, identifyTargetList());
        //        Destroy(transform.gameObject);
        //        return;
        //    case ProjectileTypeEnum.Aura:
        //    case ProjectileTypeEnum.Area:
        //        tickTime += Time.deltaTime;
        //        if (tickTime >= 0f)
        //        {
        //            ConditionHandlingExecutor.execute(skillStat, caster, identifyTargetList());
        //            tickTime -= skillStat.delayTime;
        //        }
        //        installTime += Time.deltaTime;
        //        if (installTime >= skillStat.durationTime)
        //        {
        //            Destroy(transform.gameObject);
        //        }
        //        break;
        //}
    }

    private MonsterBattleController[] identifyTargetList()
    {
        List<MonsterBattleController> res = new List<MonsterBattleController>();
        //foreach (Transform curTargetTf in caster.enemies)
        //{
        //    if (Vector3.Distance(transform.position, curTargetTf.position) <= skillStat.durationTime)
        //    {
        //        res.Add(curTargetTf.GetComponent<MonsterBattleController>());
        //    }
        //}
        return res.ToArray();
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
