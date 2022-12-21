using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject monsterBase;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!LocalStorage.MONSTER_LOADING_DONE && LocalStorage.TILESET_LOADING_DONE && LocalStorage.MONSTER_DATACALL_DONE)
        {
            LocalStorage.monsters.ForEach((monsterInfo) =>
            {
                generateMonster(monsterInfo);
            });
            LocalStorage.MONSTER_LOADING_DONE = true;
        }
    }

    private void generateMonster(MonsterInfo monsterInfo)
    {
        GameObject newMonster = Instantiate(monsterBase);
        newMonster.GetComponent<MonsterBaseController>().initInfo(monsterInfo);
        newMonster.GetComponent<MonsterBaseController>().assignMonsterToTileSet(LocalStorage.tileSetTransforms[monsterInfo.installedPosition], false);
        newMonster.transform.localPosition = new Vector3(0f, 0f, 0f);
    }
}
