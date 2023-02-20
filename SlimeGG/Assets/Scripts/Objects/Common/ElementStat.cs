[System.Serializable]
public class ElementStat
{
    public ElementEnum name;
    public float amount;
    public bool shouldOver;

    public ElementStat()
    {
        amount = 0f;
        shouldOver = true;
    }

    public ElementStat(float amount)
    {
        this.amount = amount;
        shouldOver = true;
    }

    public ElementStat(ElementStat stat)
    {
        amount = stat.amount;
        name = stat.name;
        shouldOver = stat.shouldOver;
    }
}
