using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class MonsterBattleController : MonoBehaviour
{
    private Vector2 entryNum { get; set; }
    public Transform[] allies { get; set; }
    public Transform[] enemies { get; set; }
    private MonsterInfo monsterInfo { get; set; }
    private Transform bg { get; set; }
    private Animator anim { get; set; }

    public float[] distanceAllies;
    public float[] distanceEnemies;

    private SkillStat curSkillStat = null;

    private Dictionary<MonsterSkillEnum, float> skillTimer = new Dictionary<MonsterSkillEnum, float>();

    public Vector3 curKnockback = Vector3.zero;
    public float distanceToKeep { get; set; }

    public void initInfo(MonsterInfo monsterInfo)
    {
        this.monsterInfo = monsterInfo;

        MonsterSpeciesInfo speciesInfo = LocalDictionary.monsters[monsterInfo.accuSpecies.Last()];
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
            if (distanceToKeep > ran) distanceToKeep = Mathf.Max(ran - 1f, 0f);
        }
    }

    public void setFieldInfo(Vector2 entryNum, Transform[] allies, Transform[] enemies)
    {
        this.entryNum = entryNum;
        this.allies = allies;
        this.enemies = enemies;
        distanceAllies = new float[allies.Length];
        distanceEnemies = new float[enemies.Length];
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (LocalStorage.BATTLE_SCENE_LOADING_DONE)
        {
            curKnockback *= 0.9f;
            //curKnockback = Vector3.zero;
            List<MonsterSkillEnum> skillEnums = skillTimer.Keys.ToList();
            foreach (MonsterSkillEnum skillEnum in skillEnums)
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
                distanceAllies[i] = Vector2.Distance(transform.localPosition, allies[i].localPosition);
                if (closestLength == 0f ||
                    closestLength > distanceAllies[i])
                {
                    closestLength = distanceAllies[i];
                    closestAllyIndex = i;
                }
            }
        }
        closestLength = 0f;
        int closestEnemyIndex = 0;
        for (int i = 0; i < enemies.Length; i++)
        {
            distanceEnemies[i] = Vector2.Distance(transform.localPosition, enemies[i].localPosition);
            if (closestLength == 0f ||
                closestLength > distanceEnemies[i])
            {
                closestLength = distanceEnemies[i];
                closestEnemyIndex = i;
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
            in LocalDictionary.monsters[monsterInfo.accuSpecies.Last()].skills)
        {
            SkillStat skillStat = LocalDictionary.skills[skillName];
            if (skillStat.coolTime <= skillTimer[skillName])
            {
                List<int> targetIndexList = SkillExecutor.selectTargetIndexList(skillStat, this);
                if (targetIndexList.Count > 0)
                {
                    if (curSkillStat == null ||
                        skillStat.coolTime <= curSkillStat.coolTime)
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
        if (curDistance > distanceToKeep)
        {
            transform.Translate(
                ((
                    Vector3.Normalize(
                        new Vector3(
                            target.localPosition.x - transform.localPosition.x,
                            target.localPosition.y - transform.localPosition.y,
                            0f
                        ) * monsterInfo.spd
                    ) + curKnockback
                ) * Time.deltaTime),
                Space.Self
            );
        }
        else
        {
            transform.Translate(
                ((
                    Vector3.Normalize(
                        new Vector3(
                            transform.localPosition.x - target.localPosition.x,
                            transform.localPosition.y - target.localPosition.y,
                            0f
                        ) * monsterInfo.spd
                    ) + curKnockback
                ) * Time.deltaTime),
                Space.Self
            );
        }
    }

    // 해당 적으로부터 도망

    // n번 스킬을 해당 적을 대상으로 사용
    void executeSkill(SkillStat skillStat, List<int> targetList)
    {
        skillTimer[skillStat.skillName] = 0f;
        SkillExecutor.execute(skillStat, this, targetList);
    }

}
