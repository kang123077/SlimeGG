using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileSetController : MonoBehaviour
{
    [SerializeField]
    private TileInfo[] tileInfos;
    [SerializeField]
    private GameObject tileBase;
    [SerializeField]
    private GameObject tileSetInventory;
    private bool isInInventory = false;

    private GameObject[] tiles;
    private float zCoor = 19f;

    void Start()
    {
        tiles = new GameObject[tileInfos.Length];
        for (int i = 0; i < tileInfos.Length; i++)
        {
            int x = tileInfos[i].x;
            int y = tileInfos[i].y;
            GameObject newTile = Instantiate(tileBase);
            newTile.transform.position = new Vector3(y % 2 == 0 ? x * 2 : ((x * 2) + 1), -y * 2, zCoor);
            (tiles[i] = newTile).transform.transform.SetParent(transform);
        }
    }


    void OnMouseDrag()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zCoor);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePos);
        objPosition.z = zCoor;
        transform.position = objPosition;
    }

    private void OnMouseDown()
    {
        isInInventory = false;
        transform.SetParent(null);
        transform.localScale = new Vector3(1f, 1f, 1f);
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].GetComponent<TileBaseController>().detach();
        }
    }

    private void OnMouseUp()
    {
        if (tileSetInventory.GetComponent<TileSetInventoryController>().getIsMouseIn())
        {
            print("Go to Inventory");
            isInInventory = true;
            transform.SetParent(tileSetInventory.transform.Find("Viewport").Find("Content"));
            transform.localScale = new Vector3(60f, 60f, 1f);
        }
        else
        {
            isInInventory = false;
            transform.SetParent(null);
        }
        if (!isInInventory)
        {
            bool isAllAttachable = true;
            for (int i = 0; i < tiles.Length; i++)
            {
                isAllAttachable = isAllAttachable && (tiles[i].GetComponent<TileBaseController>().returnSocketMountable() != null);
            }
            if (isAllAttachable)
            {
                for (int i = 0; i < tiles.Length; i++)
                {
                    tiles[i].GetComponent<TileBaseController>().attach();
                }
            }
        }
    }
}
