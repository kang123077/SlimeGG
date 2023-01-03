
[System.Serializable]
public class EffectStat : BasicStatVO
{
    public float duration { get; set; }
    public float tickTime { get; set; }

    // 틱 타임 계산용
    public float tickTimer = -1f;

    public EffectStat() { }
    public EffectStat(float amount) : base(amount)
    {
    }

    public EffectStat(EffectStat origin) : base()
    {
        this.duration = origin.duration;
        this.tickTime = origin.tickTime;
        this.tickTimer = origin.tickTimer;
        this.amount = origin.amount;
        this.isMultiple = origin.isMultiple;
        this.name = origin.name;
        this.directionWithPower = origin.directionWithPower;
    }
}