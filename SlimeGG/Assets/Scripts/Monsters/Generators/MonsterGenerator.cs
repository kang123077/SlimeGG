using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject monsterBase;
    private MonsterInfo[] monsterInfos;
    public GameObject baseTileSet;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void initGeneration()
    {
        MonsterInfo monsterInfo = new MonsterInfo(InfantType.Ore);
        generateMonster(monsterInfo);
        monsterInfo = new MonsterInfo(InfantType.Lava);
        generateMonster(monsterInfo);
        monsterInfo = new MonsterInfo(InfantType.Dark);
        generateMonster(monsterInfo);
        monsterInfo = new MonsterInfo(InfantType.Ore);
        generateMonster(monsterInfo);
    }

    private void generateMonster(MonsterInfo monsterInfo)
    {
        GameObject newMonster = Instantiate(monsterBase);
        newMonster.GetComponent<MonsterBaseController>().initInfo(monsterInfo);
        newMonster.GetComponent<MonsterBaseController>().assignMonsterToTileSet(baseTileSet.transform);
        newMonster.transform.localPosition = new Vector3(0f, 0f, 0f);
    }
}
