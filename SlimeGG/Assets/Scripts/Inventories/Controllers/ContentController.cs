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

    public bool isInstalledOnField = false;
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
        if (prevPerant != null)
        {
            if (prevPerant.GetComponent<EntrySlotController>() != null)
            {
                // 직전에 필드에 있었다 -> 필드에서 제거
                prevPerant.GetComponent<EntrySlotController>().truncateMonster(this, false);
                return;
            }
            if (prevPerant.GetComponent<SlotController>() != null)
            {
                // 직전에 슬롯에 있었다 -> 슬롯에서 제거
                prevPerant.GetComponent<SlotController>().removeContent();
                return;
            }
        }
    }

    private void OnMouseUp()
    {
        if (!canMove) return;
        if (Vector3.Distance(mousePos, Camera.main.ScreenToWorldPoint(Input.mousePosition)) < 0.15f)
        {
            if (inventoryManager.getIsForEntry())
            {
                // 다시 붙이기
                if (prevPerant != null)
                {
                    if (prevPerant.GetComponent<EntrySlotController>() != null)
                        prevPerant.GetComponent<EntrySlotController>().installMonster(this, false);
                    if (prevPerant.GetComponent<SlotController>() != null)
                        prevPerant.GetComponent<SlotController>().installContent(transform);
                }
            }
            else
            {
                if (prevPerant.GetComponent<SlotController>() != null)
                    prevPerant.GetComponent<SlotController>().installContent(transform);
            }
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
            // 전투씬임
            if (!isInstalledOnField)
            {
                // 설치 안되어있음
                // 이전 위치로 도르마무
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
            // 전투 필드 배치 판별
            if (inventoryManager.getIsForEntry())
            {
                if (type == InventoryType.Monster)
                {
                    checkIsOnFieldTile(res.transform);
                    return;
                }
            }

            // 판매인지 판별
            if (res.transform.name == "Sell")
            {
                if (infoWindowController != null)
                {
                    infoWindowController.closeWindow();
                }
                inventoryManager.sellContent(this);
                return;
            }

            // 슬롯 있는지?
            if (res.transform.tag == "Slot")
            {
                SlotController slot = res.transform.GetComponent<SlotController>();
                // 이 컨텐츠가 무엇인가?
                switch (type)
                {
                    case InventoryType.Monster:
                        // 몬스터
                        if (slot.type == InventoryType.Monster)
                        {
                            // 빈 슬롯인가?
                            if (!slot.isOccupied())
                            {
                                // 몬스터 칸 -> 몬스터 칸
                                prevPerant.GetComponent<SlotController>().removeContent();
                                slot.installContent(transform);
                                return;
                            }
                        }
                        break;
                    case InventoryType.Item:
                        // 아이템
                        if (slot.type == InventoryType.Equipment)
                        {
                            if (itemLiveStat.saveStat.equipMonsterId == null)
                            {
                                // 아이템 칸 -> 장착 칸
                                inventoryManager.mountItemToMonster(slot, this);
                                prevPerant = slot.transform;
                                return;
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
                                return;
                            }
                            else
                            {
                                // 아이템 칸 -> 아이템 칸
                                prevPerant.GetComponent<SlotController>().removeContent();
                                slot.installContent(transform);
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
        if (objectBelow.parent != null && objectBelow.parent.tag == "Tile" && objectBelow.parent.GetComponent<EntrySlotController>().isAlly && !objectBelow.parent.GetComponent<EntrySlotController>().isPosessed)
        {
            // 전투 필드에 배치
            if (prevPerant != null && prevPerant.GetComponent<EntrySlotController>() != null)
            {
                // 이전 필드에서 해당 몬스터 정보 제거
                prevPerant.GetComponent<EntrySlotController>().truncateMonster(this, false);
            }
            // 새로운 필드에 배치
            prevPerant = objectBelow;
            objectBelow.parent.GetComponent<EntrySlotController>().installMonster(this, false);
            return;
        }
        if (objectBelow.tag == "Slot")
        {
            // 전투 필드에서 제거:: 전투 필드 -> 인벤토리
            isInstalledOnField = false;
            if (prevPerant && prevPerant.GetComponent<EntrySlotController>())
            {
                // 이전 필드에서 해당 몬스터 정보 제거
                prevPerant.GetComponent<EntrySlotController>().truncateMonster(this);
            }
            // 슬롯으로 도르마무
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
