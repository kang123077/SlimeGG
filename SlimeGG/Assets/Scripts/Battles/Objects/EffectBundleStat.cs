using System.Collections.Generic;

[System.Serializable]
public class EffectBundleStat
{
    List<EffectStat> instant { get; set; }
    List<EffectStat> sustain { get; set; }
}