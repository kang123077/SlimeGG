using System.Collections.Generic;

[System.Serializable]
public class SkillStat
{
    public string name { get; set; }
    public string desc { get; set; }
    public float range { get; set; }
    public float coolTime { get; set; }
    public float castingTime { get; set; }
    public string targetType { get; set; }
    public List<EffectStat> toCaster { get; set; }
    public float delayProjectile { get; set; }
    public List<ProjectileStat> projectiles { get; set; }

    public float timeCharging { get; set; }

    public int numTarget = 1;
    public SkillStat()
    {
    }
    public SkillStat(SkillStat skillStat)
    {
        this.range = skillStat.range;
        coolTime = skillStat.coolTime;
        castingTime = skillStat.castingTime;
        targetType = skillStat.targetType;
        delayProjectile = skillStat.delayProjectile;
        timeCharging = skillStat.timeCharging;
        numTarget = skillStat.numTarget;
        toCaster = new List<EffectStat>();
        projectiles = new List<ProjectileStat>();
        if (skillStat.toCaster != null)
        {
            foreach (EffectStat effectStat in skillStat.toCaster)
            {
                toCaster.Add(new EffectStat(effectStat));
            }
        }
        if (skillStat.projectiles != null)
        {
            foreach (ProjectileStat projectileStat in skillStat.projectiles)
            {
                projectiles.Add(new ProjectileStat(projectileStat));
            }
        }
    }
}