using System.Collections.Generic;
using UnityEngine;
public class FieldVO
{
    public FieldType fieldType;
    public string name;
    public int[] numberRestrictPerSide = new int[2];
    public List<Vector2>[] initPosList = new List<Vector2>[2];

    public FieldVO()
    {
        numberRestrictPerSide[0] = 2;
        numberRestrictPerSide[1] = 2;
        initPosList[0] = new List<Vector2>() { new Vector2(-2f, 0f), new Vector2(-1.7f, -1f) };
        initPosList[1] = new List<Vector2>() { new Vector2(1.7f, -1f), new Vector2(2f, 0f) };
    }
}