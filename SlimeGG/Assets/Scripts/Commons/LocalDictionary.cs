using System.Collections.Generic;
using System.Numerics;

public static class LocalDictionary
{
    public static Dictionary<MonsterSpeciesEnum, MonsterSpeciesInfo> monsters =
        new Dictionary<MonsterSpeciesEnum, MonsterSpeciesInfo>();
    public static Dictionary<TileSetShapeEnum, TileSetShapeStat> tileSetCoors =
        new Dictionary<TileSetShapeEnum, TileSetShapeStat>();
}