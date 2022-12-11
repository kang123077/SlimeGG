using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject monsterBase;
    private MonsterInfo[] monsterInfos;
    // Start is called before the first frame update
    void Start()
    {
        monsterInfos = new MonsterInfo[1];
        for (int i = 0; i < monsterInfos.Length; i++)
        {
            generateMonster(monsterInfos[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void generateMonster(MonsterInfo monsterInfo)
    {
        GameObject newMonster = Instantiate(monsterBase);
        newMonster.transform.SetParent(transform);
        newMonster.transform.localPosition = new Vector3(0f, 0f, 0f);
    }
}
