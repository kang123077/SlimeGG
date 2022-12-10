using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileSetController : MonoBehaviour
{
    [SerializeField]
    private GameObject tileBase;
    private TileInfo[] tileInfos;
    private GameObject tileSetInventory;
    private bool isInInventory = false;

    private GameObject[] tiles;
    private float zCoor = 19f;
    private Vector3 size;

    //void Start()
    //{
    //    initTileSet();
    //}

    public void initTileSet()
    {
        tiles = new GameObject[tileInfos.Length];
        size.z = tileInfos.Length;
        for (int i = 0; i < tileInfos.Length; i++)
        {
            int x = tileInfos[i].x;
            size.x = Mathf.Max(size.x, x);
            int y = tileInfos[i].y;
            size.y = Mathf.Max(size.y, y);
            GameObject newTile = Instantiate(tileBase);
            newTile.transform.position = new Vector3(y % 2 == 0 ? x * 2 : ((x * 2) + 1), -y * 2, zCoor);
            (tiles[i] = newTile).transform.transform.SetParent(transform);
        }
        size.x += 1;
        size.y += 1;
    }

    public void setTileInfos(TileInfo[] tileInfos)
    {
        this.tileInfos = tileInfos;
        initTileSet();
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
        tileSetInventory.GetComponent<TileSetInventoryController>().removeTileSet(transform);
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].GetComponent<TileBaseController>().detach();
        }
    }

    private void OnMouseUp()
    {
        if (tileSetInventory.GetComponent<TileSetInventoryController>().getIsMouseIn())
        {
            tileSetInventory.GetComponent<TileSetInventoryController>().addTileSet(transform);
        }
        else
        {
            tileSetInventory.GetComponent<TileSetInventoryController>().removeTileSet(transform);
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

    public void setIsInInventory(bool isInInventory)
    {
        this.isInInventory = isInInventory;
    }

    public bool getIsInInvetory()
    {
        return isInInventory;
    }

    public Vector2 getSize()
    {
        return size;
    }

    public void setTileSetInventory(GameObject tileSetInventory)
    {
        this.tileSetInventory = tileSetInventory;
    }
}
