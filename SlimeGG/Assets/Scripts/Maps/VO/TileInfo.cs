using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileInfo
{
    public int x, y;
    public TileInfo()
    {
        x = 0;
        y = 0;
    }
    public TileInfo(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
