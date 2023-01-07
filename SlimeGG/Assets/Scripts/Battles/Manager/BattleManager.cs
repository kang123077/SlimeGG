using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static Unity.Burst.Intrinsics.Arm;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private GameObject PenalForPause;
    [SerializeField]
    private GameObject monsterBase;
    [SerializeField]
    private GameObject fieldGenerated;
    [SerializeField]
    private GameObject monsterUIBase;
    [SerializeField]
    private GameObject monsterInfoUIGenerated;
    [SerializeField]
    public GameObject projectilePrefab;
    [SerializeField]
    public GameObject areaPrefab;
    [SerializeField]
    public GameObject entryListUI;

    public static GameObject staticProjectilePrefab;
    public static GameObject staticAreaPrefab;

    public static MonsterBattleController[][] monsterBattleControllerList =
        new MonsterBattleController[2][];
    public static float[][][] distanceAllies = new float[2][][];
    public static float[][][] distanceEnemies = new float[2][][];
    public static bool isBattleReady = false;
    public static Vector2 fieldSize;
    private static MonsterVO[] enemyEntry;
    private static MonsterVO[] allyEntry;
    private int sideWin { get; set; }

    private bool isBaseReady = false;
    private bool isEnemyReady = false;
    private bool isEntryReady = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isBattleReady && LocalStorage.DICTIONARY_LOADING_DONE && LocalStorage.MONSTER_DATACALL_DONE)
        {
            if (!isBaseReady)
            {
                staticProjectilePrefab = projectilePrefab;
                staticAreaPrefab = areaPrefab;
                initField("Normal");
                isBaseReady = true;
            }
            if (!isEnemyReady)
            {
                callEnemyEntry("Test");
                callAllyEntry();
                generateEnemies();
                isEnemyReady = true;
            }
            if (!isEntryReady)
            {
                setEntryMonsters();
                isEntryReady = true;
            }
            //isBattleReady = true;
        }
        if (isBattleReady && !LocalStorage.IS_GAME_PAUSE)
        {
            calculateDistance();
            bool isOneSideAllDead;
            int side = 1;
            foreach (MonsterBattleController[] sideList in monsterBattleControllerList)
            {
                isOneSideAllDead = true;
                foreach (MonsterBattleController battleController in sideList)
                {
                    if (battleController != null)
                    {
                        isOneSideAllDead = isOneSideAllDead && battleController.isDead;
                    }
                }
                if (isOneSideAllDead)
                {
                    sideWin = side;
                    finishBattle();
                }
                else
                {
                    side--;
                }
            }
        }
        if (LocalStorage.IS_BATTLE_FINISH)
        {
            Debug.Log($"Battle Finished:: Win => {sideWin} <=");
            PenalForPause.SetActive(LocalStorage.IS_BATTLE_FINISH);
            PenalForPause.transform.Find("Text Title").GetComponent<TMP_Text>().text = "BATTLE FINISHED!";
            PenalForPause.transform.Find("Text Desc").GetComponent<TMP_Text>().text = $"Team {sideWin} Win!";
            Destroy(gameObject);
        }
    }

    private void callEnemyEntry(string entryPath)
    {
        enemyEntry =
                        CommonFunctions.loadObjectFromJson<MonsterVO[]>(
                            $"Assets/Resources/Jsons/Entries/{entryPath}"
                            );
    }

    private void callAllyEntry()
    {
        allyEntry = LocalStorage.monsters.ToArray();
    }

    private void generateEnemies()
    {
        MonsterBattleInfo temp;
        int cnt = 0;
        foreach (MonsterVO enemyVO in enemyEntry)
        {
            temp = MonsterCommonFunction.generateMonsterBattleInfo(enemyVO);
            generateMonster(temp, 1, cnt, temp.entryPos);
            cnt++;
        }
    }

    private void generateAllies()
    {
        MonsterBattleInfo temp;
        int cnt = 0;
        foreach (MonsterVO enemyVO in enemyEntry)
        {
            temp = MonsterCommonFunction.generateMonsterBattleInfo(enemyVO);
            generateMonster(temp, 1, cnt, temp.entryPos);
            cnt++;
        }
    }

    private void setEntryMonsters()
    {
        MonsterBattleInfo temp;
        int cnt = 0;
        foreach (MonsterVO enemyVO in allyEntry)
        {
            temp = MonsterCommonFunction.generateMonsterBattleInfo(enemyVO);
            GameObject newMonster = Instantiate(monsterBase);
            newMonster.GetComponent<MonsterBattleController>().initInfo(temp, fieldGenerated.transform.Find("Monster Container"), entryListUI.transform);
            entryListUI.GetComponent<EntryController>().addEntry(newMonster);
            cnt++;
        }
    }

    public void initGeneration()
    {
        // 적 몬스터 생성
        generateEnemies();
        //fieldGenerated.GetComponent<FieldController>().setFieldInfoForMonsters();
        //calculateDistance();
    }

    private void generateMonster(MonsterBattleInfo monsterInfo, int side, int numPos, int[] setPos)
    {
        GameObject newMonster = Instantiate(monsterBase);
        newMonster.GetComponent<MonsterBattleController>().initInfo(monsterInfo, fieldGenerated.transform.Find("Monster Container"));
        newMonster.GetComponent<MonsterBattleController>().setPlaceInfo(new Vector2(side, numPos));

        monsterBattleControllerList[side][numPos] = newMonster.GetComponent<MonsterBattleController>();

        fieldGenerated.GetComponent<FieldController>().setMonsterInPosition(newMonster.transform, side, setPos);

        GameObject newMonsterInfoUI = Instantiate(monsterUIBase);
        newMonsterInfoUI.transform.SetParent(monsterInfoUIGenerated.transform.Find($"{side}"));
        newMonsterInfoUI
            .GetComponent<MonsterBattleUIController>().initInfo(newMonster.GetComponent<MonsterBattleController>()
            , side
            , numPos);

    }

    //private void test1on1()
    //{
    //    MonsterBattleInfo newMon = MonsterCommonFunction.generateMonsterBattleInfo(LocalStorage.monsters[3]);
    //    generateMonster(newMon, 0, 0);
    //    newMon = MonsterCommonFunction.generateMonsterBattleInfo(LocalStorage.monsters[1]);
    //    generateMonster(newMon, 1, 0);
    //}

    //private void test3on2()
    //{
    //    MonsterBattleInfo newMon = MonsterCommonFunction.generateMonsterBattleInfo(LocalStorage.monsters[3]);
    //    generateMonster(newMon, 0, 0);
    //    newMon = MonsterCommonFunction.generateMonsterBattleInfo(LocalStorage.monsters[1]);
    //    generateMonster(newMon, 0, 1);
    //    newMon = MonsterCommonFunction.generateMonsterBattleInfo(LocalStorage.monsters[3]);
    //    generateMonster(newMon, 1, 0);
    //    newMon = MonsterCommonFunction.generateMonsterBattleInfo(LocalStorage.monsters[0]);
    //    generateMonster(newMon, 1, 1);
    //    newMon = MonsterCommonFunction.generateMonsterBattleInfo(LocalStorage.monsters[5]);
    //    generateMonster(newMon, 1, 2);
    //}

    //private void test3on4()
    //{
    //    MonsterBattleInfo newMon = MonsterCommonFunction.generateMonsterBattleInfo(LocalStorage.monsters[0]);
    //    generateMonster(newMon, 0, 0);
    //    newMon = MonsterCommonFunction.generateMonsterBattleInfo(LocalStorage.monsters[1]);
    //    generateMonster(newMon, 1, 0);
    //    newMon = MonsterCommonFunction.generateMonsterBattleInfo(LocalStorage.monsters[2]);
    //    generateMonster(newMon, 0, 1);
    //    newMon = MonsterCommonFunction.generateMonsterBattleInfo(LocalStorage.monsters[3]);
    //    generateMonster(newMon, 1, 1);
    //    newMon = MonsterCommonFunction.generateMonsterBattleInfo(LocalStorage.monsters[4]);
    //    generateMonster(newMon, 0, 2);
    //    newMon = MonsterCommonFunction.generateMonsterBattleInfo(LocalStorage.monsters[5]);
    //    generateMonster(newMon, 1, 2);
    //    newMon = MonsterCommonFunction.generateMonsterBattleInfo(LocalStorage.monsters[3]);
    //    generateMonster(newMon, 1, 3);
    //}
    private void initField(string fieldName)
    {
        float[] size = LocalDictionary.fields[fieldName].size.ToArray();
        fieldSize = new Vector2(size[0], size[1]);
        fieldGenerated.GetComponent<FieldController>().initField(LocalDictionary.fields[fieldName]);
    }

    public void pauseOrResumeBattle()
    {
        LocalStorage.IS_GAME_PAUSE = !LocalStorage.IS_GAME_PAUSE;
        PenalForPause.SetActive(LocalStorage.IS_GAME_PAUSE);
    }

    public void finishBattle()
    {
        LocalStorage.IS_BATTLE_FINISH = true;
    }
    private void calculateDistance()
    {
        for (int k = 0; k < 2; k++)
        {
            for (int i = 0; i < monsterBattleControllerList[k].Length; i++)
            {
                for (int j = 0; j < monsterBattleControllerList[k].Length; j++)
                {
                    if (distanceAllies[k][i][j] != -1f)
                    {
                        if (monsterBattleControllerList[k][i] == null ||
                            monsterBattleControllerList[k][j] == null ||
                            monsterBattleControllerList[k][i].isDead ||
                            monsterBattleControllerList[k][j].isDead ||
                            i == j)
                        {
                            distanceAllies[k][i][j] = -1f;
                        }
                        else
                        {
                            distanceAllies[k][i][j] = Vector3.Distance(
                                monsterBattleControllerList[k][i].transform.localPosition,
                                monsterBattleControllerList[k][j].transform.localPosition);
                        }
                    }
                }
            }
        }

        for (int i = 0; i < monsterBattleControllerList[0].Length; i++)
        {
            for (int j = 0; j < monsterBattleControllerList[1].Length; j++)
            {
                if (distanceEnemies[0][i][j] != -1f)
                {
                    if (monsterBattleControllerList[0][i] == null ||
                        monsterBattleControllerList[1][j] == null ||
                        monsterBattleControllerList[0][i].isDead ||
                        monsterBattleControllerList[1][j].isDead)
                    {
                        distanceEnemies[0][i][j] = -1f;
                        distanceEnemies[1][j][i] = -1f;
                    }
                    else
                    {
                        distanceEnemies[0][i][j] = Vector3.Distance(
                            monsterBattleControllerList[0][i].transform.localPosition,
                            monsterBattleControllerList[1][j].transform.localPosition);
                        distanceEnemies[1][j][i] = Vector3.Distance(
                            monsterBattleControllerList[0][i].transform.localPosition,
                            monsterBattleControllerList[1][j].transform.localPosition);
                    }
                }
            }
        }
    }

    public static float getDistanceBetween(Vector2 a, Vector2 b)
    {
        if (a.x == b.x)
        {
            // 아군간 거리
            return a.y > b.y ?
                distanceAllies[(int)a.x][(int)b.y][(int)a.y] :
                distanceAllies[(int)a.x][(int)a.y][(int)b.y];
        }
        else
        {
            return distanceEnemies[(int)a.x][(int)a.y][(int)b.y];
        }

    }

    // 현재 기준에서 가장 가까운 {아군, 적} idx
    public static int[] getClosestIndex(Vector2 oneToCheck)
    {
        int[] res = new int[2] { -1, -1 };
        float dis = 100f;
        for (int i = 0; i < distanceAllies[(int)oneToCheck.x][(int)oneToCheck.y].Length; i++)
        {
            if (distanceAllies[(int)oneToCheck.x][(int)oneToCheck.y][i] != -1f &&
                dis > distanceAllies[(int)oneToCheck.x][(int)oneToCheck.y][i])
            {
                dis = distanceAllies[(int)oneToCheck.x][(int)oneToCheck.y][i];
                res[0] = i;
            }
        }
        dis = 100f;
        for (int i = 0; i < distanceEnemies[(int)oneToCheck.x][(int)oneToCheck.y].Length; i++)
        {
            float tempDis = distanceEnemies[(int)oneToCheck.x][(int)oneToCheck.y][i];
            if (tempDis != -1f &&
                dis > tempDis)
            {
                dis = tempDis;
                res[1] = i;
            }
        }
        return res;
    }

    // cirterionDis 거리 안에서 아군 또는 적의 가까운 순의 idx
    public static int[] getIndexListByDistance(Vector2 entryNum, string targetType, float criterionDis, int numTarget)
    {
        float[] distArr;
        // 만약 본인 참조일 경우: 바로 본인만 반환
        if (targetType.Contains("SELF")) return new int[] { (int)entryNum.y, -1 };
        if (targetType.Contains("ALLY"))
        {
            distArr = distanceAllies[(int)entryNum.x][(int)entryNum.y];
        }
        else
        {
            distArr = distanceEnemies[(int)entryNum.x][(int)entryNum.y];
        }
        List<int> res = new List<int>();
        Dictionary<float, int> sort = new Dictionary<float, int>();
        for (int j = 0; j < distArr.Length; j++)
        {
            if (distArr[j] != -1f && criterionDis >= distArr[j])
            {
                sort[distArr[j]] = j;
            }
        }
        foreach (KeyValuePair<float, int> index in sort.OrderBy((i) => i.Key))
        {
            res.Add(index.Value);
        }
        // 만약 먼 순서일 경우: 역정렬
        if (targetType.Contains("FAR")) res.Reverse();
        List<int> resExt = new List<int>();
        if (res.Count <= numTarget)
        {
            resExt = res;
        }
        else
        {
            for (int i = 0; i < numTarget; i++)
            {
                resExt.Add(res[i]);
            }
        }
        return resExt.ToArray();
    }

    // 현재 위치에서 사정거리 criterionDis로 원형 안에 존재하는 모든 대상 인덱스 리스트
    public static int[] getTargetIdxListByDistance(Vector3 curPos, int targetSide, float criterionDis)
    {
        //Debug.Log(curPos);
        //Debug.Log(targetSide);
        //Debug.Log(criterionDis);
        List<int> res = new List<int>();
        for (int i = 0; i < monsterBattleControllerList[targetSide].Length; i++)
        {
            if (monsterBattleControllerList[targetSide][i] != null)
            {
                if (Vector3.Distance(curPos, monsterBattleControllerList[targetSide][i].transform.position) <= criterionDis)
                {
                    res.Add(i);
                }
            }
        }
        return res.ToArray();
    }

    // 투사체들을 해당 인덱스들을 향해 생성
    public static void executeProjectiles(List<ProjectileStat> projectileStats, float delayProjectile, int[] targetIdxList, Vector3 startingPos, int[] entryNum, int targetSide)
    {
        // 타겟 순서대로 실행
        foreach (int targetIdx in targetIdxList)
        {
            float d = 0f;
            // 투사체 순서대로 생성
            foreach (ProjectileStat projectileStat in projectileStats)
            {
                createProjectile(projectileStat, d, startingPos, monsterBattleControllerList[entryNum[0]][entryNum[1]], monsterBattleControllerList[targetSide][targetIdx], targetSide);
                d += delayProjectile;
            }
        }
    }

    public static void createProjectile(ProjectileStat projectileStat, float delay, Vector3 startingPos, MonsterBattleController casterController, MonsterBattleController targetController, int targetSide)
    {
        GameObject res = Instantiate(staticProjectilePrefab);
        res.transform.position = startingPos;
        res.GetComponent<ProjectileController>().initInfo(projectileStat, delay, casterController, targetController, targetSide);
    }

    public static void executeArea(Vector3 setPos, MonsterBattleController casterController, EffectAreaStat effectAreaStat, int targetSide)
    {
        GameObject areaObject = Instantiate(staticAreaPrefab);
        areaObject.transform.position = setPos;
        areaObject.GetComponent<AreaController>().initInfo(effectAreaStat, casterController, targetSide);
    }

    public static int calculateDamage(float amount, MonsterBattleInfo atkInfo, MonsterBattleInfo defInfo)
    {
        float pureDmg = amount * atkInfo.basic[BasicStatEnum.atk].amount * (
            100 / (100 + defInfo.basic[BasicStatEnum.def].amount)
            );
        pureDmg *= AttributesFunction.calculateAttributeAgainstWeight(atkInfo.element, defInfo.element);
        return (int)pureDmg;
    }
}
