using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryController : MonoBehaviour
{
    private List<GameObject> objectList = new List<GameObject>();

    public void addEntry(GameObject newMonster)
    {
        newMonster.transform.SetParent(transform);
        newMonster.transform.Find("Image").GetComponent<SpriteRenderer>().sortingLayerName = "GameObject Above UI";
        objectList.Add(newMonster);
        alignEntries();
    }

    public void removeEntry(GameObject newMonster)
    {
        newMonster.transform.Find("Image").GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        newMonster.transform.SetParent(null);
    }

    private void alignEntries()
    {
        for (int i = 0; i < objectList.Count; i++)
        {
            objectList[i].transform.localPosition = new Vector3(i * Screen.width / 24, (i % 2 == 0 ? 1 : -1) * Screen.width / 36, 0f);
        }
    }

    public void changeOnMoveStatusFromInven(GameObject newMonster)
    {
        newMonster.transform.Find("Image").GetComponent<SpriteRenderer>().sortingLayerName = "GameObject Above UI";
        newMonster.transform.SetParent(transform);
        newMonster.transform.localScale = new Vector3(56.5f, 56.5f, 1f);
        objectList.Remove(newMonster);
        alignEntries();
    }
}
