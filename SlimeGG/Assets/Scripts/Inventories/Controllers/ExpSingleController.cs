using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExpSingleController : MonoBehaviour
{
    private TextMeshProUGUI elementText;
    private TextMeshProUGUI amountText;

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
    }

    private void initSetting()
    {
        elementText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        amountText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        isInit = true;
    }

    private void adjustSize()
    {
        elementText.fontSize = MainGameManager.adjustFontSize * sizeRatio;
        amountText.fontSize = MainGameManager.adjustFontSize * sizeRatio;
        elementText.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize * 0.4f * sizeRatio,
            elementText.GetComponent<RectTransform>().sizeDelta.y * sizeRatio
            );
        amountText.GetComponent<RectTransform>().sizeDelta = new Vector2(
            MainGameManager.screenUnitSize * 0.4f * sizeRatio,
            elementText.GetComponent<RectTransform>().sizeDelta.y * sizeRatio
            );
        amountText.GetComponent<RectTransform>().anchoredPosition = Vector2.left * MainGameManager.screenUnitSize * 0.4f * sizeRatio;
    }

    public void destoySelf()
    {
        Destroy(transform.gameObject);
    }

    public void initInfo(ElementStat expStat)
    {
        // 값 연결
    }

    public void setSizeRatio(float ratio)
    {
        this.sizeRatio = ratio;
    }
}
