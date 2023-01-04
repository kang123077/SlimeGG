using System.Collections.Generic;

[System.Serializable]
public class EffectBundleStat
{
    public List<EffectStat> instant { get; set; }
    public List<EffectStat> sustain { get; set; }
}