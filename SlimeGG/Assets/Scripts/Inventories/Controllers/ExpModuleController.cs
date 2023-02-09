using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpModuleController : MonoBehaviour
{
    [SerializeField]
    Transform expSinglePrefab;
    private TextMeshProUGUI expText;
    private GridLayoutGroup expContainer;
    private Dictionary<ElementEnum, ExpSingleController> curExpController = new Dictionary<ElementEnum, ExpSingleController>();

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
    }

    private void initSetting()
    {
        if (isInit) return;
        expText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        expContainer = transform.GetChild(1).GetComponent<GridLayoutGroup>();
        isInit = true;
    }

    private void adjustSize()
    {
        expText.fontSize = MainGameManager.adjustFontSize;
        expText.GetComponent<RectTransform>().sizeDelta = new Vector2(
            expText.GetComponent<RectTransform>().sizeDelta.x,
            MainGameManager.screenUnitSize * 0.2f
            );
        expContainer.GetComponent<RectTransform>().sizeDelta = Vector2.right *
            MainGameManager.screenUnitSize;
        expContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            0f,
            -MainGameManager.screenUnitSize * 0.4f
            );
        expContainer.cellSize = new Vector2(
            MainGameManager.screenUnitSize,
            MainGameManager.screenUnitSize * 0.4f
            );
        expContainer.spacing = Vector2.up * MainGameManager.screenUnitSize * 0.1f;
    }

    public void initInfo(List<ElementEnum> elements, List<ElementStat> expStats)
    {
        if (!isInit) initSetting();
        foreach (ExpSingleController expSingle in curExpController.Values)
        {
            expSingle.destoySelf();
        }
        curExpController = new Dictionary<ElementEnum, ExpSingleController>();
        Transform temp;
        foreach (ElementStat expStat in expStats)
        {
            temp = Instantiate(expSinglePrefab);
            curExpController[expStat.name] = temp.GetComponent<ExpSingleController>();
            temp.SetParent(expContainer.transform);
            temp.localScale = Vector3.one;
            temp.GetComponent<ExpSingleController>().initInfo(expStat);
        }
    }
}