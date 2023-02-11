using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommerceFunction
{
    public static void sellContent(ContentController contentController)
    {
        LocalStorage.currency += weighPriceOfContent(contentController);
    }
    public static int weighPriceOfContent(ContentController contentController)
    {
        int res = 0;
        if (contentController.type == InventoryType.Monster)
        {
            res = weighPriceOfMonster(contentController.monsterLiveStat);
        }
        else
        {
            res = weighPriceOfItem(contentController.itemLiveStat);
        }
        return res;
    }
    private static int weighPriceOfMonster(MonsterLiveStat monsterLiveStat)
    {
        int price = 0;
        switch (monsterLiveStat.dictionaryStat.tier)
        {
            case 1:
                price = 50;
                break;
            case 2:
                price = 120;
                break;
            case 3:
                price = 290;
                break;
            case 4:
                price = 690;
                break;
            case 5:
                price = 1650;
                break;
        }
        return price;
    }

    private static int weighPriceOfItem(ItemLiveStat itemLiveStat)
    {
        int price = 20;
        return price;
    }
}