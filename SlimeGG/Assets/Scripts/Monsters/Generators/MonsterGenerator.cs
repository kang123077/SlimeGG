using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject monsterBase;
    public GameObject baseTileSet;
    private bool isMonsterInitialized = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMonsterInitialized)
        {
            if (LocalStorage.monsters != null)
            {
                initGeneration();
            }
        }
    }
    public void initGeneration()
    {
        LocalStorage.monsters.ForEach((monsterInfo) =>
        {
            generateMonster(monsterInfo);
        });
        isMonsterInitialized = true;
    }

    private void generateMonster(MonsterInfo monsterInfo)
    {
        GameObject newMonster = Instantiate(monsterBase);
        newMonster.GetComponent<MonsterBaseController>().initInfo(monsterInfo);
        newMonster.GetComponent<MonsterBaseController>().assignMonsterToTileSet(baseTileSet.transform);
        newMonster.transform.localPosition = new Vector3(0f, 0f, 0f);
    }
}
