using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ContentController : MonoBehaviour
{
    public static InventoryManager inventoryManager;
    InventoryType type = InventoryType.None;
    public MonsterLiveStat monsterLiveStat;
    public ItemLiveStat itemLiveStat;
    Transform image;
    bool isMoving = false;

    float cntMouseOn = 0f;
    bool isMouseOn = false;
    bool isWindowOpen = false;
    InfoWindowController infoWindowController;

    private Vector3 mousePos;
    // Start is called before the first frame update
    void Start()
    {
        image = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        adjustSize();
        onDrag();
        if (type == InventoryType.Item && infoWindowController != null)
        {
            checkMouseForWindow();
        }
    }

    public void initContent(MonsterLiveStat monsterLiveStat)
    {
        type = InventoryType.Monster;
        this.monsterLiveStat = monsterLiveStat;
        if (image == null)
        {
            image = transform.GetChild(0);
        }
        image.GetComponent<Image>().sprite = Resources.Load<Sprite>(
            $"{PathInfo.Monster.Sprite}{monsterLiveStat.saveStat.speicie}"
            );
    }

    public void initContent(ItemLiveStat itemLiveStat)
    {
        type = InventoryType.Item;
        this.itemLiveStat = itemLiveStat;
        if (image == null)
        {
            image = transform.GetChild(0);
        }
        image.GetComponent<Image>().sprite = Resources.Load<Sprite>(
            $"{PathInfo.Item.Sprite}{itemLiveStat.saveStat.itemName}"
            );
    }

    private void adjustSize()
    {
        if (image != null && transform.parent != null)
        {
            Vector2 temp = transform.parent.GetComponent<RectTransform>().sizeDelta;
            image.GetComponent<BoxCollider>().size = new Vector3(temp.x, temp.y, 0.2f);
        }
    }

    private void OnMouseDown()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (!isMoving) isMoving = true;
        LocalStorage.isCameraPosessed = true;
        if (type == InventoryType.Monster)
        {
            inventoryManager.selectMonster(this);
        }
    }

    private void OnMouseUp()
    {
        if (Vector3.Distance(mousePos, Camera.main.ScreenToWorldPoint(Input.mousePosition)) < 0.15f)
        {
        }
        else
        {
            if (isMoving)
            {
                checkInstallable();
            }
        }
        mousePos = Vector3.zero;
        isMoving = false;
        LocalStorage.isCameraPosessed = false;
        transform.localPosition = new Vector3(0f, 0f, -2f);
    }

    private void onDrag()
    {
        if (isMoving)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.localPosition = new Vector3(
                transform.localPosition.x, transform.localPosition.y, -13f);
            if (type != InventoryType.Item)
            {
                RaycastHit res;
                if (Physics.Raycast(transform.position, Vector3.forward, out res, 1.2f))
                {
                    if (res.transform.tag == "Content")
                    {
                        ContentController content = res.transform.GetComponent<ContentController>();
                        if (content == null) return;
                        if (content.type == InventoryType.Item) return;
                        if (content.monsterLiveStat.saveStat.id == monsterLiveStat.saveStat.id) return;
                        switch (type)
                        {
                            case InventoryType.Monster:
                                if (content.type == InventoryType.Monster)
                                {
                                    if (inventoryManager.curSelectedMonster.monsterLiveStat.saveStat.id
                                        != content.monsterLiveStat.saveStat.id)
                                        inventoryManager.selectMonster(content);
                                    inventoryManager.viewExpectation(monsterLiveStat);
                                }
                                break;
                            default: break;
                        }
                    }
                }
            }
        }
    }

    private void checkInstallable()
    {
        RaycastHit res;
        if (Physics.Raycast(transform.position, Vector3.forward, out res, 1.2f))
        {
            if (res.transform.tag == "Slot")
            {
                SlotController slot = res.transform.GetComponent<SlotController>();
                switch (type)
                {
                    case InventoryType.Monster:
                        break;
                    case InventoryType.Item:
                        if (slot.type == InventoryType.Equipment)
                        {
                            if (itemLiveStat.saveStat.equipMonsterId == null)
                            {
                                //Debug.Log("아이템 >> 장착칸");
                                inventoryManager.mountItemToMonster(slot, this);
                            }
                        }
                        else if (slot.type == InventoryType.Item)
                        {
                            if (itemLiveStat.saveStat.equipMonsterId != null)
                            {
                                //Debug.Log("장착칸 >> 아이템");
                                transform.parent.GetComponent<SlotController>().removeContent();
                                inventoryManager.unMountItemFromMonster(slot, this);
                            }
                        }
                        break;
                    case InventoryType.Equipment:
                        break;
                    case InventoryType.None:
                        break;
                }
            }
            else if (res.transform.tag == "Content")
            {
                ContentController content = res.transform.GetComponent<ContentController>();
                switch (type)
                {
                    case InventoryType.Monster:
                        if (content.type == InventoryType.Monster)
                        {
                            // 경험치를 하나라도 줄 수 있어야 함
                            if (!isFeedable(content.monsterLiveStat.saveStat.exp)) return;
                            inventoryManager.feedMonster(this);
                        }
                        break;
                    default: break;
                }
            }
        }
    }

    public List<ItemLiveStat> getItemLiveStat()
    {
        return monsterLiveStat.itemStatList.Values.ToList();
    }

    public void destroySelf()
    {
        Destroy(gameObject);
    }

    public void removeItem(ItemLiveStat item)
    {
        item.saveStat.equipMonsterId = null;
        monsterLiveStat.itemStatList.Remove(item.saveStat.id);
    }

    public void addItem(ItemLiveStat item)
    {
        item.saveStat.equipMonsterId = monsterLiveStat.saveStat.id;
        monsterLiveStat.itemStatList[item.saveStat.id] = item;
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
                    infoWindowController.initInfo(itemLiveStat.dictionaryStat.displayName, itemLiveStat.dictionaryStat.desc);
                    infoWindowController.openWindow();
                }
            }
        }
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
        if (infoWindowController != null)
        {
            infoWindowController.initInfo(null, null);
            infoWindowController.closeWindow();
        }
    }

    public void setInfoWindowController(InfoWindowController infoWindowController)
    {
        this.infoWindowController = infoWindowController;
    }

    public void feedMosnter(List<ElementStat> feedStats)
    {
        foreach (ElementStat stat in monsterLiveStat.saveStat.exp)
        {
            int i = 0;
            while (feedStats.Count > 0)
            {
                if (i >= feedStats.Count) break;
                if (feedStats[i].name == stat.name)
                {
                    stat.amount += feedStats[i].amount;
                    feedStats.RemoveAt(i);
                    break;
                }
                else
                {
                    i++;
                }
            }
        }
    }

    public bool isFeedable(List<ElementStat> feedStats)
    {
        foreach (ElementStat stat in monsterLiveStat.saveStat.exp)
        {
            int i = 0;
            while (feedStats.Count > 0)
            {
                if (i >= feedStats.Count) break;
                if (feedStats[i].name == stat.name)
                {
                    return true;
                }
                else
                {
                    i++;
                }
            }
        }
        return false;
    }
}
