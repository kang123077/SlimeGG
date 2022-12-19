using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    private FieldVO fieldVO = new FieldVO();
    private Transform monsterContainer = null;

    // Start is called before the first frame update
    void Start()
    {
        //fieldVO.numberRestrictPerSide = 1;
        //fieldVO.initPosList = new List<Vector2>() { new Vector2(-2f, 0f), new Vector2(2f, 0f) };

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setMonsterInPosition(Transform monster, int side, int numPos)
    {
        if (monsterContainer == null)
        {
            monsterContainer = transform.Find("Monster Container");
        }
        monster.SetParent(monsterContainer);
        monster.localPosition = fieldVO.initPosList[side][numPos];
    }
}
