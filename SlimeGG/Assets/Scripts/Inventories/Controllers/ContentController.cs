using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ContentController : MonoBehaviour
{
    public static InventoryManager inventoryManager;
    public InventoryType type = InventoryType.None;
    public MonsterLiveStat monsterLiveStat;
    public ItemLiveStat itemLiveStat;
    private Transform image;
    private SpriteRenderer spriteRenderer;
    bool isMoving = false;

    float cntMouseOn = 0f;
    bool isMouseOn = false;
    bool isWindowOpen = false;
    InfoWindowController infoWindowController;

    private Vector3 mousePos;
    private Transform prevPerant;
    private Vector2 prevSize;

    private bool isInstalledOnField = false;
    private bool canMove = true;
    // Start is called before the first frame update
    void Start()
    {
        image = transform.GetChild(0);
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
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

    public void initContent(MonsterLiveStat monsterLiveStat, bool canMove = true)
    {
        this.canMove = canMove;
        type = InventoryType.Monster;
        this.monsterLiveStat = monsterLiveStat;
        if (image == null)
        {
            image = transform.GetChild(0);
            spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
        image.GetComponent<Image>().sprite = Resources.Load<Sprite>(
            $"{PathInfo.Monster.Sprite}{monsterLiveStat.saveStat.speicie}"
            );
        spriteRenderer.sprite = Resources.Load<Sprite>(
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
            spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
        image.GetComponent<Image>().sprite = Resources.Load<Sprite>(
            $"{PathInfo.Item.Sprite}{itemLiveStat.saveStat.itemName}"
            );
        spriteRenderer.sprite = Resources.Load<Sprite>(
        $"{PathInfo.Item.Sprite}{itemLiveStat.saveStat.itemName}"
        );
    }

    private void adjustSize()
    {
        if (image != null && transform.parent != null && !isInstalledOnField)
        {
            Vector2 temp = transform.parent.GetComponent<RectTransform>().sizeDelta;
            image.GetComponent<BoxCollider>().size = new Vector3(temp.x, temp.y, 0.2f);
        }
    }

    private void OnMouseDown()
    {
        if (LocalStorage.CURRENT_SCENE == "Battle")
        {
            if (type == InventoryType.Monster)
            {
                // 전투 정보창 띄우기
                Debug.Log("전투 정보창 띄우기!");
            }
        }
        if (!canMove) return;
        prevPerant = transform.parent;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (!isMoving) isMoving = true;
        LocalStorage.isCameraPosessed = true;
        if (type == InventoryType.Monster)
        {
            inventoryManager.selectMonster(this);
        }
        isInstalledOnField = false;
    }

    private void OnMouseUp()
    {
        if (!canMove) return;
        if (Vector3.Distance(mousePos, Camera.main.ScreenToWorldPoint(Input.mousePosition)) < 0.15f)
        {
        }
        else
        {
            if (isMoving)
            {
                checkBelow();
            }
        }
        mousePos = Vector3.zero;
        isMoving = false;
        LocalStorage.isCameraPosessed = false;
        if (inventoryManager.getIsForEntry())
        {
            if (!isInstalledOnField)
            {
                transform.SetParent(prevPerant);
                prevPerant = null;
                transform.localScale = Vector3.one;
                transform.GetComponent<RectTransform>().sizeDelta = prevSize;
                image.transform.GetComponent<BoxCollider>().size = prevSize;
                prevSize = Vector2.zero;
            }
        }
        if (!isInstalledOnField)
        {
            transform.localPosition = new Vector3(0f, 0f, -2f);
        }
    }

    private void onDrag()
    {
        if (isMoving)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (inventoryManager.getIsForEntry())
            {
                transform.localPosition = new Vector3(
                    transform.localPosition.x, transform.localPosition.y, 8f);
                if (transform.parent != null)
                {
                    prevPerant = transform.parent;
                    prevSize = transform.GetComponent<RectTransform>().sizeDelta;
                    transform.SetParent(null);
                    transform.localScale = Vector3.one;
                    transform.GetComponent<RectTransform>().sizeDelta = Vector2.one;
                    image.transform.GetComponent<BoxCollider>().size = new Vector3(1f, 1f, 0.2f);
                }
            }
            else
            {
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
    }

    private void checkBelow()
    {
        RaycastHit res;
        if (Physics.Raycast(transform.position, Vector3.forward, out res, 20f))
        {
            if (inventoryManager.getIsForEntry())
            {
                if (type == InventoryType.Monster)
                {
                    checkIsOnFieldTile(res.transform);
                    return;
                }
            }
            if (res.transform.name == "Sell")
            {
                // 판매
                if (infoWindowController != null)
                {
                    infoWindowController.closeWindow();
                }
                inventoryManager.sellContent(this);
                return;
            }
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
                                // 아이템 칸 -> 장착 칸
                                inventoryManager.mountItemToMonster(slot, this);
                                prevPerant = slot.transform;
                            }
                        }
                        else if (slot.type == InventoryType.Item)
                        {
                            if (itemLiveStat.saveStat.equipMonsterId != null)
                            {
                                // 장착 칸 -> 아이템 칸
                                prevPerant.GetComponent<SlotController>().removeContent();
                                inventoryManager.unMountItemFromMonster(slot, this);
                                prevPerant = slot.transform;
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
                            // 경험치 멕이기
                            if (!isFeedable(content.monsterLiveStat.saveStat.exp)) return;
                            inventoryManager.feedMonster(this);
                        }
                        break;
                    default: break;
                }
            }
        }
    }

    private void checkIsOnFieldTile(Transform objectBelow)
    {
        if (objectBelow.parent != null && objectBelow.parent.tag == "Tile" && !objectBelow.parent.GetComponent<EntrySlotController>().isPosessed)
        {
            isInstalledOnField = true;
            prevPerant = objectBelow;
            objectBelow.parent.GetComponent<EntrySlotController>().installMonster(this);
            return;
        }
        if (objectBelow.tag == "Slot")
        {
            isInstalledOnField = false;
            if (prevPerant.GetComponent<EntrySlotController>() != null)
            {
                prevPerant.GetComponent<EntrySlotController>().truncateMonster();
            }
            prevPerant = objectBelow;
            prevSize = Vector2.one;
            objectBelow.GetComponent<SlotController>().installContent(transform);
            return;
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
        if (isWindowOpen && infoWindowController != null)
        {
            infoWindowController.initInfo(null, null);
            infoWindowController.closeWindow();
        }
        isWindowOpen = false;
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
