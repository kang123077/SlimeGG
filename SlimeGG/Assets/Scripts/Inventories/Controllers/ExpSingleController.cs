using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExpSingleController : MonoBehaviour
{
    private TextMeshProUGUI elementText;
    private TextMeshProUGUI amountText;
    private TextMeshProUGUI expectText;

    private bool isInit = false;
    private float sizeRatio = 1f;
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

    private void initSetting()
    {
        elementText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        amountText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        expectText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        isInit = true;
        adjustSize();
    }

    private void adjustSize()
    {
        elementText.fontSize = MainGameManager.adjustFontSize * sizeRatio;
        amountText.fontSize = MainGameManager.adjustFontSize * sizeRatio;
        expectText.fontSize = MainGameManager.adjustFontSize * sizeRatio;
        elementText.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize * 0.3f * sizeRatio,
            elementText.GetComponent<RectTransform>().sizeDelta.y * sizeRatio
            );
        amountText.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize * 0.3f * sizeRatio,
            elementText.GetComponent<RectTransform>().sizeDelta.y * sizeRatio
            );
        amountText.GetComponent<RectTransform>().anchoredPosition = Vector2.right * MainGameManager.screenUnitSize * 0.4f * sizeRatio;
        expectText.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize * 0.3f * sizeRatio,
            elementText.GetComponent<RectTransform>().sizeDelta.y * sizeRatio
            );
        expectText.GetComponent<RectTransform>().anchoredPosition = Vector2.right * MainGameManager.screenUnitSize * 0.7f * sizeRatio;
    }

    public void destoySelf()
    {
        Destroy(transform.gameObject);
    }

    public void initInfo(ElementStat expStat)
    {
        if (!isInit) initSetting();
        elementText.text = expStat.name.ToString();
        amountText.text = expStat.amount.ToString();
        expectText.text = $"";
    }

    public void initExpectAmount(float amount)
    {
        expectText.text = $"+ {amount}";
    }

    public void setSizeRatio(float ratio)
    {
        this.sizeRatio = ratio;
    }
}
