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
        titleText.text = $"몬스터 보상";
        generateSlots(3);
        objectMoveController.toggle();
    }

    private void generateSlots(int cnt)
    {
        curSlotCnt = cnt;
        while (cnt > 0)
        {
            Transform newSlot = Instantiate(slotPrefab);
            newSlot.SetParent(slotGridLayout.transform);
            newSlot.localScale = Vector3.one;
            cnt--;
        }
    }

    private void truncateSlots()
    {
        while (curSlotCnt > 0)
        {
            Destroy(slotGridLayout.transform.GetChild(0).gameObject);
            curSlotCnt--;
        }
    }

    private void generateItem()
    {

    }

    public void openInventoryReward()
    {
        titleText.text = $"아이템 보상";
        objectMoveController.toggle();
    }

    public void closeReward()
    {
        objectMoveController.toggle();
        truncateSlots();
    }
}
