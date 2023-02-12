using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        updateGridCellSize();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void updateGridCellSize()
    {
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
            for (int i = 0; i < x * y; i++)
            {
                Transform newCell = Instantiate(cellTf);
                if (newCell.GetComponent<EntrySlotController>())
                {
                    newCell.GetComponent<EntrySlotController>().setCoordinate(x, y);
                }
                newCell.SetParent(transform);
                newCell.localPosition = Vector3.zero;
                cells.Add(newCell);
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
