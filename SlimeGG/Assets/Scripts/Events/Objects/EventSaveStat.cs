using System.Collections;
using System.Collections.Generic;

#nullable enable
[System.Serializable]
public class EventSaveStat
{
    public int id { get; set; }
    public string? background { get; set; }
    public string? characterLeft { get; set; }
    public string? characterRight { get; set; }
    public string desc { get; set; }
    public int? nextId { get; set; }
    public List<EventChoiceStat?>? choices { get; set; }
    public List<EventRewardStat?>? rewards { get; set; }
}