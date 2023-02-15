using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntrySlotController : MonoBehaviour
{
    public bool isAlly = false;
    private int x;
    private int y;
    private List<BasicStat> basicStats = new List<BasicStat>();
    private ContentController contentController = null;

    public bool isPosessed = false;

    public void installMonster(ContentController originMonster, bool isEnemy = true)
    {
        if (!isEnemy)
        {
            BattleManager.allyEntry.Add(originMonster);
        }
        contentController = originMonster;
        originMonster.isInstalledOnField = true;
        originMonster.monsterLiveStat.saveStat.entryPos = new int[] { x, y };
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
        contentController = null;
        originMonster.isInstalledOnField = false;
        originMonster.monsterLiveStat.saveStat.entryPos = new int[] { };
        isPosessed = false;
    }

    public void setCoordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int[] getCoordinate()
    {
        return new int[] { x, y };
    }

    public void destroySelf()
    {
        transform.parent.GetComponent<PlaceableSlotController>().uninstallEntrySlot();
        Destroy(gameObject);
    }
    public void addBasicStat(BasicStat basicStat)
    {
        basicStats.Add(new BasicStat(basicStat));
    }

    public void removeBasicStat(BasicStat basicStat)
    {
        basicStats.Remove(basicStat);
    }

    public List<BasicStat> getBasicStats()
    {
        return basicStats;
    }

    public void initEffects(List<BasicStat> basicStats)
    {
        this.basicStats = basicStats;
    }

    public ContentController getContentController()
    {
        return contentController;
    }
}
