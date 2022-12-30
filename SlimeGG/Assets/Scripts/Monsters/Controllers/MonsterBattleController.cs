using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;

public class MonsterBattleController : MonoBehaviour
{
    private Vector2 entryNum { get; set; }
    public Transform[] allies { get; set; }
    public Transform[] enemies { get; set; }
    public MonsterInfo monsterInfo { get; set; }
    public MonsterSpeciesInfo speciesInfo { get; set; }
    private Transform bg { get; set; }
    private Animator anim { get; set; }
    private Animator animHit { get; set; }
    private SpriteRenderer spriteHit { get; set; }
    private float animTime = 0f;
    private float stopTime = 0f;
    public float maxHp, curHp, def;

    public float[] distanceAllies;
    public float[] distanceEnemies;

    private SkillStat curSkillStat = null;

    private Dictionary<MonsterSkillEnum, float> skillTimer = new Dictionary<MonsterSkillEnum, float>();

    public Vector3 curKnockback = Vector3.zero;
    public Vector3 curDash = Vector3.zero;
    public float distanceToKeep { get; set; }

    public bool isDead { get; set; }

    private GaugeController hpController { get; set; }

    [SerializeField]
    private GameObject bulletPrefab;

    public void initInfo(MonsterInfo monsterInfo)
    {
        this.monsterInfo = monsterInfo;
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
            skillTimer[skillEnum] = 0f;
            float ran = LocalDictionary.skills[skillEnum].range;
            if (distanceToKeep > ran) distanceToKeep = Mathf.Max(ran - 1f, 0.5f);
        }
        maxHp = monsterInfo.hp * speciesInfo.hp;
        curHp = monsterInfo.hp * speciesInfo.hp;
        def = monsterInfo.def * speciesInfo.def;
        isDead = false;

        hpController = transform.Find("HP Bar").GetComponent<GaugeController>();
        hpController.initData((int)maxHp, 8, 1);

        spriteHit = transform.Find("Hit Effect").GetComponent<SpriteRenderer>();
        animHit = spriteHit.GetComponent<Animator>();
        animHit.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(
            PathInfo.ANIMATION + "Effects/Hits" + "/Normal" + "/Controller"
            );
    }

    public void setFieldInfo(Vector2 entryNum, Transform[] allies, Transform[] enemies)
    {
        this.entryNum = entryNum;
        this.allies = allies;
        this.enemies = enemies;
        distanceAllies = new float[allies.Length];
        distanceEnemies = new float[enemies.Length];
        hpController.setSide(entryNum.x);
        anim.SetFloat("DirectionX", entryNum.x == 0f ? 1f : -1f);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!LocalStorage.IS_GAME_PAUSE)
        {
            if (LocalStorage.BATTLE_SCENE_LOADING_DONE && !isDead)
            {
                if (LocalStorage.IS_BATTLE_FINISH)
                {
                    Destroy(anim);
                    Destroy(animHit);
                }
                else
                {
                    curKnockback *= 0.9f;
                    curDash *= 0.9f;
                    foreach (MonsterSkillEnum skillEnum in skillTimer.Keys.ToList())
                    {
                        skillTimer[skillEnum] += Time.deltaTime;
                    }
                    int[] closest = identifyTarget();
                    moveTo(enemies[closest[1]]);

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
            if (isDead)
            {
                Destroy(anim);
                Destroy(animHit);
                Destroy(spriteHit);
                bg.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>(
                    PathInfo.SPRITE + speciesInfo.resourcePath
                    )[entryNum.x == 0 ? 13 : 12];
            }
        }

    }

    // 2. 현재 사용 가능 스킬이 있는가? = 사정거리 내에 적이 있음 -> 사용
    // 3. 사용 가능 스킬이 없는가? = 사정 거리 내에 적이 없음 -> 이동

    // 1. 주변 몬스터까지의 거리 식별
    //  각 적까지의 거리를 계산하여 보관 -> distances
    //  return -> int[0] = 가장 가까운 아군, int[1] = 가장 가까운 적 인덱스
    int[] identifyTarget()
    {
        float closestLength = 0f;
        int closestAllyIndex = 0;
        for (int i = 0; i < allies.Length; i++)
        {
            if (i != entryNum.y)
            {
                if (!allies[i].GetComponent<MonsterBattleController>().isDead)
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
            if (!enemies[i].GetComponent<MonsterBattleController>().isDead)
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

    // 사용 가능한 스킬 중 가장 쿨타임이 긴 스킬 반환
    // res 길이 0 <= 사용 가능 스킬 없음
    List<int> checkSkillsAvailable()
    {
        List<int> res = new List<int>();
        foreach (MonsterSkillEnum skillName
            in speciesInfo.skills)
        {
            SkillStat skillStat = LocalDictionary.skills[skillName];
            if (skillStat.coolTime <= skillTimer[skillName])
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

    // 해당 적에게 이동

    private void moveTo(Transform target)
    {
        float curDistance = Vector3.Distance(target.localPosition, transform.localPosition);
        Vector3 direction =
                        new Vector3(
                            (target.localPosition.x - transform.localPosition.x) * Random.Range(1f, 10f),
                            (target.localPosition.y - transform.localPosition.y) * Random.Range(1f, 10f),
                            0f
                        );
        anim.SetFloat("DirectionX", direction.x);
        animHit.SetFloat("DirectionX", direction.x);
        if (curDistance > distanceToKeep)
        {
            transform.Translate(
                ((
                    (stopTime <= 0f
                    ? (Vector3.Normalize(direction) * monsterInfo.spd * speciesInfo.spd)
                    : Vector3.zero)
                    + curKnockback + curDash
                ) * Time.deltaTime),
                Space.Self
            );
        }
        else
        {
            transform.Translate(
                ((
                    (stopTime <= 0f
                     ? (Vector3.Normalize(-direction) * monsterInfo.spd * speciesInfo.spd)
                     : Vector3.zero)
                     + curKnockback + curDash
                ) * Time.deltaTime),
                Space.Self
            );
        }
    }
    void executeSkill(SkillStat skillStat, List<int> targetList)
    {
        if (stopTime == 0f)
        {
            stopTime = skillStat.delayTime;
        }
        else if (stopTime > 0f)
        {
            stopTime = stopTime - Time.deltaTime == 0f ? -1f : stopTime - Time.deltaTime;
        }
        else
        {
            curSkillStat = null;
            skillTimer[skillStat.skillName] = 0f;
            anim.SetFloat("BattleState", 1f);
            SkillExecutor.execute(skillStat, this, targetList);
            stopTime = 0f;
        }
    }

    public void calcHpDamage(int amount)
    {
        hpController.addOrSubGauge(amount);
    }

    public void makeDead()
    {
        isDead = true;
    }

    public void activateHitEffect(MonsterSkillTypeEnum skillType, bool isCritical)
    {
        float dir = anim.GetFloat("DirectionX");
        animHit.transform.localPosition = Vector3.Normalize(new Vector3(dir, Random.Range(-dir, dir), 0f)) / 5f;
        anim.SetFloat("BattleState", -1f);
        animHit.SetFloat("isCritical", isCritical ? 2f : 1f);
    }

    public GameObject generateBullet()
    {
        float dir = anim.GetFloat("DirectionX");
        GameObject res = Instantiate(bulletPrefab);
        res.transform.position = transform.position + (Vector3.Normalize(new Vector3(dir, Random.Range(-dir, dir), 0f)) / 5f);
        return res;
    }
}