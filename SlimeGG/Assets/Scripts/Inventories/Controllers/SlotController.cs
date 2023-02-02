using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotController : MonoBehaviour
{
    [SerializeField]
    InventoryType type = InventoryType.Item;
    private bool isMouseOn = false;
    float cntMouseOn = 0f;
    bool isWindowOpen = false;
    private bool isClicked = false;
    private Image bg { get; set; }
    private InventoryStat stat = null;

    InfoWindowController infoWindowController;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        adjustSize();
        if (stat != null && type == InventoryType.Item)
        {
            checkMouseForWindow();
        }
        if (isClicked && Input.GetMouseButton(0) && type == InventoryType.Item)
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
        isMouseOn = true;
    }

    private void OnMouseExit()
    {
        isMouseOn = false;
        cntMouseOn = 0f;
        isWindowOpen = false;
        infoWindowController.closeWindow();
    }

    private void OnMouseDown()
    {
        isClicked = true;
    }

    private void OnMouseUp()
    {
        isClicked = false;
    }

    private void adjustSize()
    {
        GetComponent<BoxCollider2D>().size = GetComponent<RectTransform>().sizeDelta;
    }
    public void setInfoWindowController(InfoWindowController infoWindowController)
    {
        this.infoWindowController = infoWindowController;
    }

    private void checkMouseForWindow()
    {
        if (isMouseOn)
        {
            if (cntMouseOn < 0.25f)
            {
                cntMouseOn += Time.deltaTime;
            }
            else
            {
                if (!isWindowOpen)
                {
                    // open info window
                    isWindowOpen = true;
                    infoWindowController.openWindow();
                }
            }
        }
    }
}
