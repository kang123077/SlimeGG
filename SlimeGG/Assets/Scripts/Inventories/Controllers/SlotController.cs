using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotController : MonoBehaviour
{
    [SerializeField]
    InventoryType type = InventoryType.Item;
    private bool isMouseIn = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMouseIn && Input.GetMouseButton(0) && type == InventoryType.Item)
        {
            Debug.Log("Dragging...");
        }
    }

    public void initSlot(InventoryType type)
    {
        this.type = type;
    }

    private void OnMouseEnter()
    {
        isMouseIn = true;
        Debug.Log("In!");
    }

    private void OnMouseExit()
    {
        isMouseIn = false;
        Debug.Log("Out");
    }
}
