
[System.Serializable]
public class PlainStatVO
{
    public float amount;
    public bool isMultiple;

    public PlainStatVO()
    {
        this.amount = 0;
        isMultiple = false;
    }

    public PlainStatVO(float amount)
    {
        this.amount = amount;
        isMultiple = false;
    }
}
