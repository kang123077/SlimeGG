
[System.Serializable]
public class ProjectileStat
{
    public float spd { get; set; }
    public string src { get; set; }
    public string rgb { get; set; }
    public bool isTarget { get; set; }
    public EffectBundleStat effects { get; set; }
    public EffectAreaStat area { get; set; }

    public ProjectileStat()
    {
    }
    public ProjectileStat(ProjectileStat projectileStat)
    {
        spd = projectileStat.spd;
        src = projectileStat.src;
        rgb = projectileStat.rgb;
        isTarget = projectileStat.isTarget;
        effects = projectileStat.effects != null ? new EffectBundleStat(projectileStat.effects) : null;
        area = projectileStat.area != null ? new EffectAreaStat(projectileStat.area) : null;
    }
}