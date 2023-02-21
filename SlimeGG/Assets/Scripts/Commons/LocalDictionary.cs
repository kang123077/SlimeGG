using System.Collections.Generic;
using System.Numerics;

public static class LocalDictionary
{
    public static Dictionary<string, MonsterDictionaryStat> speicies =
        new Dictionary<string, MonsterDictionaryStat>();
    public static Dictionary<string, SkillStat> skills =
        new Dictionary<string, SkillStat>();

    public static Dictionary<string, ItemDictionaryStat> items =
        new Dictionary<string, ItemDictionaryStat>();

    public static List<List<MonsterDictionaryStat>> speicesByTier = new List<List<MonsterDictionaryStat>>()
    {
        null,
        new List<MonsterDictionaryStat>(),
        new List<MonsterDictionaryStat>(),
        new List<MonsterDictionaryStat>(),
        new List<MonsterDictionaryStat>(),
    };
    public static List<List<ItemDictionaryStat>> itemsByTier = new List<List<ItemDictionaryStat>>()
    {
        null,
        new List<ItemDictionaryStat>(),
        new List<ItemDictionaryStat>(),
        new List<ItemDictionaryStat>(),
        new List<ItemDictionaryStat>(),
    };

    public static Dictionary<string, DungeonStat> dungeons = new Dictionary<string, DungeonStat>();

    public static Dictionary<string, FieldSaveStat> fields = new Dictionary<string, FieldSaveStat>();
    public static Dictionary<string, List<MonsterSaveStat>> entries = new Dictionary<string, List<MonsterSaveStat>>();
}