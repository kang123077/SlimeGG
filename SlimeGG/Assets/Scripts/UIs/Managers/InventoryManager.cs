using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    Transform monsterSlot;
    Transform equipmentSlot;
    Transform itemSlot;

    public ContentController curSelectedMonster;
    private MonsterInfoController monsterInfoController;

    public bool isInfoNeeded;
    private Vector2 invenSize = new Vector2(4f, 7f);

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
            checkKeyPress();
        }
        else
        {
            initSetting();
        }
    }

    private void initSetting()
    {
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
        if (isInfoNeeded)
            monsterInfoController = transform.GetChild(1).GetComponent<MonsterInfoController>();

        ContentController.inventoryManager = this;
        loadInventory();
        isInit = true;
        adjustSize();
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
        LocalStorage.UIOpenStatus.inventory = !isActive;
        LocalStorage.UIOpenStatus.info = !isActive;
        isActive = !isActive;
        isAnimating = true;
        while (isActive
            ? (transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.x + MainGameManager.screenUnitSize < -1f)
            : ((transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.x + MainGameManager.screenUnitSize * invenSize.x) > 1f)
            )
        {
            yield return new WaitForSeconds(0.01f);
            transform.GetChild(0).Translate(
                (isActive
                ? Vector3.right
                : Vector3.left
                ) *
                (isActive
                ? (
                -transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.x + MainGameManager.screenUnitSize
                )
                : (
                transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.x + MainGameManager.screenUnitSize * invenSize.x
                )
                )
                * 0.01f
                * SettingVariables.slideToggleSpd
                );
            if (isInfoNeeded)
            {
                transform.GetChild(1).Translate(
                    (isActive
                    ? Vector3.left
                    : Vector3.right
                    ) *
                    (isActive
                    ? (
                    transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition.x - MainGameManager.screenUnitSize + (MainGameManager.screenUnitSize * monsterInfoController.size.x)
                    )
                    : (
                    -transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition.x
                    )
                    )
                    * 0.01f
                    * SettingVariables.slideToggleSpd
                    );
            }
        }
        isAnimating = false;
        if (!isActive)
        {
            // 정보 날리기
            selectMonster(null);
        }
    }

    void adjustSize()
    {
        GetComponent<RectTransform>().sizeDelta = MainGameManager.screenSize;
        transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(MainGameManager.screenUnitSize * invenSize.x, MainGameManager.screenUnitSize * -2f);
        if (isInfoNeeded)
            transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(MainGameManager.screenUnitSize * monsterInfoController.size.x, MainGameManager.screenUnitSize * -2f);
        if (!isAnimating)
        {
            transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition =
                isActive
                ? Vector2.right * MainGameManager.screenUnitSize
                : (Vector2.left * MainGameManager.screenUnitSize * invenSize.x);
            if (isInfoNeeded)
                transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition =
                    isActive
                    ? ((Vector2.left * MainGameManager.screenUnitSize * monsterInfoController.size.x) - (Vector2.right * MainGameManager.screenUnitSize))
                    : Vector2.zero;
        }
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

        equipmentSlot.GetComponent<RectTransform>().sizeDelta = new Vector2(
           equipmentSlot.GetComponent<RectTransform>().sizeDelta.x,
            MainGameManager.screenUnitSize * 1.25f);
        equipmentSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -MainGameManager.screenUnitSize * 3f);
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
        curSelectedMonster.addItem(targetItem.itemLiveStat);
        targetSlot.installContent(targetItem.transform);
        if (isInfoNeeded)
            monsterInfoController.initInfo(curSelectedMonster.monsterLiveStat);
    }

    public void unMountItemFromMonster(SlotController targetSlot, ContentController targetItem)
    {
        curSelectedMonster.removeItem(targetItem.itemLiveStat);
        targetSlot.installContent(targetItem.transform);
        if (isInfoNeeded)
            monsterInfoController.initInfo(curSelectedMonster.monsterLiveStat);
        LocalStorage.Live.items[targetItem.itemLiveStat.saveStat.id] = targetItem.itemLiveStat;
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
        if (selectedMonster != null)
        {
            generateEquipmentItems(selectedMonster.getItemLiveStat());
            if (isInfoNeeded)
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
        isInfoNeeded = false;
        transform.GetChild(1).gameObject.SetActive(false);
    }
}
