using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotController : MonoBehaviour
{
    [SerializeField]
    public InventoryType type = InventoryType.None;
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
        Vector2 temp = GetComponent<RectTransform>().sizeDelta;
        GetComponent<BoxCollider>().size = new Vector3(temp.x, temp.y, 0.2f);
    }

    public void installContent(Transform newContent)
    {
        newContent.SetParent(transform);
        newContent.localPosition = new Vector3(0f, 0f, -2f);
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
    }

    public void removeContent()
    {
        contentController = null;
    }
}
