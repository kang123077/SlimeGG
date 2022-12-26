using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    public FieldInfo fieldInfo;
    private Transform monsterContainer = null;
    public Transform[][] monstersInBattle;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void initField(FieldInfo fieldInfo)
    {
        this.fieldInfo = fieldInfo;
        monstersInBattle = new Transform[2][];
        monstersInBattle[0] = new Transform[fieldInfo.numberRestrictPerSide[0]];
        monstersInBattle[1] = new Transform[fieldInfo.numberRestrictPerSide[1]];
    }

    public void setMonsterInPosition(Transform monster, int side, int numPos)
    {
        if (monsterContainer == null)
        {
            monsterContainer = transform.Find("Monster Container");
        }
        monstersInBattle[side][numPos] = monster;
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
                monstersInBattle[i][j].GetComponent<MonsterBattleController>().setFieldInfo(new Vector2(i, j), monstersInBattle[i], monstersInBattle[i == 0 ? 1 : 0]);
            }
        }
    }
}
