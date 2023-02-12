using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntrySlotController : MonoBehaviour
{
    public bool isAlly =false;
    public int x;
    public int y;

    public bool isPosessed = false;

    public void installMonster(ContentController originMonster, bool isEnemy = true)
    {
        if (!isEnemy)
        {
            BattleManager.allyEntry.Add(originMonster);
        }
        originMonster.monsterLiveStat.saveStat.entryPos = new float[] { x, y };
        isPosessed = true;
        originMonster.transform.SetParent(transform);
        originMonster.transform.localScale = Vector3.one;
        originMonster.transform.localPosition = Vector3.zero;
        originMonster.transform.GetChild(0).GetComponent<BoxCollider>().size = Vector3.one;
    }

    public void truncateMonster(ContentController originMonster, bool isEnemy = true)
    {
        if (!isEnemy)
        {
            BattleManager.allyEntry.Remove(originMonster);
        }
        originMonster.monsterLiveStat.saveStat.entryPos = new float[] { };
        isPosessed = false;
    }

    public void setCoordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
