using System.Collections.Generic;

[System.Serializable]
public class EffectBundleStat
{
    public List<EffectStat> instant { get; set; }
    public List<EffectStat> sustain { get; set; }

    public EffectBundleStat()
    {

    }
    public EffectBundleStat(EffectBundleStat effectBundleStat)
    {
        this.instant = new List<EffectStat>();
        this.sustain = new List<EffectStat>();
        if (effectBundleStat.instant != null)
        {
            foreach (EffectStat effectStat in effectBundleStat.instant)
            {
                this.instant.Add(new EffectStat(effectStat));
            }
        }
        if (effectBundleStat.sustain != null)
        {
            foreach (EffectStat effectStat in effectBundleStat.sustain)
            {
                this.sustain.Add(new EffectStat(effectStat));
            }
        }
    }
}