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
        MonsterInfo monsterInfo = new MonsterInfo("M L 0", SpeciesName.Ore);
        generateMonster(monsterInfo, 0, 0);
        monsterInfo = new MonsterInfo("M L 1", SpeciesName.Ore);
        generateMonster(monsterInfo, 0, 1);
        monsterInfo = new MonsterInfo("M R 0", SpeciesName.Ore);
        generateMonster(monsterInfo, 1, 0);
        monsterInfo = new MonsterInfo("M R 2", SpeciesName.Ore);
        generateMonster(monsterInfo, 1, 1);

        fieldGenerated.GetComponent<FieldController>().setFieldInfoForMonsters();
    }

    private void generateMonster(MonsterInfo monsterInfo, int side, int numPos)
    {
        GameObject newMonster = Instantiate(monsterBase);
        newMonster.GetComponent<MonsterBattleController>().initInfo(monsterInfo);
        fieldGenerated.GetComponent<FieldController>().setMonsterInPosition(newMonster.transform, side, numPos);
    }
}
