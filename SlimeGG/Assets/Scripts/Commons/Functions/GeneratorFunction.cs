using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneratorFunction
{
    private static string[] hexCode = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "a", "b", "c", "d", "e", };
    public static void generateMonsterLiveStat(MonsterSaveStat saveStat)
    {
        LocalStorage.Live.monsters[saveStat.id] = returnMonsterLiveStat(saveStat);
    }

    public static MonsterLiveStat returnMonsterLiveStat(MonsterSaveStat saveStat)
    {
        MonsterLiveStat res = new MonsterLiveStat();
        res.saveStat = saveStat;
        res.dictionaryStat = LocalDictionary.speicies[saveStat.speicie];
        res.itemStatList = new Dictionary<string, ItemLiveStat>();
        return res;
    }

    public static ItemLiveStat returnItemLiveStat(ItemSaveStat saveStat)
    {
        ItemLiveStat res = new ItemLiveStat();
        res.saveStat = saveStat;
        res.dictionaryStat = LocalDictionary.items[saveStat.itemName];
        return res;
    }

    public static void generateItemLiveStat(ItemSaveStat saveStat)
    {
        ItemLiveStat res = new ItemLiveStat();
        res.saveStat = saveStat;
        res.dictionaryStat = LocalDictionary.items[saveStat.itemName];
        if (saveStat.equipMonsterId != null && LocalStorage.Live.monsters.ContainsKey(saveStat.equipMonsterId))
        {
            LocalStorage.Live.monsters[saveStat.equipMonsterId].itemStatList[saveStat.id] = res;
        }
        else
        {
            LocalStorage.Live.items[saveStat.id] = res;
        }
    }
    public static MonsterLiveStat generateMonsterLiveStatFromDictionaryStat(MonsterDictionaryStat monsterDictionaryStat)
    {
        return returnMonsterLiveStat(generateMonsterSaveStat(monsterDictionaryStat));
    }
    private static MonsterSaveStat generateMonsterSaveStat(MonsterDictionaryStat monsterDictionaryStat)
    {
        MonsterSaveStat res = new MonsterSaveStat();
        res.exp = new List<ElementStat>();
        res.exp.AddRange(monsterDictionaryStat.exp);
        res.entryPos = null;
        res.id = generateRandomcode();
        res.speicie = monsterDictionaryStat.name;
        return res;
    }
    public static ItemLiveStat generateItemLiveStatFromDictionaryStat(ItemDictionaryStat itemDictionaryStat)
    {
        return returnItemLiveStat(generateItemSaveStat(itemDictionaryStat));
    }
    private static ItemSaveStat generateItemSaveStat(ItemDictionaryStat itemDictionaryStat)
    {
        ItemSaveStat res = new ItemSaveStat();
        res.equipMonsterId = null;
        res.id = generateRandomcode();
        res.itemName = itemDictionaryStat.name;
        return res;
    }

    private static string generateRandomcode()
    {
        string res = string.Empty;
        for (int i = 0; i < 8; i++)
        {
            int randInt = Random.Range(0, 16);
            res += hexCode[randInt];
        }
        return res;
    }
}