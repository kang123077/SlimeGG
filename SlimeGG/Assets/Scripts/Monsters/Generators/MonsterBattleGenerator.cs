using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBattleGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject monsterBase;
    [SerializeField]
    private GameObject fieldGenerated;
    private MonsterInfo[] monsterInfos = null;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (fieldGenerated.GetComponent<FieldController>().monstersInBattle == null && monsterInfos == null &&
            LocalStorage.DICTIONARY_LOADING_DONE && LocalStorage.MONSTER_DATACALL_DONE)
        {
            fieldGenerated.GetComponent<FieldController>().initField(LocalDictionary.fields[FieldNameEnum.Normal]);
            initGeneration();
        }
    }

    public void initGeneration()
    {
        monsterInfos = new MonsterInfo[4];
        MonsterInfo monsterInfo = LocalStorage.monsters[0];
        generateMonster(monsterInfo, 0, 0);
        monsterInfo = LocalStorage.monsters[0];
        generateMonster(monsterInfo, 1, 1);
        monsterInfo = LocalStorage.monsters[2];
        generateMonster(monsterInfo, 0, 2);
        monsterInfo = LocalStorage.monsters[1];
        generateMonster(monsterInfo, 0, 1);
        monsterInfo = LocalStorage.monsters[1];
        generateMonster(monsterInfo, 1, 0);
        monsterInfo = LocalStorage.monsters[2];
        generateMonster(monsterInfo, 1, 2);
        monsterInfo = LocalStorage.monsters[2];
        generateMonster(monsterInfo, 1, 3);
        monsterInfo = LocalStorage.monsters[3];
        generateMonster(monsterInfo, 0, 3);

        fieldGenerated.GetComponent<FieldController>().setFieldInfoForMonsters();
        LocalStorage.BATTLE_SCENE_LOADING_DONE = true;
    }

    private void generateMonster(MonsterInfo monsterInfo, int side, int numPos)
    {
        GameObject newMonster = Instantiate(monsterBase);
        newMonster.GetComponent<MonsterBattleController>().initInfo(monsterInfo);
        LocalStorage.monsterBattleControllerList[side].Add(newMonster.GetComponent<MonsterBattleController>());
        fieldGenerated.GetComponent<FieldController>().setMonsterInPosition(newMonster.transform, side, numPos);
    }
}
