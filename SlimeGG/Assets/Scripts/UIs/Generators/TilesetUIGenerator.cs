using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSetUIGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject tileSetUI;

    public void generateTileSetUI(TileSetInfo tileSetInfo)
    {
        TileSetUIInfo newTileSetUIInfo = new TileSetUIInfo(tileSetInfo.tileType, tileSetInfo.tileShape);
        GameObject newTileSetUI = Instantiate(tileSetUI);
        newTileSetUI.GetComponent<TileSetUIController>().setTileSetUIInfo(newTileSetUIInfo);
        newTileSetUI.transform.SetParent(transform.Find("Viewport").Find("Content"));
    }
}
