using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotController : MonoBehaviour
{
    [SerializeField]
    InventoryType type = InventoryType.Item;
    private bool isMouseIn = false;
    private Transform bg { get; set; }
    private InventoryStat stat;
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

    public void initSlot(MonsterVO monsterVO)
    {
        this.type = InventoryType.Monster;
        stat = MonsterCommonFunction.generateMonsterInventoryStat(monsterVO);
        bg = transform.Find("Image");
        bg.GetComponent<Image>().sprite = Resources.Load<Sprite>(
            PathInfo.SPRITE + stat.src
            );
    }

    public void initSlot(Object itemVO)
    {
        this.type = InventoryType.Item;
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

    private void OnMouseDown()
    {
        Debug.Log("Down!");
    }

    private void OnMouseUp()
    {
        Debug.Log("UP!");
    }
}
