using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class MonsterBattleController : MonoBehaviour
{
    private Vector2 entryNum { get; set; }
    private Vector2 fieldSize { get; set; }
    public Transform[] allies { get; set; }
    public Transform[] enemies { get; set; }
    public MonsterInfo monsterInfo { get; set; }
    public MonsterInfo currentMonsterInfo { get; set; }
    public MonsterSpeciesInfo speciesInfo { get; set; }
    private Transform bg { get; set; }
    private Animator anim { get; set; }
    private Animator animHit { get; set; }
    private SpriteRenderer spriteHit { get; set; }
    private float animTime = 0f;
    public float stopTime = 0f;
    public float maxHp;

    public float[] distanceAllies;
    public float[] distanceEnemies;

    private SkillStat curSkillStat = null;

    public Dictionary<MonsterSkillEnum, float> skillTimer = new Dictionary<MonsterSkillEnum, float>();
    public List<SkillStat> skillStatList = new List<SkillStat>();

    public Vector3 extraMovement = Vector3.zero;
    public float distanceToKeep { get; set; }

    public bool isDead { get; set; }

    private GaugeController hpController { get; set; }

    [SerializeField]
    private GameObject projectilePrefab;

    private Vector3 curDirection = Vector3.zero;
    private Vector2 dirFromCenter;
    private bool onlyMove = false;

    public void initInfo(MonsterInfo monsterInfo)
    {
        this.monsterInfo = monsterInfo.Clone();
        speciesInfo = LocalDictionary.monsters[monsterInfo.accuSpecies.Last()];
        bg = transform.Find("Image");
        bg.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(
            PathInfo.SPRITE + speciesInfo.resourcePath
            );
        Destroy(bg.GetComponent<PolygonCollider2D>());
        bg.AddComponent<PolygonCollider2D>();

        anim = bg.GetComponent<Animator>();
        anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(
            PathInfo.ANIMATION + speciesInfo.resourcePath + "/Controller"
            );
        distanceToKeep = 100f;
        foreach (MonsterSkillEnum skillEnum in speciesInfo.skills)
        {
            SkillStat temp = LocalDictionary.skills[skillEnum];
            skillStatList.Add(LocalDictionary.skills[skillEnum]);
            skillTimer[skillEnum] = 0f;
            float ran = temp.range;
            if (distanceToKeep > ran) distanceToKeep = Mathf.Max(ran - 1f, 0.5f);
        }

        currentMonsterInfo = new MonsterInfo();
        foreach (MonsterVariableEnum basicEnum in speciesInfo.basicDict.Keys)
        {
            currentMonsterInfo.basicDict[basicEnum] = new MonsterVariableStat(basicEnum, 1, false);
            currentMonsterInfo.basicDict[basicEnum].amount = monsterInfo.basicDict[basicEnum].amount * speciesInfo.basicDict[basicEnum].amount;
        }

        maxHp = currentMonsterInfo.basicDict[MonsterVariableEnum.hp].amount;
        isDead = false;

        hpController = transform.Find("HP Bar").GetComponent<GaugeController>();
        hpController.initData((int)maxHp, 8, 1);

        spriteHit = transform.Find("Hit Effect").GetComponent<SpriteRenderer>();
        animHit = spriteHit.GetComponent<Animator>();
        animHit.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(
            PathInfo.ANIMATION + "Effects/Hits" + "/Normal" + "/Controller"
            );
    }

    public void setFieldInfo(Vector2 fieldSize, Vector2 entryNum, Transform[] allies, Transform[] enemies)
    {
        this.entryNum = entryNum;
        this.allies = allies;
        this.enemies = enemies;
        distanceAllies = new float[allies.Length];
        distanceEnemies = new float[enemies.Length];
        hpController.setSide(entryNum.x);
        anim.SetFloat("DirectionX", entryNum.x == 0f ? 1f : -1f);
        this.fieldSize = fieldSize;

        setTexturetoCamera((int)entryNum.x, (int)entryNum.y);
    }

    void Update()
    {
        if (!LocalStorage.IS_GAME_PAUSE)
        {
            if (LocalStorage.BATTLE_SCENE_LOADING_DONE)
            {
                if (!isDead)
                {
                    checkCurrentHp();
                }
                if (isDead)
                {
                    Destroy(anim);
                    Destroy(animHit);
                    Destroy(spriteHit);
                    bg.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>(
                        PathInfo.SPRITE + speciesInfo.resourcePath
                        )[entryNum.x == 0 ? 13 : 12];
                    return;
                }
                if (LocalStorage.IS_BATTLE_FINISH)
                {
                    Destroy(anim);
                    Destroy(animHit);
                }
                else
                {
                    extraMovement *= 0.9f;
                    foreach (MonsterSkillEnum skillEnum in skillTimer.Keys.ToList())
                    {
                        skillTimer[skillEnum] += Time.deltaTime;
                    }
                    int[] closest = identifyTarget();
                    moveTo(enemies[closest[1]]);
                    if (stopTime > 0f)
                    {
                        stopTime = (stopTime - Time.deltaTime) == 0f ? -1f : (stopTime - Time.deltaTime);
                    }
                    List<int> skillAvailableList = checkSkillsAvailable();
                    if (skillAvailableList.Count > 0)
                    {
                        executeSkill(curSkillStat, skillAvailableList);
                    }

                    if (anim.GetFloat("BattleState") != 0f && animTime <= 1f)
                    {
                        animTime += Time.deltaTime;
                    }
                    if (animTime > 0.25f)
                    {
                        animHit.SetFloat("isCritical", 0f);
                    }
                    if (animTime > 0.5f)
                    {
                        anim.SetFloat("BattleState", 0f);
                        animTime = 0f;
                    }
                }
            }
        }

    }

    private int[] identifyTarget()
    {
        float closestLength = 0f;
        int closestAllyIndex = 0;
        for (int i = 0; i < allies.Length; i++)
        {
            if (i != entryNum.y)
            {
                if (allies[i] != null && !allies[i].GetComponent<MonsterBattleController>().isDead)
                {
                    distanceAllies[i] = Vector2.Distance(transform.localPosition, allies[i].localPosition);
                    if (closestLength == 0f ||
                        closestLength > distanceAllies[i])
                    {
                        closestLength = distanceAllies[i];
                        closestAllyIndex = i;
                    }
                    else
                    {
                        distanceAllies[i] = -1f;
                    }
                }
            }
        }
        closestLength = 0f;
        int closestEnemyIndex = 0;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null && !enemies[i].GetComponent<MonsterBattleController>().isDead)
            {
                distanceEnemies[i] = Vector2.Distance(transform.localPosition, enemies[i].localPosition);
                if (closestLength == 0f ||
                    closestLength > distanceEnemies[i])
                {
                    closestLength = distanceEnemies[i];
                    closestEnemyIndex = i;
                }
            }
            else
            {
                distanceEnemies[i] = -1f;
            }
        }
        return new int[] { closestAllyIndex, closestEnemyIndex };
    }

    private List<int> checkSkillsAvailable()
    {
        List<int> res = new List<int>();
        foreach (SkillStat skillStat
            in skillStatList)
        {
            if (skillStat.coolTime <= skillTimer[skillStat.skillName])
            {
                List<int> targetIndexList = SkillExecutor.selectTargetIndexList(skillStat, this);
                if (targetIndexList.Count > 0)
                {
                    if (curSkillStat == null ||
                        skillStat.coolTime >= curSkillStat.coolTime)
                    {
                        curSkillStat = skillStat;
                        res = targetIndexList;
                    }
                }
            }
        }
        return res;
    }

    private void moveTo(Transform target)
    {
        float curDistance = Vector3.Distance(target.localPosition, transform.localPosition);
        dirFromCenter = new Vector2(transform.position.x, transform.position.y) - (fieldSize / 2) / 5f;
        Vector3 direction =
                        new Vector3(
                            target.localPosition.x - transform.localPosition.x - dirFromCenter.x,
                            target.localPosition.y - transform.localPosition.y - dirFromCenter.y,
                            0f
                        );
        anim.SetFloat("DirectionX", direction.x);
        animHit.SetFloat("DirectionX", direction.x);
        curDirection = (stopTime <= 0f
                    ? (Vector3.Normalize((curDistance > distanceToKeep ? 1f : -1f) * direction)
                    * currentMonsterInfo.basicDict[MonsterVariableEnum.spd].amount)
                    : Vector3.zero)
                    + (onlyMove ? Vector3.zero : extraMovement);

        transform.Translate(
            curDirection * Time.deltaTime,
            Space.Self
        );
    }
    void executeSkill(SkillStat skillStat, List<int> targetList)
    {
        if (stopTime == 0f)
        {
            stopTime = skillStat.castingTime;
        }
        if (stopTime < 0f)
        {
            curSkillStat = null;
            skillTimer[skillStat.skillName] = 0f;
            anim.SetFloat("BattleState", 1f);
            SkillExecutor.execute(skillStat, this, targetList);
            stopTime = 0f;
        }
    }

    private void checkCurrentHp()
    {
        float curHp = currentMonsterInfo.basicDict[MonsterVariableEnum.hp].amount;
        hpController.updateGauge(curHp <= 0f ? 0 : (int)curHp);
        if (curHp <= 0f)
        {
            isDead = true;
        }
    }

    public void activateHitEffect(MonsterSkillTypeEnum skillType, bool isCritical)
    {
        if (anim == null) return;
        float dir = anim.GetFloat("DirectionX");
        animHit.transform.localPosition = Vector3.Normalize(new Vector3(dir, Random.Range(-dir, dir), 0f)) / 5f;
        anim.SetFloat("BattleState", -1f);
        animHit.SetFloat("isCritical", isCritical ? 2f : 1f);
    }

    public GameObject generateBullet()
    {
        float dir = anim.GetFloat("DirectionX");
        GameObject res = Instantiate(projectilePrefab);
        res.transform.position = transform.position + (Vector3.Normalize(new Vector3(dir, Random.Range(-dir, dir), 0f)) / 5f);
        return res;
    }

    private void setTexturetoCamera(int side, int numPos)
    {
        transform.Find("Tracking Camera").GetComponent<Camera>().targetTexture = Resources.Load<RenderTexture>(
            PathInfo.TEXTURE + "MonsterTracking/" + $"{side}_{numPos}"
            );
    }
}