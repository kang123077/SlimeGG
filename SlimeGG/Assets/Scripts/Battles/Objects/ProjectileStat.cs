
[System.Serializable]
public class ProjectileStat
{
    public float spd { get; set; }
    public string src { get; set; }
    public bool isTarget { get; set; }
    public EffectBundleStat effects { get; set; }
    public EffectAreaStat area { get; set; }
}