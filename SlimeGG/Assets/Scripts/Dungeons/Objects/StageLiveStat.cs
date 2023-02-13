using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageLiveStat
{
    public List<float> locationPos = new List<float>();
    public StageType type { get; set; }
    List<StageLiveStat> nextStages = new List<StageLiveStat>();
}
