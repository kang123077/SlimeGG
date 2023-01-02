using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class MonsterBattleController : MonoBehaviour
{
    public Vector2 entryNum { get; set; }
    private Vector2 fieldSize { get; set; }
    public MonsterInfo monsterInfo { get; set; }
    public MonsterInfo currentMonsterInfo { get; set; }
    public MonsterSpeciesInfo speciesInfo { get; set; }
    private Transform bg { get; set; }
    private Animator anim { get; set; }
    private Animator animHit { get; set; }
    private SpriteRenderer spriteHit { get; set; }
    private float animTime = 0f;
    private float castingTime = -1f;
    public float maxHp;

    private SkillStat curSkillStat = null;

    public Dictionary<MonsterSkillEnum, float> skillTimer = new Dictionary<MonsterSkillEnum, float>();
    public List<SkillStat> skillStatList = new List<SkillStat>();

    public Vector3 extraMovement = Vector3.zero;
    private float distanceToKeep { get; set; }

    public bool isDead { get; set; }

    private GaugeController hpController { get; set; }

    [SerializeField]
    private GameObject projectilePrefab;

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

    public void setFieldInfo(Vector2 fieldSize, Vector2 entryNum)
    {
        this.entryNum = entryNum;
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
                        int[] closest = FieldController.getClosestIndex(entryNum);
                        moveTo(closest[1]);
                        passTimer();

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
                    checkCurrentHp();
                }
            }
        }

    }

    private void passTimer()
    {
        if (castingTime > 0f) castingTime = Mathf.Max(castingTime - Time.deltaTime, 0f);
    }

    private void moveTo(int targetIdx)
    {
        MonsterBattleController target = LocalStorage.monsterBattleControllerList[entryNum.x == 0f ? 1 : 0][targetIdx];
        dirFromCenter = new Vector2(transform.position.x, transform.position.y) - (fieldSize / 2) / 5f;
        Vector3 direction =
                        new Vector3(
                            target.transform.localPosition.x - transform.localPosition.x - dirFromCenter.x,
                            target.transform.localPosition.y - transform.localPosition.y - dirFromCenter.y,
                            0f
                        );
        anim.SetFloat("DirectionX", direction.x);
        animHit.SetFloat("DirectionX", direction.x);
        Vector3 curDirection = (castingTime <= 0f
                    ? (Vector3.Normalize((FieldController.getDistanceBetween(entryNum, target.entryNum) > distanceToKeep ? 1f : -1f) * direction)
                        * currentMonsterInfo.basicDict[MonsterVariableEnum.spd].amount)
                    : Vector3.zero)
                    + (onlyMove ? Vector3.zero : extraMovement);
        transform.Translate(
            curDirection * Time.deltaTime,
            Space.Self
        );
    }

    private void checkCurrentHp()
    {
        float curHp = currentMonsterInfo.basicDict[MonsterVariableEnum.hp].amount;
        hpController.updateGauge(curHp <= 0f ? 0 : (int)curHp);
        if (curHp <= 0f)
        {
            Destroy(anim);
            Destroy(animHit);
            Destroy(spriteHit);
            bg.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>(
                PathInfo.SPRITE + speciesInfo.resourcePath
                )[entryNum.x == 0 ? 13 : 12];
            isDead = true;
        }
    }

    private void setTexturetoCamera(int side, int numPos)
    {
        transform.Find("Tracking Camera").GetComponent<Camera>().targetTexture = Resources.Load<RenderTexture>(
            PathInfo.TEXTURE + "MonsterTracking/" + $"{side}_{numPos}"
            );
    }
}