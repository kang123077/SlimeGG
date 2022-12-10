using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSetInfo
{
    public TileInfo[] tileInfos;
    public TileType tileType = TileType.Pasture;
    public TileSetInfo(TileInfo[] tileInfos)
    {
        this.tileInfos = tileInfos;
    }
    public TileSetInfo(TileInfo[] tileInfos, TileType tileType)
    {
        this.tileInfos = tileInfos;
        this.tileType = tileType;
    }
}
