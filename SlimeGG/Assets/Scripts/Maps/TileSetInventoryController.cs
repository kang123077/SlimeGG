using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileSetInventoryController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Transform contentParent;
    private bool isMouseIn = false;
    private Vector2 margin = new Vector2(100f, -100f);

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseIn = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseIn = false;
    }

    public bool getIsMouseIn()
    {
        return isMouseIn;
    }

    public void addTileSet(Transform newTileSet)
    {
        newTileSet.GetComponent<TileSetController>().setIsInInventory(true);
        newTileSet.SetParent(transform.Find("Viewport").Find("Content"));
        newTileSet.localScale = new Vector3(40f, 40f, 1f);
        sortInventory();
    }

    public void removeTileSet(Transform targetTileSet)
    {
        targetTileSet.GetComponent<TileSetController>().setIsInInventory(false);
        targetTileSet.SetParent(null);
        targetTileSet.localScale = new Vector3(1f, 1f, 1f);
        sortInventory();
    }

    public void sortInventory()
    {
        if (contentParent == null)
            contentParent = transform.Find("Viewport").Find("Content");
        float accuX = 0f;
        for (int i = 0; i < contentParent.childCount; i++)
        {
            Transform curTileSet = contentParent.GetChild(i);
            Vector3 tileSetSize = curTileSet.GetComponent<TileSetController>().getSize();
            curTileSet.localPosition = new Vector3(accuX + margin.x, margin.y, 0f);
            accuX += 100f + (tileSetSize.x * 100f);
        }
    }
}
