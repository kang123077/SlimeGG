using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SocketGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject socketBase;
    [SerializeField]
    private GameObject tileSetBase;
    [SerializeField]
    private GameObject tileSetInventoy;
    [SerializeField]
    private GameObject monsterGenerator;
    [SerializeField]
    private GameObject tileSetStore;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject baseTileSet = Instantiate(tileSetBase);
        //baseTileSet.GetComponent<TileSetController>().setTileSetInstalledStore(tileSetStore.transform);
        //baseTileSet.GetComponent<TileSetController>().setTileSetInventory(tileSetInventoy);
        for (int i = 0; i < 4; i++)
        {
            GameObject newLine = new GameObject();
            newLine.name = "Line " + i;
            Transform newLineTr = newLine.transform;
            newLineTr.position = new Vector3(
                transform.position.x + i % 2 == 0 ? 0 : 1,
                transform.position.y - (i * 2),
                transform.position.z
            );
            newLineTr.SetParent(transform);
            for (int j = 0; j < 6; j++)
            {
                GameObject newSocket = Instantiate(socketBase);
                newSocket.name = "Socket " + j;
                newSocket.tag = "Socket";
                newSocket.GetComponent<SocketController>().coor = new Vector2(j, i);
                if (i == 0 && j == 0)
                {
                    //newSocket.GetComponent<SocketController>().tileSetInstalled = baseTileSet.transform;
                }
                Transform newSocketTr = newSocket.transform;
                newSocketTr.position = newLineTr.position;
                newSocketTr.position = new Vector3(
                    newLineTr.position.x + (j * 2),
                    newLineTr.position.y,
                    newLineTr.position.z
                );
                newSocketTr.SetParent(newLineTr);
            }
        }
        //baseTileSet.GetComponent<TileSetController>().setTileSetInfo(newTileSetInfo);
        //baseTileSet.name = "Base TileSet";
        //baseTileSet.transform.SetParent(tileSetStore.transform);
        //monsterGenerator.GetComponent<MonsterGenerator>().baseTileSet = baseTileSet;
        //monsterGenerator.GetComponent<MonsterGenerator>().initGeneration();

        LocalStorage.SOCKET_LOADING_DONE = true;
    }
}
