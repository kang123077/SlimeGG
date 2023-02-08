
[System.Serializable]
public class EffectAreaStat
{
    public float duration { get; set; }
    public float range { get; set; }
    public bool isTarget { get; set; }
    public float spd { get; set; }
    public string src { get; set; }
    public string rgb { get; set; }
    public EffectBundleStat effects { get; set; }

    public EffectAreaStat()
    {

    }
    public EffectAreaStat(EffectAreaStat effectAreaStat)
    {
        if (effectAreaStat != null)
        {
            duration = effectAreaStat.duration;
            range = effectAreaStat.range;
            isTarget = effectAreaStat.isTarget;
            spd = effectAreaStat.spd;
            src = effectAreaStat.src;
            rgb = effectAreaStat.rgb;
            effects = new EffectBundleStat(effectAreaStat.effects);
        }
    }
}