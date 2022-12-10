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
        TileInfo[] newTileInfos = new TileInfo[1];
        newTileInfos[0] = new TileInfo(0, 0);
        TileSetInfo newTileSetInfo = new TileSetInfo(newTileInfos, TileType.Normal, true);
        installTileSet(newTileSetInfo);
        newTileInfos = new TileInfo[4];
        newTileInfos[0] = new TileInfo(0, 0);
        newTileInfos[1] = new TileInfo(1, 0);
        newTileInfos[2] = new TileInfo(1, 1);
        newTileInfos[3] = new TileInfo(1, 2);
        newTileSetInfo = new TileSetInfo(newTileInfos);
        addTileSetToInventory(newTileSetInfo);
        newTileInfos = new TileInfo[3];
        newTileInfos[0] = new TileInfo(0, 0);
        newTileInfos[1] = new TileInfo(1, 0);
        newTileInfos[2] = new TileInfo(0, 1);
        newTileSetInfo = new TileSetInfo(newTileInfos, TileType.Ocean);
        addTileSetToInventory(newTileSetInfo);
        newTileInfos = new TileInfo[3];
        newTileInfos[0] = new TileInfo(0, 0);
        newTileInfos[1] = new TileInfo(1, 0);
        newTileInfos[2] = new TileInfo(2, 0);
        newTileSetInfo = new TileSetInfo(newTileInfos, TileType.Volcano);
        addTileSetToInventory(newTileSetInfo);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void addTileSetToInventory(TileSetInfo newTileSetInfo)
    {
        GameObject newTileSet = Instantiate(tileSet);
        newTileSet.GetComponent<TileSetController>().setTileSetInstalledStore(transform);
        newTileSet.GetComponent<TileSetController>().setTileSetInventory(tileSetInventoy);
        newTileSet.GetComponent<TileSetController>().setTileSetInfo(newTileSetInfo);
        tileSetInventoy.GetComponent<TileSetInventoryController>().addTileSet(newTileSet.transform);
    }

    private void installTileSet(TileSetInfo newTileSetInfo)
    {
        GameObject newTileSet = Instantiate(tileSet);
        newTileSet.GetComponent<TileSetController>().setTileSetInstalledStore(transform);
        newTileSet.GetComponent<TileSetController>().setTileSetInventory(tileSetInventoy);
        newTileSet.GetComponent<TileSetController>().setTileSetInfo(newTileSetInfo);
    }
}
