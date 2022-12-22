using System.Collections.Generic;
using System.Numerics;

public static class LocalDictionary
{
    public static Dictionary<MonsterSpeciesEnum, MonsterSpeciesInfo> monsters =
        new Dictionary<MonsterSpeciesEnum, MonsterSpeciesInfo>();
    public static Dictionary<MonsterSkillEnum, SkillStat> skills =
        new Dictionary<MonsterSkillEnum, SkillStat>();
    public static Dictionary<TileSetShapeEnum, TileSetShapeStat> tileSetCoors =
        new Dictionary<TileSetShapeEnum, TileSetShapeStat>();
    public static Dictionary<TileSetNameEnum, TileSetInfo> tileSets =
        new Dictionary<TileSetNameEnum, TileSetInfo>();
    public static Dictionary<FieldNameEnum, FieldInfo> fields =
        new Dictionary<FieldNameEnum, FieldInfo>();
}