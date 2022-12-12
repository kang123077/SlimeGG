using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSetController : MonoBehaviour
{
    [SerializeField]
    private GameObject tileBase;
    private TileSetInfo tileSetInfo;
    private GameObject tileSetInventory;
    private Transform tileSetInstalledStore;
    private bool isInInventory = false;

    private GameObject[] tiles;
    private Transform[] monsters;
    private bool isOnMonster = false;
    private float zCoor = 19f;
    private Vector3 size;

    public void initTileSet()
    {
        tiles = new GameObject[tileSetInfo.tileInfos.Length];
        size.z = tileSetInfo.tileInfos.Length;
        for (int i = 0; i < tileSetInfo.tileInfos.Length; i++)
        {
            int x = tileSetInfo.tileInfos[i].x;
            size.x = Mathf.Max(size.x, x);
            int y = tileSetInfo.tileInfos[i].y;
            size.y = Mathf.Max(size.y, y);
            GameObject newTile = Instantiate(tileBase);
            newTile.transform.position = new Vector3(y % 2 == 0 ? x * 2 : ((x * 2) + 1), -y * 2, zCoor);
            newTile.GetComponent<TileBaseController>().setTileType(tileSetInfo.tileType);
            (tiles[i] = newTile).transform.SetParent(transform);
        }
        size.x += 1;
        size.y += 1;
    }

    public void setTileSetInfo(TileSetInfo tileSetInfo)
    {
        this.tileSetInfo = tileSetInfo;
        initTileSet();
        if (tileSetInfo.isFixed)
        {
            transform.GetComponent<CompositeCollider2D>().geometryType = CompositeCollider2D.GeometryType.Outlines;
            tryAttachTileSet();
        }
    }

    void OnMouseDrag()
    {
        if (tileSetInfo.isFixed || isOnMonster) return;
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zCoor);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePos);
        objPosition.z = zCoor;
        transform.position = objPosition;
    }

    private void OnMouseDown()
    {
        if (tileSetInfo.isFixed || isOnMonster) return;
        tileSetInventory.GetComponent<TileSetInventoryController>().removeTileSet(transform);
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].GetComponent<TileBaseController>().detach();
        }
        transform.gameObject.layer = 7;
    }

    private void OnMouseUp()
    {
        if (tileSetInfo.isFixed || isOnMonster) return;
        transform.gameObject.layer = 3;
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
            tryAttachTileSet();
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

    public void setTileSetInstalledStore(Transform tileSetInstalledStore)
    {
        this.tileSetInstalledStore = tileSetInstalledStore;
    }

    public bool tryAttachTileSet()
    {
        bool isAllAttachable = true;
        for (int i = 0; i < tiles.Length; i++)
        {
            isAllAttachable = isAllAttachable && (tiles[i].GetComponent<TileBaseController>().returnSocketMountable() != null);
        }
        if (isAllAttachable)
        {
            transform.SetParent(tileSetInstalledStore);
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i].GetComponent<TileBaseController>().attach();
            }
        }
        return isAllAttachable;
    }

    public void addMonster(Transform targetMonster)
    {
        transform.GetComponent<CompositeCollider2D>().geometryType = CompositeCollider2D.GeometryType.Outlines;
        targetMonster.SetParent(transform.Find("Monster Container"));
        isOnMonster = true;
    }

    public void removeMonster()
    {
        isOnMonster = transform.Find("Monster Container").childCount > 0;
        transform.GetComponent<CompositeCollider2D>().geometryType = CompositeCollider2D.GeometryType.Polygons;
    }
}
