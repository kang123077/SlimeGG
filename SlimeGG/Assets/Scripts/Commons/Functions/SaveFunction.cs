using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveFunction
{
    public static void saveData()
    {
        List<MonsterSaveStat> temp = new List<MonsterSaveStat>();
        List<ItemSaveStat> tempItem = new List<ItemSaveStat>();
        foreach (MonsterLiveStat monsterLiveStat in LocalStorage.Live.monsters.Values)
        {
            temp.Add(monsterLiveStat.saveStat);
            foreach (ItemLiveStat itemLiveStat in monsterLiveStat.itemStatList.Values)
            {
                tempItem.Add(itemLiveStat.saveStat);
            }
        }
        foreach (ItemLiveStat itemLiveStat in LocalStorage.Live.items.Values)
        {
            tempItem.Add(itemLiveStat.saveStat);
        }
        CommonFunctions.saveObjectToJson<List<MonsterSaveStat>>($"Assets/Resources/{PathInfo.Json.Save.monster}", temp);
        CommonFunctions.saveObjectToJson<List<ItemSaveStat>>($"Assets/Resources/{PathInfo.Json.Save.item}", tempItem);
        CommonFunctions.saveObjectToJson<int>($"Assets/Resources/{PathInfo.Json.Save.currency}", LocalStorage.Live.currency);
        CommonFunctions.saveObjectToJson<List<int>>($"Assets/Resources/{PathInfo.Json.Save.journey}", LocalStorage.Live.journeyInfo);
    }

    public static void saveDungeon(string fileName, DungeonStat dungeonStat)
    {
        CommonFunctions.saveObjectToJson<DungeonStat>($"Assets/Resources/{PathInfo.Json.Dictionary.Dungeon}{fileName}", dungeonStat);
    }
    public static void saveField(string fileName, FieldSaveStat fieldStat)
    {
        CommonFunctions.saveObjectToJson<FieldSaveStat>($"Assets/Resources/{PathInfo.Json.Dictionary.Field}{fileName}", fieldStat);
    }
    public static void saveMonsters(string fileName, List<MonsterSaveStat> monsterSaveStats)
    {
        CommonFunctions.saveObjectToJson<List<MonsterSaveStat>>($"Assets/Resources/{PathInfo.Json.Dictionary.Entry}{fileName}", monsterSaveStats);
    }
}
