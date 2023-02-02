using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemCommonFunction
{
    public static ItemStat generateItemInventoryStat(ItemVO itemVO)
    {
        ItemStat res = LocalDictionary.items[itemVO.name];
        return res;
    }
}
