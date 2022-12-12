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

    public void generateMonsters()
    {
        monsterInfos = new MonsterInfo[1];
        for (int i = 0; i < monsterInfos.Length; i++)
        {
            generateMonster(monsterInfos[i]);
        }
    }

    private void generateMonster(MonsterInfo monsterInfo)
    {
        GameObject newMonster = Instantiate(monsterBase);
        newMonster.transform.SetParent(baseTileSet.transform);
        newMonster.transform.localPosition = new Vector3(0f, 0f, 0f);
    }
}
