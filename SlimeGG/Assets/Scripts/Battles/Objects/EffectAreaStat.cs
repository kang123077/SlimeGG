
[System.Serializable]
public class EffectAreaStat
{
    public float duration { get; set; }
    public float range { get; set; }
    public bool isTarget { get; set; }
    public float spd { get; set; }
    public string src { get; set; }
    public EffectBundleStat effects { get; set; }
}