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
        if (!LocalStorage.TILESET_LOADING_DONE && LocalStorage.TILESET_DATACALL_DONE && LocalStorage.SOCKET_LOADING_DONE)
        {
            LocalStorage.tileSets.ForEach((tileSetBriefInfo) =>
            {
                generateTileset(tileSetBriefInfo);
            });
            LocalStorage.TILESET_LOADING_DONE = true;
        }
    }

    private void generateTileset(TileSetBriefInfo tileSetBriefInfo)
    {
        GameObject newTileSet = Instantiate(tileSet);
        LocalStorage.tileSetTransforms.Add(newTileSet.transform);
        newTileSet.GetComponent<TileSetController>().setTileSetInstalledStore(transform);
        newTileSet.GetComponent<TileSetController>().setTileSetInventory(tileSetInventoy);
        newTileSet.GetComponent<TileSetController>().setTileSetBriefInfo(tileSetBriefInfo);
    }
}
