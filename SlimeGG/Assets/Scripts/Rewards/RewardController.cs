using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewardController : MonoBehaviour
{
    private TextMeshProUGUI titleText;
    private ObjectMoveController objectMoveController;
    // Start is called before the first frame update
    void Start()
    {
        titleText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        titleText.text = $"";
        objectMoveController = GetComponent<ObjectMoveController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void openMonsterReward()
    {
        titleText.text = $"몬스터 보상";
        objectMoveController.toggle();
    }

    public void openInventoryReward()
    {
        titleText.text = $"아이템 보상";
        objectMoveController.toggle();
    }

    public void closeReward()
    {
        objectMoveController.toggle();
    }
}
