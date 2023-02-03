using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotController : MonoBehaviour
{
    [SerializeField]
    InventoryType type = InventoryType.None;
    Transform content;
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
        }
        return true;
    }

    public void initSlot(InventoryType type)
    {
        this.type = type;
    }

    private void adjustSize()
    {
        GetComponent<BoxCollider2D>().size = GetComponent<RectTransform>().sizeDelta;
    }
}
