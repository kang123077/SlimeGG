public class MonsterInfo
{
    public EggType eggType;
    public InfantType infantType;

    public MonsterInfo()
    {
        eggType = EggType.Normal;
        infantType = InfantType.Beast;
    }

    public MonsterInfo(InfantType infantType)
    {
        this.infantType = infantType;
    }
}
