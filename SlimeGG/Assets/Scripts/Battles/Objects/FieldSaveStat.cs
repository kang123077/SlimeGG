using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class FieldSaveStat
{
    public string name, displayName;
    public List<EntrySlotSaveStat> entries;

    public FieldSaveStat()
    {
        name = string.Empty;
        displayName = string.Empty;
        entries = new List<EntrySlotSaveStat>();
    }
}