using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileSetController : MonoBehaviour
{
    private static string sprite_path = "Sprites/Tiles/";
    [SerializeField]
    private GameObject tileBase;
    private TileSetInfo tileSetInfo;
    private GameObject tileSetInventory;
    private Transform tileSetInstalledStore;
    private bool isInInventory = false;
    private Vector2 installedCoor = new Vector2(-1f, -1f);
    private SpriteRenderer bgSprite;

    private GameObject[] tiles;
    private Transform[] monsters;
    private bool isOnMonster = false;
    private float zCoor = 19f;
    private Vector3 size;

    public void initTileSet()
    {
        string spriteName = "";
        switch (tileSetInfo.tileShape)
        {
            case TileShape.Single:
                spriteName = "Single";
                break;
            case TileShape.StraightVertical2:
                break;
            case TileShape.StraightHorizontal2:
                break;
            case TileShape.StraightVertical3:
                break;
            case TileShape.StraightHorizontal3:
                spriteName = "StraightHorizontal3";
                break;
            case TileShape.CurvedClock3:
                spriteName = "CurvedClock3";
                break;
            case TileShape.CurvedClock4:
                spriteName = "CurvedClock4";
                break;
            case TileShape.CurvedCounterClock3:
                break;
            case TileShape.CurvedCounterClock4:
                break;
            case TileShape.ZigZagR3:
                break;
            case TileShape.ZigZagL3:
                break;
            case TileShape.Triangle3:
                break;
            case TileShape.TriangleTailR4:
                break;
            case TileShape.TriangleTailL4:
                break;
            case TileShape.TriangleReverse3:
                spriteName = "TriangleReverse3";
                break;
            case TileShape.TriangleReverseTailR4:
                break;
            case TileShape.TriangleReverseTailL4:
                break;
            case TileShape.Diamond:
                break;
            case TileShape.ParallR4:
                break;
            case TileShape.ParallL4:
                break;
        }
        bgSprite = transform.Find("bg").GetComponent<SpriteRenderer>();
        bgSprite.sprite = Resources.Load<Sprite>(sprite_path + spriteName);
        bgSprite.transform.position = new Vector3(tileSetInfo.adjCoor.x, tileSetInfo.adjCoor.y, zCoor);
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
        objPosition.x -= size.x / 2f + (size.y % 2 == 0 ? 0 : 0.5f);
        objPosition.y += size.y / 2f;
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
            sendTileSetToInventory();
        }
        else
        {
            tileSetInventory.GetComponent<TileSetInventoryController>().removeTileSet(transform);
        }
        if (!isInInventory)
        {
            if (!tryAttachTileSet())
            {
                if (installedCoor.x != -1f)
                {
                    sendTileSetToPrevPosition();
                }
                else
                {
                    sendTileSetToInventory();
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

    public void setTileSetInstalledStore(Transform tileSetInstalledStore)
    {
        this.tileSetInstalledStore = tileSetInstalledStore;
    }


    /** Try to install TileSet into Socket
     *  True if installation successed
     *  False if installation failed
     */
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
                if (i == 0)
                {
                    installedCoor = tiles[i].GetComponent<TileBaseController>().attach();
                }
                else
                {
                    tiles[i].GetComponent<TileBaseController>().attach();
                }
            }
            transform.position = new Vector3(
                installedCoor.x * 2 + (installedCoor.y % 2 == 0 ? 0 : 1),
                -installedCoor.y * 2,
                zCoor);
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

    /**
     * Send TileSet to Inventory
     */
    private void sendTileSetToInventory()
    {
        installedCoor = new Vector2(-1f, -1f);
        tileSetInventory.GetComponent<TileSetInventoryController>().addTileSet(transform);
    }

    /**
     * Send TileSet to previous installed position(Socket)
     */
    private void sendTileSetToPrevPosition()
    {
        transform.position = new Vector3(
            installedCoor.x * 2 + (installedCoor.y % 2 == 0 ? 0 : 1),
            -installedCoor.y * 2,
            zCoor);
        tryAttachTileSet();
    }
}
