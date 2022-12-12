using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSetInfo
{
    public bool isFixed;
    public TileInfo[] tileInfos;
    public TileType tileType = TileType.Pasture;
    public TileSetInfo(TileInfo[] tileInfos)
    {
        this.tileInfos = tileInfos;
        this.isFixed = false;
    }
    public TileSetInfo(TileInfo[] tileInfos, TileType tileType)
    {
        this.tileInfos = tileInfos;
        this.tileType = tileType;
        this.isFixed = false;
    }
    public TileSetInfo(TileInfo[] tileInfos, TileType tileType, bool isFixed)
    {
        this.tileInfos = tileInfos;
        this.tileType = tileType;
        this.isFixed = isFixed;
    }
}
