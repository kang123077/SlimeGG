using System.Collections;
using System.Collections.Generic;

public static class GeneratorFunction
{
    public static void generateMonsterLiveStat(MonsterSaveStat saveStat)
    {
        MonsterLiveStat res = new MonsterLiveStat();
        res.saveStat = saveStat;
        res.dictionaryStat = LocalDictionary.speices[saveStat.speice];
        res.itemStatList = new Dictionary<string, ItemLiveStat>();
        LocalStorage.Live.monsters[saveStat.id] = res;
    }

    public static void generateItemLiveStat(ItemSaveStat saveStat)
    {
        ItemLiveStat res = new ItemLiveStat();
        res.saveStat = saveStat;
        res.dictionaryStat = LocalDictionary.items[saveStat.itemName];
        if (LocalStorage.Live.monsters.ContainsKey(saveStat.equipMonsterId))
        {
            LocalStorage.Live.monsters[saveStat.equipMonsterId].itemStatList[saveStat.id] = res;
        }
        else
        {
            LocalStorage.Live.items[saveStat.id] = res;
        }
    }
}