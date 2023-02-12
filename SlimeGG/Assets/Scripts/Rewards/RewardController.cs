using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardController : MonoBehaviour
{
    [SerializeField]
    private Transform slotPrefab, contentPrefab;

    private TextMeshProUGUI titleText;
    private ObjectMoveController objectMoveController;
    private GridLayoutGroup slotGridLayout;
    private int curSlotCnt;
    private List<SlotController> slotControllers;
    // Start is called before the first frame update
    void Start()
    {
        titleText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        titleText.text = $"";
        objectMoveController = GetComponent<ObjectMoveController>();
        slotGridLayout = transform.GetChild(1).GetComponent<GridLayoutGroup>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void openMonsterReward()
    {
        LocalStorage.UIOpenStatus.reward = true;
        titleText.text = $"몬스터 보상";
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
        objectMoveController.toggle();
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
        foreach(SlotController slot in slotControllers)
        {
            Destroy(slot.gameObject);
        }
    }

    public void openInventoryReward()
    {
        LocalStorage.UIOpenStatus.reward = true;
        titleText.text = $"아이템 보상";
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
        objectMoveController.toggle();
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
    }
}
