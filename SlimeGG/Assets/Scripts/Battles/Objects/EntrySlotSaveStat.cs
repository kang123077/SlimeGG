using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntrySlotSaveStat
{
    public int x, y;
    public List<BasicStat> stats;

    public EntrySlotSaveStat()
    {
        stats = new List<BasicStat>();
    }
}
