using System.Collections.Generic;

[System.Serializable]
public class SkillStat
{
    public float range { get; set; }
    public float coolTime { get; set; }
    public float castingTime { get; set; }
    public TargetTypeEnum targetType { get; set; }
    public List<EffectStat> toCaster { get; set; }
    public float delayProjectile { get; set; }
    public List<ProjectileStat> projectiles { get; set; }

    public float timeCharging { get; set; }
}