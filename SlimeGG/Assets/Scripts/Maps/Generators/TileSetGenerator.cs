using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSetGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject tileSetInventoy;
    [SerializeField]
    private GameObject tileSet;
    [SerializeField]
    private GameObject monsterGenerator;
    private List<TileInfo[]> tileSetInfos;
    // Start is called before the first frame update
    void Start()
    {
        TileSetInfo newTileSetInfo = new TileSetInfo(TileType.Normal, true, TileShape.Single);
        installBaseTileSet(newTileSetInfo);
        newTileSetInfo = new TileSetInfo(TileShape.CurvedClockArc4);
        addTileSetToInventory(newTileSetInfo);
        newTileSetInfo = new TileSetInfo(TileType.Ocean, TileShape.CurvedCounterClockArc4);
        addTileSetToInventory(newTileSetInfo);
        newTileSetInfo = new TileSetInfo(TileType.Volcano, TileShape.Seven4);
        addTileSetToInventory(newTileSetInfo);
        newTileSetInfo = new TileSetInfo(TileType.Volcano, TileShape.SevenReverse4);
        addTileSetToInventory(newTileSetInfo);
        newTileSetInfo = new TileSetInfo(TileType.Volcano, TileShape.L4);
        addTileSetToInventory(newTileSetInfo);
        newTileSetInfo = new TileSetInfo(TileType.Volcano, TileShape.LReverse4);
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

    private void installBaseTileSet(TileSetInfo newTileSetInfo)
    {
        GameObject newTileSet = Instantiate(tileSet);
        newTileSet.GetComponent<TileSetController>().setTileSetInstalledStore(transform);
        newTileSet.GetComponent<TileSetController>().setTileSetInventory(tileSetInventoy);
        newTileSet.GetComponent<TileSetController>().setTileSetInfo(newTileSetInfo);
        newTileSet.name = "Base TileSet";
        setBaseTileSet(newTileSet);
    }

    private void setBaseTileSet(GameObject baseTileSet)
    {
        monsterGenerator.GetComponent<MonsterGenerator>().baseTileSet = baseTileSet;
        monsterGenerator.GetComponent<MonsterGenerator>().initGeneration();
    }
}
