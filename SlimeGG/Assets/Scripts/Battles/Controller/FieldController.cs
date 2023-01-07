using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    public FieldInfo fieldInfo = null;
    private Transform monsterContainer = null;

    public void initField(FieldInfo fieldInfo)
    {
        this.fieldInfo = fieldInfo;
        BattleManager.monsterBattleControllerList[0] = new MonsterBattleController[fieldInfo.numberRestrictPerSide[0]];
        BattleManager.monsterBattleControllerList[1] = new MonsterBattleController[fieldInfo.numberRestrictPerSide[1]];

        for (int k = 0; k < 2; k++)
        {
            BattleManager.distanceAllies[k] = new float[fieldInfo.numberRestrictPerSide[k]][];
            for (int i = 0; i < fieldInfo.numberRestrictPerSide[k]; i++)
            {
                BattleManager.distanceAllies[k][i] = new float[fieldInfo.numberRestrictPerSide[k]];
            }
        }

        BattleManager.distanceEnemies[0] = new float[fieldInfo.numberRestrictPerSide[0]][];
        for (int i = 0; i < fieldInfo.numberRestrictPerSide[0]; i++)
        {
            BattleManager.distanceEnemies[0][i] = new float[fieldInfo.numberRestrictPerSide[1]];
        }
        BattleManager.distanceEnemies[1] = new float[fieldInfo.numberRestrictPerSide[1]][];
        for (int i = 0; i < fieldInfo.numberRestrictPerSide[1]; i++)
        {
            BattleManager.distanceEnemies[1][i] = new float[fieldInfo.numberRestrictPerSide[0]];
        }
    }

    public void setMonsterInPosition(Transform monster, int side, int[] setPos)
    {
        if (monsterContainer == null)
        {
            monsterContainer = transform.Find("Monster Container");
        }
        monster.SetParent(monsterContainer);
        monster.localPosition = new Vector3((side == 1 ? 1 : -1) * (1 + (setPos[0])), setPos[1], 0f);
    }

    public void setFieldInfoForMonsters()
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < fieldInfo.numberRestrictPerSide[i]; j++)
            {
                if (BattleManager.monsterBattleControllerList[i][j] != null)
                {
                    BattleManager.monsterBattleControllerList[i][j]
                        .setFieldInfo(
                            new Vector2(i, j)
                        );
                }
            }
        }
    }
}
