using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntrySlotController : MonoBehaviour
{
    public int x;
    public int y;

    public bool isPosessed = false;

    public void installMonster(ContentController originMonster)
    {
        isPosessed = true;
        originMonster.transform.SetParent(transform);
        originMonster.transform.localScale = Vector3.one;
        originMonster.transform.localPosition = Vector3.zero;
        originMonster.transform.GetChild(0).GetComponent<BoxCollider>().size = Vector3.one;
    }

    public void truncateMonster()
    {
        isPosessed = false;
    }
}
