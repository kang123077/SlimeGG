using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private SkillStat skillStat;
    private MonsterBattleController caster { get; set; }
    private MonsterBattleController target { get; set; }
    private float speed;

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
            if (Vector3.Distance(transform.position, target.transform.position) < 0.25f)
            {
                calculateDamage();
                Destroy(transform.gameObject);
                return;
            }
            else
            {
                transform.Translate(
                    Vector3.Normalize(target.transform.position - transform.position) * speed * Time.deltaTime
                    );
                if (anim != null)
                {
                    anim.SetFloat("DirectionX",
                        (target.transform.position.x - transform.position.x) > 0f ? 1f : -1f
                    );
                }
            }
        }
    }

    public void initInfo(
        SkillStat skillStat,
        MonsterBattleController caster,
        MonsterBattleController target,
        float speed)
    {
        this.skillStat = skillStat;
        this.caster = caster;
        this.target = target;
        this.speed = speed <= 0f ? 100 : speed;
        if (skillStat.resourcePath != null)
        {
            anim = GetComponent<Animator>();
            anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(
                PathInfo.ANIMATION + skillStat.resourcePath + "/Controller");
        }
    }

    private void calculateDamage()
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
        target.calcHpDamage((int)res);
        target.activateHitEffect(skillStat.skillType, isCrit);
        if ((target.curHp -= res) <= 0f)
        {
            target.makeDead();
        }
    }
}
