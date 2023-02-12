using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class MonsterBattleController : MonoBehaviour
{
    private Vector2 entryNum = new Vector2(-1f, -1f);
    public MonsterBattleInfo monsterBattleInfo { get; set; }
    public MonsterBattleInfo liveBattleInfo { get; set; }
    // 부여받은 효과 (디버프, 버프 전부)
    public List<EffectStat> effects { get; set; }

    // ------------ 그래픽 관련 변수들 ------------------
    private Transform bg { get; set; }
    private Animator anim { get; set; }
    private Animator animCasting { get; set; }

    private float animTime = 0f;

    // ------------------------------------------
    private float castingTime = -1f;
    private string targetSkillName { get; set; }
    private int[] targetIdxList { get; set; }
    private float distanceToKeep { get; set; }
    public bool isDead { get; set; }

    private GaugeController hpController { get; set; }

    private int directionDistortion = 0;
    private float timeDistortion = 0f;

    private Vector3 directionToTarget { get; set; }
    private Vector3 extraMovement = Vector3.zero;
    private Vector3 curDirection { get; set; }

    private Transform monsterContainer;
    private Transform entryContainer;

    public void initInfo(MonsterBattleInfo monsterBattleInfo, Transform monsterContainer)
    {
        this.monsterContainer = monsterContainer;
        this.monsterBattleInfo = monsterBattleInfo;
        liveBattleInfo = new MonsterBattleInfo(monsterBattleInfo);
        bg = transform.Find("Image");
        bg.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(
            PathInfo.Monster.Sprite + monsterBattleInfo.speicie
            );

        anim = bg.GetComponent<Animator>();
        anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(
            PathInfo.Monster.Animation + monsterBattleInfo.speicie + "/Controller"
            );
        distanceToKeep = 100f;
        foreach (SkillStat skillStat in monsterBattleInfo.skills.Values)
        {
            skillStat.timeCharging = 0f;
            if (distanceToKeep > skillStat.range) distanceToKeep = Mathf.Max(skillStat.range - 1f, 0.5f);
        }
        isDead = false;

        hpController = transform.Find("HP Bar").GetComponent<GaugeController>();
        hpController.initData((int)monsterBattleInfo.basic[BasicStatEnum.hp].amount, 8, 1);

        effects = new List<EffectStat>();

        animCasting = transform.Find("Casting Effect").GetComponent<Animator>();

        hpController.gameObject.SetActive(false);
        transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        bg.GetComponent<SpriteRenderer>().flipX = true;
    }

    public void initInfo(MonsterBattleInfo monsterBattleInfo, Transform monsterContainer, Transform entryContainer)
    {
        initInfo(monsterBattleInfo, monsterContainer);
        this.entryContainer = entryContainer;
    }

    public void setPlaceInfo(Vector2 entryNum)
    {
        this.entryNum = entryNum;
        hpController.setSide(entryNum.x);
        bg.GetComponent<SpriteRenderer>().flipX = entryNum.x == 0f;
    }

    void Update()
    {
        if (!LocalStorage.IS_GAME_PAUSE)
        {
            if (BattleManager.getCurStage() == 5)
            {
                if (!hpController.gameObject.activeSelf)
                {
                    hpController.gameObject.SetActive(true);
                    transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                }
                if (!isDead)
                {
                    if (LocalStorage.IS_BATTLE_FINISH)
                    {
                    }
                    else
                    {
                        int[] closest = BattleManager.getClosestIndex(entryNum);
                        moveTo(closest[1]);
                        passTimer();
                        passEffects();
                        chooseSkillToExecute();
                        tryExecuteSkill();
                    }
                    checkCurrentHp();
                }
            }
        }
        if (LocalStorage.IS_BATTLE_FINISH)
        {
            Destroy(anim);
            Destroy(animCasting);
        }
    }

    // 시간에 따른 효과 소멸 또는 쿨타임 감소 등은 여기서 관리
    private void passTimer()
    {
        // 캐스팅 타임 관리
        if (castingTime > 0f)
        {
            animCasting.SetFloat("isCasting", 1f);
            castingTime = Mathf.Max(castingTime - (Time.deltaTime * (1 + liveBattleInfo.basic[BasicStatEnum.timeCastingCycle].amount)), 0f);
        }
        else
        {
            animCasting.SetFloat("isCasting", 0f);
        }

        // 방향 왜곡 지속 시간 관리
        timeDistortion += Time.deltaTime;

        // 쿨타임 관리
        foreach (SkillStat skill in liveBattleInfo.skills.Values)
        {
            skill.timeCharging += Time.deltaTime * (1 / liveBattleInfo.basic[BasicStatEnum.timeCoolCycle].amount);
        }

        // 애니메이션 관리
        if (anim.GetFloat("BattleState") != 0f && animTime <= 1f)
        {
            animTime += Time.deltaTime;
        }
        else
        {
            anim.SetFloat("BattleState", 0f);
        }

        // 넉백/대쉬 등 관리
        extraMovement *= 0.9f;
    }

    private void passEffects()
    {
        // 적용중인 효과 관리
        List<int> idxEnd = new List<int>();
        for (int i = 0; i < effects.Count; i++)
        {
            if (effects[i] != null)
            {
                EffectStat effect = effects[i];
                // 지속 시간 차감
                effect.duration -= Time.deltaTime;
                // 틱 시간 체크
                if (effect.tickTime == 0)
                {
                    // 틱 시간이 0이다 -> 최초 1번만 적용
                    // 적용 후 tick 시간을 -1로 변경 -> 다시는 적용 안함
                    if (!effect.isMultiple)
                    {
                        if (effect.name == BasicStatEnum.position)
                        {
                            extraMovement += effect.directionWithPower;
                        }
                        else
                        {
                            liveBattleInfo.basic[effect.name].amount += effect.amount;
                        }
                    }
                    else
                    {
                        // 곱 연산의 경우:: 현재 수치가 아닌 원래 수치에 곱연산 적용 후 결과값을 현 수치에 합 연산
                        // 계산 후, 해당 effect가 결괏값을 들고 isMultiple을 false로 바꾸어준다
                        float actualAmount = monsterBattleInfo.basic[effect.name].amount * effect.amount;
                        liveBattleInfo.basic[effect.name].amount += actualAmount;
                        effect.amount = actualAmount;
                        effect.isMultiple = false;
                    }
                    effect.tickTime = -1f;
                }
                else if (effect.tickTime > 0f)
                {
                    // 틱 시간이 0이 아니다 -> 지속 적용
                    // tickTimer 시간 누적
                    effect.tickTimer += Time.deltaTime;
                    // tickTimer > tickTime -> 작용 후 tickTimer 초기화
                    if (effect.tickTimer >= effect.tickTime)
                    {
                        effect.tickTimer = 0f;
                        if (!effect.isMultiple)
                        {
                            if (effect.name == BasicStatEnum.position)
                            {
                                extraMovement += effect.directionWithPower;
                            }
                            else
                            {
                                liveBattleInfo.basic[effect.name].amount += effect.amount;
                            }
                        }
                        else
                        {
                            // 곱 연산의 경우:: 현재 수치가 아닌 원래 수치에 곱연산 적용 후 결과값을 현 수치에 합 연산
                            // 계산 후, 해당 effect가 결괏값을 들고 isMultiple을 false로 바꾸어준다
                            float actualAmount = monsterBattleInfo.basic[effect.name].amount * effect.amount;
                            liveBattleInfo.basic[effect.name].amount += actualAmount;
                            effect.amount = actualAmount;
                            effect.isMultiple = false;
                        }
                    }
                }
                // duration이 0 이하이다 -> 효과 종료 -> 효과로 인해 깎인 수치 회복:: hp 제외
                if (effect.duration <= 0f)
                {
                    if (effect.name != BasicStatEnum.hp)
                    {
                        liveBattleInfo.basic[effect.name].amount -= effect.amount;
                    }
                    idxEnd.Add(i);
                }
            }
        }
        foreach (int i in idxEnd)
        {
            effects[i] = null;
        }
    }

    private void moveTo(int targetIdx)
    {
        MonsterBattleController target = BattleManager.monsterBattleControllerList[1 - (int)entryNum.x][targetIdx];
        Vector3 direction =
                        new Vector3(
                            target.transform.localPosition.x - transform.localPosition.x,
                            target.transform.localPosition.y - transform.localPosition.y,
                            0f
                        );
        directionToTarget = direction.normalized;
        bg.GetComponent<SpriteRenderer>().flipX = directionToTarget.x >= 0f;
        direction *= BattleManager.getDistanceBetween(entryNum, target.entryNum) > distanceToKeep ? 1f : -1f;
        if (timeDistortion > 3f)
        {
            timeDistortion = 0f;
            directionDistortion = MonsterCommonFunction.decideDistortion(direction, transform.position);
        }
        if (BattleManager.getDistanceBetween(entryNum, target.entryNum) <= distanceToKeep)
        {
            direction += MonsterCommonFunction.getDistortedDirection(direction, transform.position, directionDistortion);
        }
        curDirection = (castingTime <= 0f
                ? (Vector3.Normalize(direction)
                    * liveBattleInfo.basic[BasicStatEnum.spd].amount)
                : Vector3.zero)
                + extraMovement;
        transform.Translate(
            curDirection * Time.deltaTime,
            Space.Self
        );
    }

    private void checkCurrentHp()
    {
        float curHp = liveBattleInfo.basic[BasicStatEnum.hp].amount;
        hpController.updateGauge(curHp <= 0f ? 0 : (int)curHp);
        if (curHp <= 0f)
        {
            Destroy(anim);
            bg.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>(
                PathInfo.Monster.Sprite + monsterBattleInfo.speicie
                )[7];
            bg.GetComponent<SpriteRenderer>().flipX = directionToTarget.x >= 0f;
            isDead = true;
        }
    }

    private void chooseSkillToExecute()
    {
        // 현재 영창중인 스킬이 있는지 확인
        if (castingTime == -1f)
        {
            float targetSkillCooltime = 0f;
            // 쿨타임 관리
            foreach (KeyValuePair<string, SkillStat> skillPair in liveBattleInfo.skills)
            {
                SkillStat skill = skillPair.Value;
                // 스킬 쿨 다 찼는지?
                if (skill.timeCharging >= skill.coolTime)
                {
                    // 스킬 사용이 가능한지? = 즉, 사정거리 내에 대상이 존재하는지?
                    targetIdxList = BattleManager.getIndexListByDistance(entryNum, skill.targetType, skill.range, skill.numTarget);
                    if (targetIdxList.Length != 0)
                    {
                        // skillStatToExecute가 비었는지? || skill 쿨타임이 skillStatToExecute의 쿨타임보다 긴지?
                        if (targetSkillName == null || targetSkillCooltime < skill.coolTime)
                        {
                            targetIdxList = targetIdxList;
                            targetSkillName = skillPair.Key;
                            targetSkillCooltime = skill.coolTime;
                            castingTime = skill.castingTime;
                            animCasting.SetFloat("scale", castingTime >= 1.5f ? 1f : 0f);
                            //animCasting.transform.localPosition = directionToTarget / 3f;
                        }
                    }
                }
            }
        }
    }

    private void tryExecuteSkill()
    {
        // 주문 영창이 종료되었는가?
        if (castingTime == 0f)
        {
            setAnimation(1);
            SkillStat targetSkillStat = liveBattleInfo.skills[targetSkillName];
            // 본인에게 미치는 영향 적용
            if (targetSkillStat.toCaster != null)
                foreach (EffectStat eff in targetSkillStat.toCaster)
                {
                    if (eff.name == BasicStatEnum.position)
                    {
                        eff.directionWithPower =
                            MonsterCommonFunction.translatePositionPowerToVector3(
                                directionToTarget,
                                eff.amount
                                );
                    }
                    effects.Add(new EffectStat(eff));
                }
            // 스킬 사용
            BattleManager.executeProjectiles(
                targetSkillStat.projectiles,
                targetSkillStat.delayProjectile,
                targetIdxList,
                transform.position,
                new int[] { (int)entryNum.x, (int)entryNum.y },
                targetSkillStat.targetType.Contains("ENEMY") ? (1 - (int)entryNum.x) : (int)entryNum.x);

            // 초기화
            castingTime = -1f;
            liveBattleInfo.skills[targetSkillName].timeCharging = 0f;
            targetSkillName = null;
        }
    }

    public Vector2 getSide()
    {
        return entryNum;
    }

    public void OnMouseDown()
    {
        if (entryNum.x == -1f)
        {
            entryContainer.GetComponent<EntryController>().changeOnMoveStatusFromInven(transform.gameObject);
            for (int i = 0; i < BattleManager.monsterBattleControllerList[0].Length; i++)
            {
                if (BattleManager.monsterBattleControllerList[0][i] == this)
                {
                    BattleManager.monsterBattleControllerList[0][i] = null;
                    break;
                }
            }
        }
    }

    public void OnMouseDrag()
    {
        if (entryNum.x == -1f)
        {
            Vector3 mousePos = new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                10f
                );
            Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePos);
            objPosition.z = 18;
            transform.position = objPosition;
        }
    }

    public void OnMouseUp()
    {
        if (entryNum.x == -1f)
        {
            RaycastHit temp;
            if (Physics.Raycast(transform.position, Vector3.forward, out temp, 3f))
            {
                if (temp.transform.parent.tag == "Tile")
                {
                    entryContainer.GetComponent<EntryController>().removeEntry(transform.gameObject);
                    transform.localScale = Vector3.one;
                    EntrySlotController tempSlot = temp.transform.parent.GetComponent<EntrySlotController>();
                    transform.SetParent(monsterContainer);
                    transform.localPosition = new Vector3(-1f + tempSlot.x, tempSlot.y, 0f);
                    for (int i = 0; i < BattleManager.monsterBattleControllerList[0].Length; i++)
                    {
                        if (BattleManager.monsterBattleControllerList[0][i] == null)
                        {
                            BattleManager.monsterBattleControllerList[0][i] = this;
                            break;
                        }
                    }
                    transform.localScale = Vector3.one;
                }
            }
            else
            {
                entryContainer.GetComponent<EntryController>().addEntry(transform.gameObject);
            }
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "Barrier")
        {
            extraMovement = -curDirection / 10;
        }
    }

    /** 애니메이션 변경용 함수
     * statusToApply: -1 <- 피격 || 1 <- 공격
    */
    public void setAnimation(int statusToApply)
    {
        if (anim != null)
        {
            anim.SetFloat("BattleState", statusToApply);
        }
        animTime = 0f;
    }
}