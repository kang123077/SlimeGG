using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotController : MonoBehaviour
{
    [SerializeField]
    InventoryType type = InventoryType.None;
    ContentController contentController;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        adjustSize();
    }

    public bool isOccupied()
    {
        switch (type)
        {
            case InventoryType.None: return false;
            default:
                return contentController != null;
        }
    }

    public void initSlot(InventoryType type)
    {
        this.type = type;
    }

    private void adjustSize()
    {
        GetComponent<BoxCollider2D>().size = GetComponent<RectTransform>().sizeDelta;
    }

    public void installContent(Transform newContent)
    {
        GetComponent<CompositeCollider2D>().geometryType = CompositeCollider2D.GeometryType.Outlines;
        newContent.SetParent(transform);
        newContent.localPosition = Vector3.zero;
        newContent.localScale = Vector3.one;
        newContent.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        contentController = newContent.GetComponent<ContentController>();
    }

    public void truncateContent()
    {
        if (contentController != null)
        {
            contentController.destroySelf();
            contentController = null;
        }
        GetComponent<CompositeCollider2D>().geometryType = CompositeCollider2D.GeometryType.Polygons;
    }
}
