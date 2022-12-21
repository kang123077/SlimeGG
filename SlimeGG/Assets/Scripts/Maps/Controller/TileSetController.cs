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
    private List<Transform> monsters = new List<Transform>();
    private float zCoor = 19f;
    private Vector3 size;

    private Vector2 correctionCoor;

    public void initTileSet()
    {
        //bgSprite = transform.Find("bg").GetComponent<SpriteRenderer>();
        //bgSprite.sprite = Resources.Load<Sprite>(sprite_path + tileSetInfo.tileShape.ToString());
        //bgSprite.transform.position = new Vector3(tileSetInfo.adjCoor.x, tileSetInfo.adjCoor.y, zCoor);
        //tiles = new GameObject[tileSetInfo.tileInfos.Length];
        //size.z = tileSetInfo.tileInfos.Length;
        //for (int i = 0; i < tileSetInfo.tileInfos.Length; i++)
        //{
        //    int x = tileSetInfo.tileInfos[i].x;
        //    size.x = Mathf.Max(size.x, x);
        //    int y = tileSetInfo.tileInfos[i].y;
        //    size.y = Mathf.Max(size.y, y);
        //    GameObject newTile = Instantiate(tileBase);
        //    newTile.transform.position = new Vector3(y % 2 == 0 ? x * 2 : ((x * 2) + 1), -y * 2, zCoor);
        //    (tiles[i] = newTile).transform.SetParent(transform);
        //}
        //size.x += 1;
        //size.y += 1;
    }

    public void setTileSetInfo(TileSetInfo tileSetInfo)
    {
        //this.tileSetInfo = tileSetInfo;
        //initTileSet();
        //if (tileSetInfo.isFixed)
        //{
        //    transform.GetComponent<CompositeCollider2D>().geometryType = CompositeCollider2D.GeometryType.Outlines;
        //    tryAttachTileSet();
        //}
    }

    void OnMouseDrag()
    {
        //if (tileSetInfo.isFixed || monsters.Count > 0) return;
        //Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zCoor);
        //Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePos);
        //transform.position = objPosition - new Vector3(correctionCoor.x, correctionCoor.y, 0f);
    }

    private void OnMouseDown()
    {
       // if (tileSetInfo.isFixed || monsters.Count > 0) return;
       // Vector3 mousePos = new Vector3(
       //Input.mousePosition.x - correctionCoor.x,
       //Input.mousePosition.y - correctionCoor.y,
       //10f
       //);
       // Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePos);
       // correctionCoor = objPosition - transform.position;

       // GameObject.Find("UI").GetComponent<UIController>().UIOnChecker();
       // GameObject.Find("Popup UI").GetComponent<PopupUIController>().generateUI(tileSetInfo);

       // tileSetInventory.GetComponent<TileSetInventoryController>().removeTileSet(transform);
       // for (int i = 0; i < tiles.Length; i++)
       // {
       //     tiles[i].GetComponent<TileBaseController>().detach();
       // }
       // transform.gameObject.layer = 7;
    }

    private void OnMouseUp()
    {
        //if (tileSetInfo.isFixed || monsters.Count > 0) return;
        //transform.gameObject.layer = 3;
        //if (tileSetInventory.GetComponent<TileSetInventoryController>().getIsMouseIn())
        //{
        //    sendTileSetToInventory();
        //}
        //else
        //{
        //    tileSetInventory.GetComponent<TileSetInventoryController>().removeTileSet(transform);
        //}
        //if (!isInInventory)
        //{
        //    if (!tryAttachTileSet())
        //    {
        //        if (installedCoor.x != -1f)
        //        {
        //            sendTileSetToPrevPosition();
        //        }
        //        else
        //        {
        //            sendTileSetToInventory();
        //        }
        //    }
        //}
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
                    installedCoor = tiles[i].GetComponent<TileBaseController>().attach(transform);
                }
                else
                {
                    tiles[i].GetComponent<TileBaseController>().attach(transform);
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
        targetMonster.localPosition = new Vector3(targetMonster.localPosition.x, targetMonster.localPosition.y, 0f);
        monsters.Add(targetMonster);
    }

    public void removeMonster(Transform targetMonster)
    {
        monsters.Remove(targetMonster);
        if (monsters.Count == 0)
        {
            transform.GetComponent<CompositeCollider2D>().geometryType = CompositeCollider2D.GeometryType.Polygons;
        }
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
