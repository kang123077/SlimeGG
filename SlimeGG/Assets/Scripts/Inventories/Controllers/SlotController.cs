using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotController : MonoBehaviour
{
    [SerializeField]
    InventoryType type = InventoryType.Item;
    private bool isMouseIn = false;
    private Image bg { get; set; }
    private InventoryStat stat = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        adjustSize();
        if (isMouseIn && Input.GetMouseButton(0) && type == InventoryType.Item)
        {
            Debug.Log("Dragging...");
        }
    }

    public bool isOccupied()
    {
        return stat != null;
    }

    public void initStat(MonsterVO monsterVO)
    {
        this.type = InventoryType.Monster;
        stat = MonsterCommonFunction.generateMonsterInventoryStat(monsterVO);
        bg = transform.Find("Image").GetComponent<Image>();
        bg.sprite = Resources.Load<Sprite>(
            PathInfo.SPRITE + stat.src
            );
    }

    public void initStat(ItemVO itemVO)
    {
        this.type = InventoryType.Item;
        stat = ItemCommonFunction.generateItemInventoryStat(itemVO);
        bg = transform.Find("Image").GetComponent<Image>();
        bg.sprite = Resources.Load<Sprite>(
            PathInfo.SPRITE + stat.src
            );
    }

    public void truncateStat()
    {
        stat = null;
        bg.sprite = null;
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

    private void OnMouseDown()
    {
        Debug.Log("Down!");
    }

    private void OnMouseUp()
    {
        Debug.Log("UP!");
    }

    private void adjustSize()
    {
        GetComponent<BoxCollider2D>().size = GetComponent<RectTransform>().sizeDelta;
    }
}
