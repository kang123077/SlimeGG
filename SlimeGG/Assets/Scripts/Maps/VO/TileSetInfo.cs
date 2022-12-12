using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSetInfo
{
    public bool isFixed;
    public TileInfo[] tileInfos;
    public TileType tileType = TileType.Pasture;
    public TileShape tileShape = TileShape.Single;
    public Vector2 adjCoor = new Vector2(-1f, -1f);
    public TileSetInfo(TileShape tileShape)
    {
        this.tileShape = tileShape;
        isFixed = false;
        setTileCoorsFromTileType(tileShape);
    }
    public TileSetInfo(TileType tileType, TileShape tileShape)
    {
        this.tileShape = tileShape;
        this.tileType = tileType;
        isFixed = false;
        setTileCoorsFromTileType(tileShape);
    }
    public TileSetInfo(TileType tileType, bool isFixed, TileShape tileShape)
    {
        this.tileShape = tileShape;
        this.tileType = tileType;
        this.isFixed = isFixed;
        setTileCoorsFromTileType(tileShape);
    }

    private void setTileCoorsFromTileType(TileShape tileShape)
    {
        switch (tileShape)
        {
            case TileShape.Single:
                tileInfos = new TileInfo[1];
                tileInfos[0] = new TileInfo(0, 0);
                adjCoor = new Vector2(0f, 0f);
                break;
            case TileShape.StraightVertical2:
                break;
            case TileShape.StraightHorizontal2:
                break;
            case TileShape.StraightVertical3:
                break;
            case TileShape.StraightHorizontal3:
                tileInfos = new TileInfo[3];
                tileInfos[0] = new TileInfo(0, 0);
                tileInfos[1] = new TileInfo(1, 0);
                tileInfos[2] = new TileInfo(2, 0);
                adjCoor = new Vector2(2f, 0f);
                break;
            case TileShape.CurvedClock3:
                break;
            case TileShape.CurvedClock4:
                tileInfos = new TileInfo[4];
                tileInfos[0] = new TileInfo(0, 0);
                tileInfos[1] = new TileInfo(1, 0);
                tileInfos[2] = new TileInfo(1, 1);
                tileInfos[3] = new TileInfo(1, 2);
                adjCoor = new Vector2(1.5f, -2f);
                break;
            case TileShape.CurvedCounterClock3:
                break;
            case TileShape.CurvedCounterClock4:
                break;
            case TileShape.ZigZagR3:
                break;
            case TileShape.ZigZagL3:
                break;
            case TileShape.Triangle3:
                break;
            case TileShape.TriangleTailR4:
                break;
            case TileShape.TriangleTailL4:
                break;
            case TileShape.TriangleReverse3:
                tileInfos = new TileInfo[3];
                tileInfos[0] = new TileInfo(0, 0);
                tileInfos[1] = new TileInfo(1, 0);
                tileInfos[2] = new TileInfo(0, 1);
                adjCoor = new Vector2(1f, -1f);
                break;
            case TileShape.TriangleReverseTailR4:
                break;
            case TileShape.TriangleReverseTailL4:
                break;
            case TileShape.Diamond:
                break;
            case TileShape.ParallR4:
                break;
            case TileShape.ParallL4:
                break;
        }
    }
}
