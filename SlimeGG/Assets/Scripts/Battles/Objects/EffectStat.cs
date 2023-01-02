
[System.Serializable]
public class EffectStat : BasicStatVO
{
    public float duration { get; set; }
    public float tickTime { get; set; }

    // 틱 타임 계산용
    public float tickTimer = -1f;
    public EffectStat(float amount) : base(amount)
    {
    }
}