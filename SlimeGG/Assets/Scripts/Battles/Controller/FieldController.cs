using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    [SerializeField]
    public GridCellGenerator allyEntryGenerator, enemyEntryGenerator;
    public FieldInfo fieldInfo = null;
    private Transform monsterContainer = null;

    private void Start()
    {
    }

    //public void initField(FieldInfo fieldInfo)
    //{
    //    this.fieldInfo = fieldInfo;
    //    int alliesCount = 6;
    //    int enemiesCount = 8;
    //    BattleManager.monsterBattleControllerList[0] = new MonsterBattleController[alliesCount];
    //    BattleManager.monsterBattleControllerList[1] = new MonsterBattleController[enemiesCount];

    //    BattleManager.distanceAllies[0] = new float[alliesCount][];
    //    for (int i = 0; i < alliesCount; i++)
    //    {
    //        BattleManager.distanceAllies[0][i] = new float[alliesCount];
    //    }
    //    BattleManager.distanceAllies[1] = new float[enemiesCount][];
    //    for (int i = 0; i < enemiesCount; i++)
    //    {
    //        BattleManager.distanceAllies[1][i] = new float[enemiesCount];
    //    }

    //    BattleManager.distanceEnemies[0] = new float[alliesCount][];
    //    for (int i = 0; i < alliesCount; i++)
    //    {
    //        BattleManager.distanceEnemies[0][i] = new float[enemiesCount];
    //    }
    //    BattleManager.distanceEnemies[1] = new float[enemiesCount][];
    //    for (int i = 0; i < enemiesCount; i++)
    //    {
    //        BattleManager.distanceEnemies[1][i] = new float[alliesCount];
    //    }
    //}

    //public void setMonsterInPosition(Transform monster)
    //{
    //    if (monsterContainer == null)
    //    {
    //        monsterContainer = transform.Find("Monster Container");
    //    }
    //    monster.SetParent(monsterContainer);
    //}

    //public void setMonsterInPosition(Transform monster, int side, float[] setPos)
    //{
    //    setMonsterInPosition(monster);
    //    monster.localPosition = new Vector3((side == 1 ? 1 : -1) * (1 + (setPos[0])), setPos[1], 0f);
    //}
}
