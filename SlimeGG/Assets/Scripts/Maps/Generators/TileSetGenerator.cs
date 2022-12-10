using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSetGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject tileSetInventoy;
    [SerializeField]
    private GameObject tileSet;
    private List<TileInfo[]> tileSetInfos;
    // Start is called before the first frame update
    void Start()
    {
        TileInfo[] newTileSetInfo = new TileInfo[4];
        newTileSetInfo[0] = new TileInfo(0, 0);
        newTileSetInfo[1] = new TileInfo(1, 0);
        newTileSetInfo[2] = new TileInfo(1, 1);
        newTileSetInfo[3] = new TileInfo(0, 2);
        addTileSetToInventory(newTileSetInfo);
        newTileSetInfo = new TileInfo[3];
        newTileSetInfo[0] = new TileInfo(0, 0);
        newTileSetInfo[1] = new TileInfo(1, 0);
        newTileSetInfo[2] = new TileInfo(0, 1);
        addTileSetToInventory(newTileSetInfo);
        newTileSetInfo = new TileInfo[3];
        newTileSetInfo[0] = new TileInfo(0, 0);
        newTileSetInfo[1] = new TileInfo(1, 0);
        newTileSetInfo[2] = new TileInfo(2, 0);
        addTileSetToInventory(newTileSetInfo);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void addTileSetToInventory(TileInfo[] newTileSetInfo)
    {
        GameObject newTileSet = Instantiate(tileSet);
        newTileSet.GetComponent<TileSetController>().setTileInfos(newTileSetInfo);
        newTileSet.GetComponent<TileSetController>().setTileSetInventory(tileSetInventoy);
        tileSetInventoy.GetComponent<TileSetInventoryController>().addTileSet(newTileSet.transform);
    }
}
