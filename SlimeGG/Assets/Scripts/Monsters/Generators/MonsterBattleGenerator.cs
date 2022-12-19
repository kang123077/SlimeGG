using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBattleGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject monsterBase;
    [SerializeField]
    private GameObject fieldGenerated;
    private MonsterInfo[] monsterInfos;
    // Start is called before the first frame update
    void Start()
    {
        initGeneration();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void initGeneration()
    {
        MonsterInfo monsterInfo = new MonsterInfo(GrowthState.Infant, "Ore");
        generateMonster(monsterInfo, 0);
        monsterInfo = new MonsterInfo(GrowthState.Infant, "Ore");
        generateMonster(monsterInfo, 1);
        //monsterInfo = new MonsterInfo(InfantType.Lava);
        //generateMonster(monsterInfo);
        //monsterInfo = new MonsterInfo(InfantType.Dark);
        //generateMonster(monsterInfo);
        //monsterInfo = new MonsterInfo(InfantType.Ore);
        //generateMonster(monsterInfo);
    }

    private void generateMonster(MonsterInfo monsterInfo, int numPos)
    {
        GameObject newMonster = Instantiate(monsterBase);
        newMonster.GetComponent<MonsterBattleController>().initInfo(monsterInfo);
        fieldGenerated.GetComponent<FieldController>().setMonsterInPosition(newMonster.transform, numPos);
    }
}
