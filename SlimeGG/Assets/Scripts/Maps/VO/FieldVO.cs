using System.Collections.Generic;
using UnityEngine;
public class FieldVO
{
    public FieldType fieldType;
    public string name;
    public int numberRestrictPerSide;
    public List<Vector2> initPosList;

    public FieldVO()
    {
        numberRestrictPerSide = 1;
        initPosList = new List<Vector2>() { new Vector2(-2f, 0f), new Vector2(2f, 0f) };
    }
} 