using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileSetInventoryController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isMouseIn = false;
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
}
