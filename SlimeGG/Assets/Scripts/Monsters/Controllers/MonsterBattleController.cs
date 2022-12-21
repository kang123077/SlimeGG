using Unity.VisualScripting;
using UnityEngine;

public class MonsterBattleController : MonoBehaviour
{
    private Vector2 entryNum;
    Transform[] allies;
    Transform[] enemies;
    private MonsterInfo monsterInfo;
    private Transform bg;
    private Animator anim;

    private float[] distanceAllies;
    private float[] distanceEnemies;

    public void initInfo(MonsterInfo monsterInfo)
    {
        this.monsterInfo = monsterInfo;
        bg = transform.Find("Image");
        bg.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(
            PathInfo.SPRITE + LocalDictionary.monsters[monsterInfo.accuSpecies[-1]].resourcePath
            );
        Destroy(bg.GetComponent<PolygonCollider2D>());
        bg.AddComponent<PolygonCollider2D>();
        anim = bg.GetComponent<Animator>();
        anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(
            PathInfo.ANIMATION + LocalDictionary.monsters[monsterInfo.accuSpecies[-1]].resourcePath + "/Controller"
            );
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
            if (closestLength == 0f ||
                closestLength > (distanceAllies[i] = Vector2.Distance(transform.position, allies[i].position)))
            {
                closestLength = distanceAllies[i];
                closestAllyIndex = i;
            }
        }
        closestLength = 0f;
        int closestEnemyIndex = 0;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (closestLength == 0f ||
                closestLength > (distanceEnemies[i] = Vector2.Distance(transform.position, enemies[i].position)))
            {
                closestLength = distanceEnemies[i];
                closestEnemyIndex = i;
            }
        }
        return new int[] { closestAllyIndex, closestEnemyIndex };
    }

    // 2. 스킬 사용 가능 여부 체크
    //  1번에서 각 몬스터 별 거리를 기준으로 사용 가능한 스킬이 있는가?
    //  일반 공격도 스킬로 처리
    //  두개 이상의 스킬이 사용 가능하다고 판명이 난 경우:
    //      쿨타임이 긴 쪽을 우선 사용
    // return -> 사용할 스킬 번호
    int checkSkillsAvailable()
    {

        return 0;
    }

    // 해당 적에게 이동

    // 해당 적으로부터 도망

    // n번 스킬을 해당 적을 대상으로 사용
    void executeSkill(int num)
    {

    }

}
