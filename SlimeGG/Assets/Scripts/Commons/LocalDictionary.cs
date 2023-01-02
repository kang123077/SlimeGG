using System.Collections.Generic;
using System.Numerics;

public static class LocalDictionary
{
    public static Dictionary<string, MonsterSpeciesVO> speices =
        new Dictionary<string, MonsterSpeciesVO>();
    public static Dictionary<string, SkillStat> skills =
        new Dictionary<string, SkillStat>();
    public static Dictionary<TileSetShapeEnum, TileSetShapeStat> tileSetCoors =
        new Dictionary<TileSetShapeEnum, TileSetShapeStat>();
    public static Dictionary<TileSetNameEnum, TileSetInfo> tileSets =
        new Dictionary<TileSetNameEnum, TileSetInfo>();
    public static Dictionary<string, FieldInfo> fields =
        new Dictionary<string, FieldInfo>();
}