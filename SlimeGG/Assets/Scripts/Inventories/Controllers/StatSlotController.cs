using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatSlotController : MonoBehaviour
{
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI amountText;
    private TextMeshProUGUI plusText;
    private TextMeshProUGUI correctionText;

    private bool isInit = false;

    // Start is called before the first frame update
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
        }
        else
        {
            initSetting();
        }
    }

    public void initBaseInfo(BasicStat basicStat)
    {
        if (basicStat == null)
        {
            nameText.text = $"";
            amountText.text = $"";
            plusText.text = $"";
            correctionText.text = $"";
            return;
        }
        switch (basicStat.name)
        {
            case BasicStatEnum.hp:
            case BasicStatEnum.def:
            case BasicStatEnum.atk:
            case BasicStatEnum.spd:
                nameText.text = $"{basicStat.name}";
                break;
            case BasicStatEnum.position:
                break;
            case BasicStatEnum.timeCoolCycle:
                nameText.text = $"CooTR";
                break;
            case BasicStatEnum.timeCastingCycle:
                nameText.text = $"CasTR";
                break;
            default: break;
        }
        amountText.text = $"{basicStat.amount}";
        initCorrectionInfo(null);
    }

    public void initCorrectionInfo(BasicStat basicStat)
    {
        if (basicStat != null)
        {
            plusText.text = basicStat.amount < 0 ? $"-" : "+";
            correctionText.text = basicStat.amount >= 0 ? $"{basicStat.amount}" : $"{-basicStat.amount}";
        }
        else
        {
            plusText.text = $"";
            correctionText.text = $"";
        }
    }

    private void initSetting()
    {
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        nameText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        amountText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        plusText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        correctionText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        nameText.text = "";
        amountText.text = "";
        plusText.text = "";
        correctionText.text = "";
        isInit = true;
        adjustSize();
    }

    private void adjustSize()
    {
        nameText.fontSize = MainGameManager.adjustFontSize;
        amountText.fontSize = MainGameManager.adjustFontSize;
        plusText.fontSize = MainGameManager.adjustFontSize;
        correctionText.fontSize = MainGameManager.adjustFontSize;
        GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize * 2f,
            MainGameManager.screenUnitSize * 0.6f
            );
        nameText.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize * 0.5f,
            MainGameManager.screenUnitSize * 0.6f
            );
        nameText.GetComponent<RectTransform>().anchoredPosition = Vector2.right * MainGameManager.screenUnitSize * 0.1f;

        amountText.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize * 0.4f,
            MainGameManager.screenUnitSize * 0.6f
            );
        amountText.GetComponent<RectTransform>().anchoredPosition = Vector2.right * MainGameManager.screenUnitSize * 0.7f;

        plusText.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize * 0.2f,
            MainGameManager.screenUnitSize * 0.6f
            );
        plusText.GetComponent<RectTransform>().anchoredPosition = Vector2.right * MainGameManager.screenUnitSize * 1.2f;

        correctionText.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize * 0.4f,
            MainGameManager.screenUnitSize * 0.6f
            );
        correctionText.GetComponent<RectTransform>().anchoredPosition = Vector2.right * MainGameManager.screenUnitSize * 1.5f;
    }
}
