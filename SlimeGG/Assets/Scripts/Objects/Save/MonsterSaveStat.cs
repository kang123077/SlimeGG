using System.Collections.Generic;


[System.Serializable]
public class MonsterSaveStat
{
    public List<ElementStat> exp = new List<ElementStat>();
    public string id;
    public string speicie;
    public float[] entryPos;
}