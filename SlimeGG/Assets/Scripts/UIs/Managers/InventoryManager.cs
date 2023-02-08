using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    string keyToToggle;
    [SerializeField]
    Transform slotPrefab;
    [SerializeField]
    Transform contentPrefab;
    [SerializeField]
    InfoWindowController infoWindowController;
    bool isActive = false;
    bool isAnimating = false;
    bool isInit = false;
    Transform slots;
    Transform monsterSlot;
    Transform equipmentSlot;
    Transform itemSlot;

    private ContentController curSelectedMonster;
    private MonsterInfoController monsterInfoController;

    void Start()
    {
        initSetting();
        loadInventory();
        isInit = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInit)
        {
            adjustSize();
            trackCamera();
            checkKeyPress();
        }
    }

    private void initSetting()
    {
        slots = transform.Find("Slots");
        transform.localScale = new Vector3(1f, 1f, 1f);
        transform.localPosition = new Vector3(0, MainGameManager.screenSize.y, 1f);
        GetComponent<RectTransform>().sizeDelta = MainGameManager.screenSize;
        monsterInfoController = slots.GetChild(1).GetComponent<MonsterInfoController>();
        monsterSlot = slots.GetChild(0).GetChild(1);
        equipmentSlot = slots.GetChild(0).GetChild(3);
        itemSlot = slots.GetChild(0).GetChild(5);
        monsterSlot.GetComponent<GridLayoutGroup>().constraintCount = 6;
        for (int i = 0; i < 18; i++)
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

        ContentController.inventoryManager = this;
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

    private void checkKeyPress()
    {
        if (Input.GetKeyDown(keyToToggle) && !isAnimating)
        {
            StartCoroutine(toggleCoroutine());
        }
    }

    private IEnumerator toggleCoroutine()
    {
        isActive = !isActive;
        LocalStorage.IS_CAMERA_FREE = false;
        isAnimating = true;
        while (isActive ? (transform.position.y > 0.01) : (transform.position.y < 9.99))
        {
            yield return new WaitForSeconds(0.01f);
            transform.Translate((isActive ? Vector3.down : Vector3.up) * (transform.position.y > 1 ? transform.position.y : 1f) * Time.deltaTime * 60);
        }
        transform.position = new Vector3(
            transform.position.x,
            isActive ? 0f : 10f,
            1f
            );
        isAnimating = false;
        LocalStorage.IS_CAMERA_FREE = !isActive;
    }

    void adjustSize()
    {
        GetComponent<RectTransform>().sizeDelta = MainGameManager.screenSize;
        slots.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(MainGameManager.screenSize.x * 6f / 16f, 0f);
        slots.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(MainGameManager.screenSize.x * 10f / 16f, 0f);
        slots.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-MainGameManager.screenSize.x * 10f / 16f, 0f);
        if (!isAnimating)
        {
            transform.localPosition = new Vector3(0, isActive ? 0f : MainGameManager.screenSize.y, 1f);
        }
        Transform temp = slots.GetChild(0);

        Transform temp2 = temp.GetChild(0);
        temp2.GetComponent<RectTransform>().sizeDelta = new Vector2(
           temp2.GetComponent<RectTransform>().sizeDelta.x,
            MainGameManager.screenSize.y * 0.05f);
        temp2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -MainGameManager.screenSize.y * 0.05f);

        monsterSlot.GetComponent<RectTransform>().sizeDelta = new Vector2(
           monsterSlot.GetComponent<RectTransform>().sizeDelta.x,
            MainGameManager.screenSize.y * 0.3f);
        monsterSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -MainGameManager.screenSize.y * 0.1f);
        monsterSlot.GetComponent<GridLayoutGroup>().cellSize = Vector2.one * MainGameManager.screenSize.y * 0.1f * 0.8f;
        monsterSlot.GetComponent<GridLayoutGroup>().spacing = Vector2.one * MainGameManager.screenSize.y * 0.1f * 0.2f;

        temp2 = temp.GetChild(2);
        temp2.GetComponent<RectTransform>().sizeDelta = new Vector2(
            temp2.GetComponent<RectTransform>().sizeDelta.x,
            MainGameManager.screenSize.y * 0.05f);
        temp2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -MainGameManager.screenSize.y * 0.4f);

        equipmentSlot.GetComponent<RectTransform>().sizeDelta = new Vector2(
           equipmentSlot.GetComponent<RectTransform>().sizeDelta.x,
            MainGameManager.screenSize.y * 0.15f);
        equipmentSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -MainGameManager.screenSize.y * 0.45f);
        equipmentSlot.GetComponent<GridLayoutGroup>().cellSize = Vector2.one * MainGameManager.screenSize.y * 0.1f * 1.2f;
        equipmentSlot.GetComponent<GridLayoutGroup>().spacing = Vector2.one * MainGameManager.screenSize.y * 0.1f * 0.2f;

        temp2 = temp.GetChild(4);
        temp2.GetComponent<RectTransform>().sizeDelta = new Vector2(
            temp2.GetComponent<RectTransform>().sizeDelta.x,
            MainGameManager.screenSize.y * 0.05f);
        temp2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -MainGameManager.screenSize.y * 0.6f);

        itemSlot.GetComponent<RectTransform>().sizeDelta = new Vector2(
            itemSlot.GetComponent<RectTransform>().sizeDelta.x,
            MainGameManager.screenSize.y * 0.3f);
        itemSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -MainGameManager.screenSize.y * 0.65f);
        itemSlot.GetComponent<GridLayoutGroup>().cellSize = Vector2.one * MainGameManager.screenSize.y * 0.1f * 1.2f;
        itemSlot.GetComponent<GridLayoutGroup>().spacing = Vector2.one * MainGameManager.screenSize.y * 0.1f * 0.2f;
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
        curSelectedMonster.addItem(targetItem.itemLiveStat);
        targetSlot.installContent(targetItem.transform);
    }

    public void unMountItemFromMonster(SlotController targetSlot, ContentController targetItem)
    {
        curSelectedMonster.removeItem(targetItem.itemLiveStat);
        targetSlot.installContent(targetItem.transform);
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
    }

    public void selectMonster(ContentController selectedMonster)
    {
        curSelectedMonster = selectedMonster;
        truncateEquipment();
        generateEquipmentItems(selectedMonster.getItemLiveStat());
        monsterInfoController.initInfo(selectedMonster.monsterLiveStat);
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
}
