using System.Collections.Generic;
using System.Numerics;

public static class LocalDictionary
{
    public static Dictionary<string, MonsterDictionaryStat> speices =
        new Dictionary<string, MonsterDictionaryStat>();
    public static Dictionary<string, SkillStat> skills =
        new Dictionary<string, SkillStat>();
    public static Dictionary<string, FieldInfo> fields =
        new Dictionary<string, FieldInfo>();

    public static Dictionary<string, ItemDictionaryStat> items = 
        new Dictionary<string, ItemDictionaryStat>();
}