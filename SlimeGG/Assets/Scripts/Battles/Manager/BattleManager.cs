using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private GameObject penalForPause;
    [SerializeField]
    private GameObject monsterBase;
    [SerializeField]
    private FieldController fieldController;
    [SerializeField]
    public GameObject projectilePrefab;
    [SerializeField]
    public GameObject areaPrefab;
    [SerializeField]
    public InventoryManager inventoryManager;
    [SerializeField]
    public GameObject barrier;
    [SerializeField]
    private RewardController rewardController;

    public static GameObject staticProjectilePrefab;
    public static GameObject staticAreaPrefab;

    public static MonsterBattleController[][] monsterBattleControllerList =
        new MonsterBattleController[2][];
    public static float[][][] distanceAllies = new float[2][][];
    public static float[][][] distanceEnemies = new float[2][][];
    public static Vector2 fieldSize;
    private static List<ContentController> enemyEntry = new List<ContentController>();
    public static List<ContentController> allyEntry = new List<ContentController>();
    private int sideWin { get; set; }
    private static int curStage = 0;

    // Start is called before the first frame update
    void Start()
    {
        LocalStorage.CURRENT_SCENE = "Battle";
    }

    void Update()
    {
        switch (curStage)
        {
            case 0:
                // 내 필드 정보 불러오기
                // 적 필드 정보 + 몬스터 정보 불러오기
                staticProjectilePrefab = projectilePrefab;
                staticAreaPrefab = areaPrefab;
                loadBattleZoneInfo(0, "swamp");
                break;
            case 1:
                // 적 배치
                placeEnemyEntry();
                break;
            case 2:
                // 대기 <== 배치 완료 버튼 누를때까지
                // 아군 배치 <- 이건 유저가
                break;
            case 3:
                // 배치 완료 버튼 누름 = 전투용 객체들 생성
                // 배치 타일 제거
                // 전투 시작 버튼 활성화
                generateBattleInfos();
                truncateFieldTiles();
                break;
            case 4:
                // 대기 <== 전투 시작 누를때까지
                calculateDistance();
                break;
            case 5:
                // 대기 <== 전투 끝날 때까지
                // 전투 실시간 로직은 여기
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
                break;
            case 6:
                // 전투 종료
                // 리워드 보여주기
                Debug.Log($"Battle Finished:: Win => {sideWin} <=");
                openRewards();
                break;
            case 7:
                // 대기 <== 리워드 종료까지
                break;
            case 8:
                // 리워드 종료
                // 배틀씬 나가기
                Destroy(gameObject);
                break;
        }
    }

    private void openRewards()
    {
        openRewardMonster();
        curStage = 7;
    }
    private void openRewardMonster()
    {
        GetComponent<MainGameManager>().controllLoading(true, null, false);
        StartCoroutine(controlMonsterReward());
    }

    private IEnumerator controlMonsterReward()
    {
        rewardController.openMonsterReward();
        inventoryManager.getMonsterInfoController().GetComponent<ObjectMoveController>().distanceToMoveRatioToUnit = 11f;
        inventoryManager.toggleAll();
        yield return new WaitForSeconds(1f);
        while (LocalStorage.UIOpenStatus.reward)
        {
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine(controlItemReward());
    }

    private IEnumerator controlItemReward()
    {
        rewardController.openInventoryReward();
        yield return new WaitForSeconds(1f);
        while (LocalStorage.UIOpenStatus.reward)
        {
            yield return new WaitForSeconds(0.1f);
        }
        curStage = 8;
    }

    //private void generateEnemies()
    //{
    //    MonsterBattleInfo temp;
    //    int cnt = 0;
    //    foreach (ContentController enemyStat in enemyEntry)
    //    {
    //        temp = MonsterCommonFunction.generateMonsterBattleInfo(enemyStat);
    //        generateMonster(temp, 1, cnt, temp.entryPos);
    //        cnt++;
    //    }
    //}

    private void generateAllies()
    {
        for (int i = 0; i < monsterBattleControllerList[0].Length; i++)
        {
            if (monsterBattleControllerList[0][i] != null)
            {
                monsterBattleControllerList[0][i].setPlaceInfo(new Vector2(0, i));
            }
        }
    }

    private void clearField()
    {
        barrier.AddComponent<PolygonCollider2D>();
        //Destroy(enemyEntrySlot);
    }

    public void pauseOrResumeBattle()
    {
        LocalStorage.IS_GAME_PAUSE = !LocalStorage.IS_GAME_PAUSE;
        penalForPause.SetActive(LocalStorage.IS_GAME_PAUSE);
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

    public void setReady(GameObject btn)
    {
        if (monsterBattleControllerList[0][0] == null)
        {
            Debug.Log("몬스터를 한 마리 이상 설치해야 함");
            return;
        }
        /*
        if (monsterBattleControllerList[0].Length > entryLimit)
        {
            Debug.Log("몬스터 배치 한도를 초과함");
            return;
        }
        */
        btn.SetActive(false);
    }

    private void loadBattleZoneInfo(int level, string dungeonTheme)
    {
        callEnemyEntry("Test");
        curStage = 1;
    }

    private void callEnemyEntry(string entryPath)
    {
        foreach (MonsterSaveStat saveStat in
                        CommonFunctions.loadObjectFromJson<MonsterSaveStat[]>(
                            $"Assets/Resources/Jsons/Entries/{entryPath}"
                            ))
        {
            ContentController newEnemy = inventoryManager.generateMonsterContentController();
            newEnemy.initContent(GeneratorFunction.returnMonsterLiveStat(saveStat), false);
            enemyEntry.Add(newEnemy);
        }
    }

    private void placeEnemyEntry()
    {
        inventoryManager.setEntriable(true);
        foreach (ContentController enemy in enemyEntry)
        {
            float[] pos = enemy.monsterLiveStat.saveStat.entryPos;
            fieldController.enemyEntryGenerator.transform.GetChild((int)pos[0] + ((int)pos[1] * SettingVariables.Battle.entrySizeMax[0])).GetComponent<EntrySlotController>().installMonster(enemy);
        }
        curStage = 2;
    }

    public void controllFunctionOfButton(Transform btnTf)
    {
        Debug.Log(curStage);
        switch (curStage)
        {
            case 2:
                finishPlacement(btnTf);
                break;
            case 4:
                startBattle(btnTf);
                break;
        }
    }

    private void finishPlacement(Transform btnTf)
    {
        inventoryManager.setEntriable(false);
        inventoryManager.toggleAll();
        btnTf.GetChild(0).GetComponent<TextMeshProUGUI>().text = "전투 시작";
        curStage = 3;
    }

    private void generateBattleInfos()
    {
        generateMatrix();
        int cnt = 0;
        foreach (ContentController enemy in enemyEntry)
        {
            generateMonster(enemy, 1, cnt);
            cnt++;
        }
        cnt = 0;
        foreach (ContentController ally in allyEntry)
        {
            generateMonster(ally, 0, cnt);
        }
        curStage = 4;
    }

    private void generateMatrix()
    {
        int alliesCount = allyEntry.Count;
        int enemiesCount = enemyEntry.Count;
        monsterBattleControllerList[0] = new MonsterBattleController[alliesCount];
        monsterBattleControllerList[1] = new MonsterBattleController[enemiesCount];
        distanceAllies[0] = new float[alliesCount][];
        for (int i = 0; i < alliesCount; i++)
        {
            distanceAllies[0][i] = new float[alliesCount];
        }
        distanceAllies[1] = new float[enemiesCount][];
        for (int i = 0; i < enemiesCount; i++)
        {
            distanceAllies[1][i] = new float[enemiesCount];
        }

        distanceEnemies[0] = new float[alliesCount][];
        for (int i = 0; i < alliesCount; i++)
        {
            distanceEnemies[0][i] = new float[enemiesCount];
        }
        distanceEnemies[1] = new float[enemiesCount][];
        for (int i = 0; i < enemiesCount; i++)
        {
            distanceEnemies[1][i] = new float[alliesCount];
        }
    }

    private void generateMonster(ContentController controller, int side, int numPos)
    {
        GameObject newMonster = Instantiate(monsterBase);
        newMonster.GetComponent<MonsterBattleController>().initInfo(MonsterCommonFunction.generateMonsterBattleInfo(controller.monsterLiveStat), fieldController.transform.Find("Monster Container"));
        newMonster.GetComponent<MonsterBattleController>().setPlaceInfo(new Vector2(side, numPos));
        newMonster.transform.position = controller.transform.position;
        newMonster.transform.localScale = Vector3.one;
        newMonster.transform.localPosition = new Vector3(
            newMonster.transform.localPosition.x,
            newMonster.transform.localPosition.y,
            0f);

        monsterBattleControllerList[side][numPos] = newMonster.GetComponent<MonsterBattleController>();
    }

    private void truncateFieldTiles()
    {
        Destroy(fieldController.enemyEntryGenerator.gameObject);
        Destroy(fieldController.allyEntryGenerator.gameObject);
        enemyEntry.Clear();
        allyEntry.Clear();
    }

    private void startBattle(Transform btnTf)
    {
        Destroy(btnTf.gameObject);
        curStage = 5;
        LocalStorage.IS_GAME_PAUSE = false;
    }

    public void finishBattle()
    {
        LocalStorage.IS_BATTLE_FINISH = true;
        curStage = 6;
    }

    public static int getCurStage()
    {
        return curStage;
    }
}