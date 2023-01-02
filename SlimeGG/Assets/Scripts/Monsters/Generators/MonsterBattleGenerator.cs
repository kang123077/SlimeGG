using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBattleGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject monsterBase;
    [SerializeField]
    private GameObject fieldGenerated;
    [SerializeField]
    private GameObject monsterUIBase;
    [SerializeField]
    private GameObject monsterInfoUIGenerated;

    // Update is called once per frame
    void Update()
    {
        if (!LocalStorage.BATTLE_SCENE_LOADING_DONE && LocalStorage.DICTIONARY_LOADING_DONE && LocalStorage.MONSTER_DATACALL_DONE)
        {
            fieldGenerated.GetComponent<FieldController>().initField(LocalDictionary.fields[FieldNameEnum.Normal]);
            initGeneration();
        }
    }

    public void initGeneration()
    {
        test4on4();

        fieldGenerated.GetComponent<FieldController>().setFieldInfoForMonsters();

        LocalStorage.BATTLE_SCENE_LOADING_DONE = true;
    }

    private void generateMonster(MonsterInfo monsterInfo, int side, int numPos)
    {
        GameObject newMonster = Instantiate(monsterBase);
        newMonster.GetComponent<MonsterBattleController>().initInfo(monsterInfo);
        LocalStorage.monsterBattleControllerList[side][numPos] = newMonster.GetComponent<MonsterBattleController>();
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
        MonsterInfo monsterInfo = LocalStorage.monsters[4];
        generateMonster(monsterInfo, 0, 0);
        monsterInfo = LocalStorage.monsters[5];
        generateMonster(monsterInfo, 1, 0);
    }

    private void test4on4()
    {
        MonsterInfo monsterInfo = LocalStorage.monsters[1];
        generateMonster(monsterInfo, 0, 0);
        monsterInfo = LocalStorage.monsters[0];
        generateMonster(monsterInfo, 1, 1);
        monsterInfo = LocalStorage.monsters[4];
        generateMonster(monsterInfo, 0, 2);
        monsterInfo = LocalStorage.monsters[4];
        generateMonster(monsterInfo, 0, 1);
        monsterInfo = LocalStorage.monsters[5];
        generateMonster(monsterInfo, 1, 0);
        monsterInfo = LocalStorage.monsters[2];
        generateMonster(monsterInfo, 1, 2);
        monsterInfo = LocalStorage.monsters[2];
        generateMonster(monsterInfo, 1, 3);
        monsterInfo = LocalStorage.monsters[1];
    }
}
