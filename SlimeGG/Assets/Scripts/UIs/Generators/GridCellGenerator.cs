using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GridCellGenerator : MonoBehaviour
{
    [SerializeField]
    int x = 0, y = 0;
    [SerializeField]
    private Transform cellTf;

    private List<Transform> cells = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void generateTiles(string fieldName = null)
    {
        FieldSaveStat temp;
        if (fieldName != null)
        {
            // 특정
            temp = LocalDictionary.fields[fieldName];
        }
        else
        {
            // 랜덤
            temp = LocalDictionary.fields[LocalDictionary.fields.Keys.ToList()[UnityEngine.Random.Range(0, LocalDictionary.fields.Keys.Count)]];
        }
        if (x != 0)
        {
            GetComponent<GridLayoutGroup>().constraintCount = x;
            for (int i = 0; i < x * y; i++)
            {
                Transform newCell = Instantiate(cellTf);
                newCell.SetParent(transform);
                newCell.localPosition = Vector3.zero;
            }
        }
        else
        {
            x = SettingVariables.Battle.entrySizeMax[0];
            y = SettingVariables.Battle.entrySizeMax[1];
            GetComponent<GridLayoutGroup>().constraintCount = x;
            for (int j = 0; j < y; j++)
            {
                for (int i = 0; i < x; i++)
                {
                    Transform newCell = Instantiate(cellTf);
                    if (newCell.GetComponent<EntrySlotController>())
                    {
                        newCell.GetComponent<EntrySlotController>().setCoordinate(i, j);
                        newCell.GetComponent<EntrySlotController>().initTileInfo(temp.entries.Find((stat) => { return stat.x == i && stat.y == j; }));
                    }
                    else if (newCell.GetComponent<PlaceableSlotController>())
                    {
                        newCell.GetComponent<PlaceableSlotController>().setCoordinate(i, j);
                    }
                    newCell.SetParent(transform);
                    newCell.localPosition = Vector3.zero;
                    cells.Add(newCell);
                }
            }
        }
    }

    internal void sendCellsFunction(Func<Transform, object> functionToSend)
    {
        foreach (Transform cell in cells)
        {
            functionToSend(cell);
        }
    }
}
