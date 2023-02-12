using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    Transform slotPrefab;
    [SerializeField]
    Transform contentPrefab;
    [SerializeField]
    InfoWindowController infoWindowController;
    [SerializeField]
    private bool isInitWithToggle = false;
    bool isInit = false;
    Transform monsterSlot;
    Transform equipmentSlot;
    Transform itemSlot;

    public ContentController curSelectedMonster;
    private MonsterInfoController monsterInfoController;

    private bool isForEntry = false;

    void Start()
    {
        initSetting();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInit)
        {
            adjustSize();
            trackCamera();
        }
        else
        {
            initSetting();
        }
    }

    private void initSetting()
    {
        LocalStorage.inventory["monsters"] = new List<SlotController>();
        LocalStorage.inventory["equipments"] = new List<SlotController>();
        LocalStorage.inventory["items"] = new List<SlotController>();
        transform.localScale = new Vector3(1f, 1f, 1f);
        transform.localPosition = new Vector3(0f, 0f, 1f);
        GetComponent<RectTransform>().sizeDelta = MainGameManager.screenSize;
        monsterSlot = transform.GetChild(0).GetChild(1);
        equipmentSlot = transform.GetChild(0).GetChild(3);
        itemSlot = transform.GetChild(0).GetChild(5);
        monsterSlot.GetComponent<GridLayoutGroup>().constraintCount = 4;
        for (int i = 0; i < 8; i++)
        {
            addSlot(InventoryType.Monster, monsterSlot);
        }
        for (int i = 0; i < 2; i++)
        {
            addSlot(InventoryType.Equipment, equipmentSlot);
            addSlot(InventoryType.Item, itemSlot);
        }
        for (int i = 0; i < 6; i++)
        {
            addSlot(InventoryType.Item, itemSlot);
        }
        monsterInfoController = transform.GetChild(1).GetComponent<MonsterInfoController>();

        ContentController.inventoryManager = this;
        loadInventory();
        isInit = true;
        adjustSize();
        if (isInitWithToggle)
        {
            transform.GetChild(0).GetComponent<ObjectMoveController>().toggle();
            transform.GetChild(1).GetComponent<ObjectMoveController>().toggle();
        }
    }

    private void trackCamera()
    {
        transform.position = new Vector3(
            Camera.main.transform.position.x,
            transform.position.y,
            1f);
        transform.localPosition = new Vector3(
            transform.localPosition.x,
            transform.localPosition.y,
            1f);
    }

    void adjustSize()
    {
        GetComponent<RectTransform>().sizeDelta = MainGameManager.screenSize;
        Transform temp = transform.GetChild(0);

        Transform temp2 = temp.GetChild(0);
        temp2.GetComponent<TextMeshProUGUI>().fontSize = MainGameManager.adjustFontSize;
        temp2.GetComponent<RectTransform>().sizeDelta = new Vector2(
           temp2.GetComponent<RectTransform>().sizeDelta.x,
           MainGameManager.screenUnitSize * 0.25f
           );
        temp2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -MainGameManager.screenUnitSize * 0.25f);

        monsterSlot.GetComponent<RectTransform>().sizeDelta = new Vector2(
           monsterSlot.GetComponent<RectTransform>().sizeDelta.x,
            MainGameManager.screenUnitSize * 2.25f);
        monsterSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -MainGameManager.screenUnitSize * 0.5f);
        monsterSlot.GetComponent<GridLayoutGroup>().cellSize = Vector2.one * MainGameManager.screenUnitSize * 0.8f;
        monsterSlot.GetComponent<GridLayoutGroup>().spacing = Vector2.one * MainGameManager.screenUnitSize * 0.1f;

        temp2 = temp.GetChild(2);
        temp2.GetComponent<TextMeshProUGUI>().fontSize = MainGameManager.adjustFontSize;
        temp2.GetComponent<RectTransform>().sizeDelta = new Vector2(
            temp2.GetComponent<RectTransform>().sizeDelta.x,
            MainGameManager.screenUnitSize * 0.25f);
        temp2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -MainGameManager.screenUnitSize * 2.75f);

        equipmentSlot.GetComponent<GridLayoutGroup>().cellSize = Vector2.one * MainGameManager.screenUnitSize * 0.8f;
        equipmentSlot.GetComponent<GridLayoutGroup>().spacing = Vector2.one * MainGameManager.screenUnitSize * 0.1f;

        temp2 = temp.GetChild(4);
        temp2.GetComponent<TextMeshProUGUI>().fontSize = MainGameManager.adjustFontSize;
        temp2.GetComponent<RectTransform>().sizeDelta = new Vector2(
            temp2.GetComponent<RectTransform>().sizeDelta.x,
            MainGameManager.screenUnitSize * 0.25f);
        temp2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -MainGameManager.screenUnitSize * 4.25f);

        itemSlot.GetComponent<RectTransform>().sizeDelta = new Vector2(
            itemSlot.GetComponent<RectTransform>().sizeDelta.x,
            MainGameManager.screenUnitSize * 2.25f);
        itemSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -MainGameManager.screenUnitSize * 4.5f);
        itemSlot.GetComponent<GridLayoutGroup>().cellSize = Vector2.one * MainGameManager.screenUnitSize * 0.8f;
        itemSlot.GetComponent<GridLayoutGroup>().spacing = Vector2.one * MainGameManager.screenUnitSize * 0.1f;
    }

    void addSlot(InventoryType type, Transform targetParent)
    {
        Transform newSlot = Instantiate(slotPrefab);
        newSlot.SetParent(targetParent);
        newSlot.localPosition = Vector3.one;
        newSlot.localScale = Vector3.one;
        newSlot.GetComponent<SlotController>().initSlot(type);
        LocalStorage.inventory[targetParent.name].Add(newSlot.GetComponent<SlotController>());
    }

    public void mountItemToMonster(SlotController targetSlot, ContentController targetItem)
    {
        if (curSelectedMonster == null) return;
        LocalStorage.Live.items.Remove(targetItem.itemLiveStat.saveStat.id);
        curSelectedMonster.addItem(targetItem.itemLiveStat);
        targetSlot.installContent(targetItem.transform);
        monsterInfoController.initInfo(curSelectedMonster.monsterLiveStat);
    }

    public void unMountItemFromMonster(SlotController targetSlot, ContentController targetItem)
    {
        curSelectedMonster.removeItem(targetItem.itemLiveStat);
        targetSlot.installContent(targetItem.transform);
        monsterInfoController.initInfo(curSelectedMonster.monsterLiveStat);
        LocalStorage.Live.items[targetItem.itemLiveStat.saveStat.id] = targetItem.itemLiveStat;
    }

    public void reloadInventory()
    {
        foreach (List<SlotController> slotControllers in LocalStorage.inventory.Values)
        {
            foreach (SlotController slotController in slotControllers)
            {
                slotController.truncateContent();
            }
        }
        loadInventory();
    }

    void loadInventory()
    {
        foreach (KeyValuePair<string, MonsterLiveStat> monsterLive in LocalStorage.Live.monsters)
        {
            Transform newMonster = Instantiate(contentPrefab);
            newMonster.GetComponent<ContentController>().initContent(monsterLive.Value);
            foreach (SlotController slotController in LocalStorage.inventory["monsters"])
            {
                if (!slotController.isOccupied())
                {
                    slotController.installContent(newMonster);
                    break;
                }
            }
        }
        foreach (KeyValuePair<string, ItemLiveStat> itemLive in LocalStorage.Live.items)
        {
            Transform newItem = Instantiate(contentPrefab);
            newItem.GetComponent<ContentController>().initContent(itemLive.Value);
            newItem.GetComponent<ContentController>().setInfoWindowController(infoWindowController);
            foreach (SlotController slotController in LocalStorage.inventory["items"])
            {
                if (!slotController.isOccupied())
                {
                    slotController.installContent(newItem);
                    break;
                }
            }
        }
        curSelectedMonster = null;
    }

    public void selectMonster(ContentController selectedMonster)
    {
        curSelectedMonster = selectedMonster;
        truncateEquipment();
        if (selectedMonster != null)
        {
            generateEquipmentItems(selectedMonster.getItemLiveStat());
            monsterInfoController.initInfo(selectedMonster.monsterLiveStat);
        }
        else
        {
            monsterInfoController.truncateData();
        }
    }

    public void viewExpectation(MonsterLiveStat candidateMonsterLiveStat)
    {
        monsterInfoController.viewExpectation(candidateMonsterLiveStat);
    }

    public void feedMonster(ContentController contentController)
    {
        // 경험치 멕이기
        curSelectedMonster.feedMosnter(contentController.monsterLiveStat.saveStat.exp);

        // 아이템 인벤토리로 보내기
        returnItemsToInventoryFromMonster(contentController);

        // 멕인 몬스터 삭제
        LocalStorage.Live.monsters.Remove(contentController.monsterLiveStat.saveStat.id);
        Destroy(contentController.gameObject);

        // 정보창 새로고침
        selectMonster(curSelectedMonster);
    }

    public void sellContent(ContentController targetContentController)
    {
        // 재화 증가
        CommerceFunction.sellContent(targetContentController);
        // 몬스터?
        //      아이템 인벤토리로 보내기
        //  아이템?
        //      장착 된거? 안된거?
        //          걍 얘만 팔고 끝
        switch (targetContentController.type)
        {
            case InventoryType.Monster:
                returnItemsToInventoryFromMonster(targetContentController);
                LocalStorage.Live.monsters.Remove(targetContentController.monsterLiveStat.saveStat.id);
                break;
            case InventoryType.Item:
                if (targetContentController.itemLiveStat.saveStat.equipMonsterId != null)
                {
                    LocalStorage.Live.monsters[targetContentController.itemLiveStat.saveStat.equipMonsterId].itemStatList.Remove(targetContentController.itemLiveStat.saveStat.id);
                }
                else
                {
                    LocalStorage.Live.items.Remove(targetContentController.itemLiveStat.saveStat.id);
                }
                break;
        }
        Destroy(targetContentController.gameObject);

        // 정보창 새로고침
        selectMonster(null);
    }

    private void returnItemsToInventoryFromMonster(ContentController contentController)
    {
        foreach (KeyValuePair<string, ItemLiveStat> itemLive in contentController.monsterLiveStat.itemStatList)
        {
            itemLive.Value.saveStat.equipMonsterId = null;
            LocalStorage.Live.items[itemLive.Value.saveStat.id] = itemLive.Value;
            Transform newItem = Instantiate(contentPrefab);
            newItem.GetComponent<ContentController>().initContent(itemLive.Value);
            newItem.GetComponent<ContentController>().setInfoWindowController(infoWindowController);
            foreach (SlotController slotController in LocalStorage.inventory["items"])
            {
                if (!slotController.isOccupied())
                {
                    slotController.installContent(newItem);
                    break;
                }
            }
        }
    }

    private void truncateEquipment()
    {
        foreach (SlotController equipSlot in LocalStorage.inventory["equipments"])
        {
            equipSlot.truncateContent();
        }
    }

    private void generateEquipmentItems(List<ItemLiveStat> itemLiveStats)
    {
        foreach (ItemLiveStat itemLiveStat in itemLiveStats)
        {
            Transform newItem = Instantiate(contentPrefab);
            newItem.GetComponent<ContentController>().initContent(itemLiveStat);
            newItem.GetComponent<ContentController>().setInfoWindowController(infoWindowController);
            foreach (SlotController slotController in LocalStorage.inventory["equipments"])
            {
                if (!slotController.isOccupied())
                {
                    slotController.installContent(newItem);
                    break;
                }
            }
        }
    }

    public void hideInfoWindow()
    {
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public void setEntriable(bool isForEntry)
    {
        this.isForEntry = isForEntry;
        transform.GetChild(0).GetChild(6).gameObject.SetActive(!isForEntry);
    }

    public bool getIsForEntry()
    {
        return isForEntry;
    }

    public ContentController generateMonsterContentController()
    {
        Transform res = Instantiate(contentPrefab);
        return res.GetComponent<ContentController>();
    }
}
