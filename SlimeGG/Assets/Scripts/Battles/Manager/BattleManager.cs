using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

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
    public static GameObject projectilePrefab;

    public static MonsterBattleController[][] monsterBattleControllerList =
        new MonsterBattleController[2][];
    public static float[][][] distanceAllies = new float[2][][];
    public static float[][][] distanceEnemies = new float[2][][];
    public static bool isBattleReady = false;
    public static Vector2 fieldSize;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isBattleReady && LocalStorage.DICTIONARY_LOADING_DONE && LocalStorage.MONSTER_DATACALL_DONE)
        {
            initField("Normal");
            initGeneration();
            isBattleReady = true;
        }
        if (isBattleReady && !LocalStorage.IS_GAME_PAUSE)
        {
            calculateDistance();
            bool isOneSideAllDead;
            foreach (MonsterBattleController[] sideList in monsterBattleControllerList)
            {
                isOneSideAllDead = true;
                foreach (MonsterBattleController battleController in sideList)
                {
                    isOneSideAllDead = isOneSideAllDead && battleController.isDead;
                }
                if (isOneSideAllDead) finishBattle();
            }
        }
    }
    private void generateMonster(MonsterBattleInfo monsterInfo, int side, int numPos)
    {
        GameObject newMonster = Instantiate(monsterBase);
        newMonster.GetComponent<MonsterBattleController>().initInfo(monsterInfo);
        monsterBattleControllerList[side][numPos] = newMonster.GetComponent<MonsterBattleController>();

        fieldGenerated.GetComponent<FieldController>().setMonsterInPosition(newMonster.transform, side, numPos);

        GameObject newMonsterInfoUI = Instantiate(monsterUIBase);
        newMonsterInfoUI.transform.SetParent(monsterInfoUIGenerated.transform.Find($"{side}"));
        newMonsterInfoUI
            .GetComponent<MonsterBattleUIController>()
            .initInfo(newMonster.GetComponent<MonsterBattleController>(),
            side, numPos);

    }

    private void test1on1()
    {
        MonsterBattleInfo newMon = MonsterCommonFunction.generateMonsterBattleInfo(LocalStorage.monsters[0], LocalDictionary.speices[LocalStorage.monsters[0].accuSpecies.Last()]);
        generateMonster(newMon, 0, 0);
        newMon = MonsterCommonFunction.generateMonsterBattleInfo(LocalStorage.monsters[1], LocalDictionary.speices[LocalStorage.monsters[1].accuSpecies.Last()]);
        generateMonster(newMon, 1, 0);
    }
    private void initField(string fieldName)
    {
        float[] size = LocalDictionary.fields[fieldName].size.ToArray();
        fieldSize = new Vector2(size[0], size[1]);
        fieldGenerated.GetComponent<FieldController>().initField(LocalDictionary.fields[fieldName]);
    }
    public void initGeneration()
    {
        // 몬스터 생성
        test1on1();

        fieldGenerated.GetComponent<FieldController>().setFieldInfoForMonsters();
        calculateDistance();
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

    public static int[] getIndexListByDistance(Vector2 entryNum, bool isEnemies, float criterionDis)
    {
        float[] distArr;
        if (!isEnemies)
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
        return res.ToArray();
    }
    public static void createProjectile(SkillStat skillStat, Vector2 entryNum, int target)
    {
        GameObject res = Instantiate(projectilePrefab);
        res.transform.position = monsterBattleControllerList[(int)entryNum.x][(int)entryNum.y].transform.position;
        res.GetComponent<ProjectileController>().initInfo(skillStat, entryNum, target);
    }
}
