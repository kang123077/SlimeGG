using UnityEngine;
using UnityEngine.EventSystems;

public class PopupUIHeader : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    private RectTransform _parentRect;
    private Vector2 correctionCoor;

    private void Awake()
    {
        _parentRect = transform.parent.GetComponent<RectTransform>();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        Vector3 mousePos = new Vector3(
       Input.mousePosition.x - correctionCoor.x,
       Input.mousePosition.y - correctionCoor.y,
       10f
       );
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePos);
        correctionCoor = objPosition - _parentRect.position;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector3 mousePos = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            10f
            );
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePos);
        _parentRect.position = objPosition - new Vector3(correctionCoor.x, correctionCoor.y, 0f);
    }
}
