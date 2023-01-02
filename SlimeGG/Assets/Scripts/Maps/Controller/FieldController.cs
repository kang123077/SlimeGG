using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    public FieldInfo fieldInfo = null;
    private Transform monsterContainer = null;

    private static float[][][] distanceAllies = new float[2][][];
    private static float[][][] distanceEnemies = new float[2][][];

    public bool isDistanceReady = false;

    [SerializeField]
    public static GameObject projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isDistanceReady)
        {
            calculateDistance();
        }
    }

    public void initField(FieldInfo fieldInfo)
    {
        this.fieldInfo = fieldInfo;
        LocalStorage.monsterBattleControllerList[0] = new MonsterBattleController[fieldInfo.numberRestrictPerSide[0]];
        LocalStorage.monsterBattleControllerList[1] = new MonsterBattleController[fieldInfo.numberRestrictPerSide[1]];

        for (int k = 0; k < 2; k++)
        {
            distanceAllies[k] = new float[fieldInfo.numberRestrictPerSide[k]][];
            for (int i = 0; i < fieldInfo.numberRestrictPerSide[k]; i++)
            {
                distanceAllies[k][i] = new float[fieldInfo.numberRestrictPerSide[k]];
            }
        }

        distanceEnemies[0] = new float[fieldInfo.numberRestrictPerSide[0]][];
        for (int i = 0; i < fieldInfo.numberRestrictPerSide[0]; i++)
        {
            distanceEnemies[0][i] = new float[fieldInfo.numberRestrictPerSide[1]];
        }
        distanceEnemies[1] = new float[fieldInfo.numberRestrictPerSide[1]][];
        for (int i = 0; i < fieldInfo.numberRestrictPerSide[1]; i++)
        {
            distanceEnemies[1][i] = new float[fieldInfo.numberRestrictPerSide[0]];
        }
    }

    public void setMonsterInPosition(Transform monster, int side, int numPos)
    {
        if (monsterContainer == null)
        {
            monsterContainer = transform.Find("Monster Container");
        }
        monster.SetParent(monsterContainer);
        List<float> initPos = fieldInfo.initPosList[side][numPos];
        monster.localPosition = new Vector3(initPos[0], initPos[1], 0f);
    }

    public void setFieldInfoForMonsters()
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < fieldInfo.numberRestrictPerSide[i]; j++)
            {
                if (LocalStorage.monsterBattleControllerList[i][j] != null)
                {
                    LocalStorage.monsterBattleControllerList[i][j]
                        .setFieldInfo(
                            new Vector2(fieldInfo.size[0], fieldInfo.size[1]),
                            new Vector2(i, j)
                        );
                }
            }
        }
        isDistanceReady = true;
    }

    private void calculateDistance()
    {
        for (int k = 0; k < 2; k++)
        {
            for (int i = 0; i < LocalStorage.monsterBattleControllerList[k].Length; i++)
            {
                for (int j = 0; j < LocalStorage.monsterBattleControllerList[k].Length; j++)
                {
                    if (distanceAllies[k][i][j] != -1f)
                    {
                        if (LocalStorage.monsterBattleControllerList[k][i] == null ||
                            LocalStorage.monsterBattleControllerList[k][j] == null ||
                            LocalStorage.monsterBattleControllerList[k][i].isDead ||
                            LocalStorage.monsterBattleControllerList[k][j].isDead ||
                            i == j)
                        {
                            distanceAllies[k][i][j] = -1f;
                        }
                        else
                        {
                            distanceAllies[k][i][j] = Vector3.Distance(
                                LocalStorage.monsterBattleControllerList[k][i].transform.localPosition,
                                LocalStorage.monsterBattleControllerList[k][j].transform.localPosition);
                        }
                    }
                }
            }
        }

        for (int i = 0; i < LocalStorage.monsterBattleControllerList[0].Length; i++)
        {
            for (int j = 0; j < LocalStorage.monsterBattleControllerList[1].Length; j++)
            {
                if (distanceEnemies[0][i][j] != -1f)
                {
                    if (LocalStorage.monsterBattleControllerList[0][i] == null ||
                        LocalStorage.monsterBattleControllerList[1][j] == null ||
                        LocalStorage.monsterBattleControllerList[0][i].isDead ||
                        LocalStorage.monsterBattleControllerList[1][j].isDead)
                    {
                        distanceEnemies[0][i][j] = -1f;
                        distanceEnemies[1][j][i] = -1f;
                    }
                    else
                    {
                        distanceEnemies[0][i][j] = Vector3.Distance(
                            LocalStorage.monsterBattleControllerList[0][i].transform.localPosition,
                            LocalStorage.monsterBattleControllerList[1][j].transform.localPosition);
                        distanceEnemies[1][j][i] = Vector3.Distance(
                            LocalStorage.monsterBattleControllerList[0][i].transform.localPosition,
                            LocalStorage.monsterBattleControllerList[1][j].transform.localPosition);
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
        res.transform.position = LocalStorage.monsterBattleControllerList[(int)entryNum.x][(int)entryNum.y].transform.position;
        res.GetComponent<ProjectileController>().initInfo(skillStat, entryNum, target);
    }
}
