using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardController : MonoBehaviour
{
    [SerializeField]
    private Transform slotPrefab, contentPrefab;
    [SerializeField]
    Button btnNext;
    private TextMeshProUGUI btnText;
    private TextMeshProUGUI cntText;

    private TextMeshProUGUI titleText;
    private ObjectMoveController objectMoveController;
    private GridLayoutGroup slotGridLayout;
    private int curSlotCnt;
    private List<SlotController> slotControllers;
    private int curStatus = 0;
    private int rerollLeftCnt = -1;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        switch (curStatus)
        {
            case 0:
                // 초기화
                initSetting();
                break;
            case 1:
                // 대기
                break;
            case 2:
                // 몬스터 보상: 컨텐츠 남아있을때
                if (!checkAllReceived())
                {
                    curStatus = 6;
                    return;
                }
                break;
            case 6:
                // 몬스터 보상: 컨텐츠 다 수령했을 때
                break;
            case 3:
                // 몬스터 보상 후 아이템 보상으로 넘어가는 사이
                // 대기
                break;
            case 4:
                // 아이템 보상: 컨텐츠 남아있을때
                if (!checkAllReceived())
                {
                    curStatus = 7;
                    return;
                }
                break;
            case 7:
                // 아이템 보상: 컨텐츠 다 수령했을 때
                break;
            case 5:
                // 아이템 보상 후 대기
                break;
        }
    }

    private void initSetting()
    {
        titleText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        titleText.text = $"";
        objectMoveController = GetComponent<ObjectMoveController>();
        slotGridLayout = transform.GetChild(1).GetComponent<GridLayoutGroup>();
        btnText = btnNext.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        cntText = btnNext.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        curStatus = 1;
    }

    public void openMonsterReward()
    {
        rerollLeftCnt = SettingVariables.Reward.RerollCount.monster;
        LocalStorage.UIOpenStatus.reward = true;
        titleText.text = $"몬스터 보상";
        btnText.text = $"다시 뽑기";
        updateCount(isInit: true);
        btnNext.onClick.RemoveAllListeners();
        btnNext.onClick.AddListener(() =>
        {
            rerollLeftContent(InventoryType.Monster);
        });
        int numOfReward = SettingVariables.Reward.tierNumOfReward[LocalStorage.CurrentLocation.stageLevel];
        generateSlots(numOfReward);
        for (int i = 0; i < numOfReward; i++)
        {
            // 랜덤 확률로 몬스터 오브젝트 생성
            Transform newContent = Instantiate(contentPrefab);
            MonsterLiveStat newMonster = GeneratorFunction.generateMonsterLiveStatFromDictionaryStat(pickRandomMonsterDictionaryStat(pickRandomTier()));
            LocalStorage.Live.monsters[newMonster.saveStat.id] = newMonster;
            newContent.GetComponent<ContentController>().initContent(newMonster);
            slotControllers[i].installContent(newContent);
        }
        objectMoveController.toggle(actionAfterToggle: (i) => { curStatus = 2; });
    }

    private MonsterDictionaryStat pickRandomMonsterDictionaryStat(int tier)
    {
        int randNum = Random.Range(0, LocalDictionary.speicesByTier[tier].Count);
        return LocalDictionary.speicesByTier[tier][randNum];
    }

    private int pickRandomTier()
    {
        float randNum = Random.Range(0.0f, 1.0f);
        int res = 1;
        foreach (float standard in SettingVariables.Reward.tierRandomStandard[LocalStorage.Live.numClearDungeon])
        {
            if (randNum <= standard)
            {
                return res;
            }
            res++;
        }
        return 1;
    }

    private void generateSlots(int cnt)
    {
        slotControllers = new List<SlotController>();
        curSlotCnt = cnt;
        while (cnt > 0)
        {
            Transform newSlot = Instantiate(slotPrefab);
            newSlot.SetParent(slotGridLayout.transform);
            newSlot.localScale = Vector3.one;
            newSlot.localPosition = Vector3.zero;
            slotControllers.Add(newSlot.GetComponent<SlotController>());
            cnt--;
        }
    }

    private void truncateSlots()
    {
        foreach (SlotController slot in slotControllers)
        {
            Destroy(slot.gameObject);
        }
    }

    public void openInventoryReward()
    {
        rerollLeftCnt = SettingVariables.Reward.RerollCount.item;
        LocalStorage.UIOpenStatus.reward = true;
        titleText.text = $"아이템 보상";
        btnText.text = $"다시 뽑기";
        updateCount(isInit: true);
        btnNext.onClick.RemoveAllListeners();
        btnNext.onClick.AddListener(() =>
        {
            rerollLeftContent(InventoryType.Item);
        });
        int numOfReward = SettingVariables.Reward.tierNumOfReward[LocalStorage.CurrentLocation.stageLevel];
        generateSlots(numOfReward);
        for (int i = 0; i < numOfReward; i++)
        {
            // 랜덤 확률로 몬스터 오브젝트 생성
            Transform newContent = Instantiate(contentPrefab);
            ItemLiveStat newItem = GeneratorFunction.generateItemLiveStatFromDictionaryStat(pickRandomItemDictionaryStat(pickRandomTier()));
            LocalStorage.Live.items[newItem.saveStat.id] = newItem;
            newContent.GetComponent<ContentController>().initContent(newItem);
            slotControllers[i].installContent(newContent);
        }
        objectMoveController.toggle(actionAfterToggle: (i) => { curStatus = 4; });
    }
    private ItemDictionaryStat pickRandomItemDictionaryStat(int tier)
    {
        int randNum = Random.Range(0, LocalDictionary.itemsByTier[tier].Count);
        return LocalDictionary.itemsByTier[tier][randNum];
    }

    public void closeReward()
    {
        objectMoveController.toggle();
        truncateSlots();
        LocalStorage.UIOpenStatus.reward = false;
        switch (curStatus)
        {
            case 2:
                // 몬스터 보상 종료
                curStatus = 3;
                break;
            case 4:
                // 아이템 보상 종료
                curStatus = 5;
                break;
        }

    }

    private bool checkAllReceived()
    {
        foreach (SlotController slotController in slotControllers)
        {
            if (slotController.isOccupied()) return false;
        }
        return true;
    }

    private void updateCount(bool isInit = false)
    {
        // 텍스트 업데이트 후 1 차감
        cntText.text = rerollLeftCnt > 0 ? $"남은 횟수: {rerollLeftCnt}" : $"리롤 불가!";
        if (!isInit) --rerollLeftCnt;
        if (rerollLeftCnt == 0) updateButtonText();
    }

    private void updateButtonText()
    {
        switch (curStatus)
        {
            case 2:
            case 4:
                // 리롤 종료 <- 근데 보상은 남은
                btnText.text = $"전부 수령 필요";
                btnNext.onClick.RemoveAllListeners();
                break;
            case 6:
            case 7:
                // 현재 보상 종료 <- 보상 전부 수령한
                cntText.text = $"";
                btnText.text = $"완료";
                btnNext.onClick.RemoveAllListeners();
                btnNext.onClick.AddListener(() =>
                {
                    closeReward();
                });
                break;
        }
    }

    private void rerollLeftContent(InventoryType type)
    {
        if (rerollLeftCnt > 0)
        {
            // 남은 컨텐츠 리롤
            if (type == InventoryType.Monster)
            {

            }
            else if (type == InventoryType.Item)
            {

            }
            updateCount();
        }
    }
}
