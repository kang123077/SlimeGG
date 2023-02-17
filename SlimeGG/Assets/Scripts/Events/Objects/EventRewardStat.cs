using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EventRewardStat
{
    public bool isRandom { get; set; }
    // 1: 몬스터, 2: 아이템, 3: 재화
    public int type { get; set; }
    // 재화 <- null, 몬스터 <- 종족명, 아이템 <- 아이템명
    public string name { get; set; }
    public bool isGive { get; set; }
}