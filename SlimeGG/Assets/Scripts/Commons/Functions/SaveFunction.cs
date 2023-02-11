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
        CommonFunctions.saveObjectToJson<int>($"Assets/Resources/{PathInfo.Json.Save.currency}", LocalStorage.currency);
    }
}
