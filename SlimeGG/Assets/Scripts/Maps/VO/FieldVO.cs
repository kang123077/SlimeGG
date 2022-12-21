using System.Collections.Generic;
using UnityEngine;
public class FieldVO
{
    public FieldType fieldType;
    public string name;
    public int[] numberRestrictPerSide = new int[2];
    public Vector2[][] initPosList = new Vector2[2][];
    public Transform[][] monsterControllers = new Transform[2][];

    public FieldVO(int monsterCntL, int monsterCntR)
    {
        numberRestrictPerSide[0] = monsterCntL;
        numberRestrictPerSide[1] = monsterCntR;
        monsterControllers[0] = new Transform[monsterCntL];
        monsterControllers[1] = new Transform[monsterCntR];
        initPosList[0] = new Vector2[] { new Vector2(-2f, 0f), new Vector2(-1.7f, -1f) };
        initPosList[1] = new Vector2[] { new Vector2(1.7f, -1f), new Vector2(2f, 0f) };
    }
}