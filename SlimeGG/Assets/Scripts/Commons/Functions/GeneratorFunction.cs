using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneratorFunction
{
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
}