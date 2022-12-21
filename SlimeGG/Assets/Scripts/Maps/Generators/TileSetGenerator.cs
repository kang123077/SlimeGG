using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSetGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject tileSetInventoy;
    [SerializeField]
    private GameObject tileSet;
    // Start is called before the first frame update
    void Start()
    {
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
}
