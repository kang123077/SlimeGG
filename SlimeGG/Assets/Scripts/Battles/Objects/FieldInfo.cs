using System.Collections.Generic;

[System.Serializable]
public class FieldInfo
{
    public string name { get; set; }
    public List<int> numberRestrictPerSide { get; set; }
    public List<float> size { get; set; }
}